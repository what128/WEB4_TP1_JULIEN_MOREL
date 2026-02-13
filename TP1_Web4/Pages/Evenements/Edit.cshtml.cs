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

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement =  await _context.Evenements.FirstOrDefaultAsync(m => m.Id == id);
            if (evenement == null)
            {
                return NotFound();
            }
            if (Evenement.Auteur.UserName != User.Identity.Name)
                return Forbid();
            Evenement = evenement;
           ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
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

        private bool EvenementExists(Guid id)
        {
            return _context.Evenements.Any(e => e.Id == id);
        }
    }
}
