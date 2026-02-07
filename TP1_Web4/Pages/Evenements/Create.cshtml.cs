using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public CreateModel(TP1_Web4.Data.ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
        ViewData["CategorieId"] = new SelectList(_context.Categories.Where(c => c.Archive == false), "Id", "Nom");
            return Page();
        }

        [BindProperty]
        public Evenement Evenement { get; set; } = default!;

        [BindProperty]
        [DisplayName("Image")]
        [Required(ErrorMessage = "Vous devez prendre une image")]
        public IFormFile ImageUpload { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Evenement.Auteur = User.Identity.Name;
            Evenement.Participants = new List<string>();
            Evenement.Participants.Add(User.Identity.Name);
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

            return RedirectToPage("./Index");
        }
    }
}
