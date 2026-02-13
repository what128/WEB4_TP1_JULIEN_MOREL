using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<Utilisateur> _userManager;

        public CreateModel(TP1_Web4.Data.ApplicationDbContext context, IConfiguration configuration,
            IWebHostEnvironment environment, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
            _userManager = userManager;
        }

        [BindProperty]
        public Evenement Evenement { get; set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            ViewData["CategorieId"] = new SelectList(_context.Categories.Where(c => c.Archive == false), "Id", "Nom");
            Utilisateur user = await _userManager.GetUserAsync(User);
            Evenement = new Evenement();
            Evenement.AuteurId = user.Id;
            return Page();
        }


        public List<Evenement> Evenements { get; set; }

        [BindProperty]
        [DisplayName("Image")]
        [Required(ErrorMessage = "Vous devez prendre une image")]
        public IFormFile ImageUpload { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Evenements = await _context.Evenements.Where(
                e => e.Titre == Evenement.Titre
                && e.DateDebut.Date == Evenement.DateDebut.Date
                && e.Ville == Evenement.Ville
                && e.DateFin < DateTime.Now
                && e.Annule == false).ToListAsync();
            if (Evenements.Count > 0)
            {
                ModelState.AddModelError("", "Une évènement ayant des caractéristiques similaires existe déjà.");
                ViewData["CategorieId"] = new SelectList(_context.Categories.Where(c => c.Archive == false), "Id", "Nom");
                return Page();
            }

            if (Evenement.DateFin <= Evenement.DateDebut)
            {
                ModelState.AddModelError("Evenement.DateFin", "La date de fin d'un évènement doit être plus tard que la date de début de celui-ci.");
                ViewData["CategorieId"] = new SelectList(_context.Categories.Where(c => c.Archive == false), "Id", "Nom");
                return Page();
            }            
            
            Evenement.Participants = new List<string>();
            Utilisateur user = await _userManager.GetUserAsync(User);
            Evenement.AuteurId = user.Id;
            Evenement.Participants.Add(user.Id);
            Evenement.Annule = false;
            if (!ModelState.IsValid)
            {
                ViewData["CategorieId"] = new SelectList(_context.Categories.Where(c => c.Archive == false), "Id", "Nom");
                return Page();
            }

            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            string extension = Path.GetExtension(ImageUpload.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("ImageUpload", "L'image n'a pas une extension valide (.jpg, .jpeg, .png, .webp).");
                ViewData["CategorieId"] = new SelectList(_context.Categories.Where(c => c.Archive == false), "Id", "Nom");
                return Page();
            }

            string uploadFolder = _configuration["Images:UploadPath"];
            string uploadPath = Path.Combine(_environment.WebRootPath, uploadFolder);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string fileName = $"{Guid.NewGuid()}{extension}";

            string filePath = Path.Combine(uploadPath, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageUpload.CopyToAsync(stream);
            }

            Evenement.Image = fileName;

            _context.Evenements.Add(Evenement);
            await _context.SaveChangesAsync();
            TempData["Confirmation"] = "L'évènement a bien été créé.";
            return RedirectToPage("./Index");
        }
    }
}
