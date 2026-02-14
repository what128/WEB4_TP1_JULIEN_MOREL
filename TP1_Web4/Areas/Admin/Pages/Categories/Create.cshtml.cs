using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Areas.Admin.Pages.Shared.Categories
{
    public class CreateModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        /// <summary>
        ///  Initialise la page web Create avec les services nécessaires.
        /// </summary>
        /// <param name="context">Le contexte de la base de données</param>
        public CreateModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Charge la page web de création d'une catégorie.
        /// </summary>
        /// <returns>Retourne la page web en question.</returns>
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Categorie Categorie { get; set; } = default!;

        /// <summary>
        /// Valide les informations de la catégorie, ajoute celle-ci dans la base de données, et redirige vers la page des catégories disponibles.
        /// </summary>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Categorie.Archive = false;

            List<Categorie> Categories = await _context.Categories.Where(c => c.Nom == Categorie.Nom && c.Archive == false).ToListAsync();
            if (Categories.Count > 0)
            {
                ModelState.AddModelError("Categorie.Nom", "Une catégorie avec ce même nom existe déjà.");
                return Page();
            }
            _context.Categories.Add(Categorie);
            await _context.SaveChangesAsync();
            TempData["Confirmation"] = "La catégorie a bien été créée";
            return RedirectToPage("./Index");
        }
    }
}
