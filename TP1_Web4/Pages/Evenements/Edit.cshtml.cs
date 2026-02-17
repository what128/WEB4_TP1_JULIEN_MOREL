using Microsoft.AspNetCore.Authorization;
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
    public class EditModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        ///  Initialise la page web Edit avec les services nécessaires.
        /// </summary>
        /// <param name="context">Le contexte de la base de données</param>
        /// <param name="configuration">la configuration de l'environnement hôte</param>
        /// <param name="environment">Une variable représentant l'environnement hôte.</param>
        public EditModel(TP1_Web4.Data.ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
        }

        [BindProperty]
        public Evenement Evenement { get; set; } = default!;

        [BindProperty]
        [DisplayName("Image")]
        public IFormFile? ImageUpload { get; set; }

        public string ImageFolder { get; set; }


        /// <summary>
        /// Charge le formulaire de modification d'un évènement.
        /// </summary>
        /// <param name="id">L'id de l'évènement à modifier</param>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement =  await _context.Evenements.Include(e => e.Categorie).Include(e => e.Auteur).FirstOrDefaultAsync(m => m.Id == id);
            if (evenement == null)
            {
                return NotFound();
            }
            Evenement = evenement;
            if (Evenement.Auteur.UserName != User.Identity.Name)
                return Forbid();
            ImageFolder = _configuration["Images:PublicUrl"];
           ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Id");
            return Page();
        }

        /// <summary>
        /// Valide les informations de l'évènement, modifie celui-ci dans la base de données, et redirige vers la page des évènements de l'utilisateur.
        /// </summary>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ImageUpload is not null)
            {
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
            }

            _context.Attach(Evenement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvenementExists(Evenement.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["Confirmation"] = "L'évènement a bien été modifié.";
            return RedirectToPage("./Index");
        }

        /// <summary>
        /// Vérifie si un évènement existe dans la base de données.
        /// </summary>
        /// <param name="id">L'id de l'évènement à rechercher.</param>
        /// <returns>Vrai si l'évènement existe, faux autrement.</returns>
        private bool EvenementExists(Guid id)
        {
            return _context.Evenements.Any(e => e.Id == id);
        }
    }
}
