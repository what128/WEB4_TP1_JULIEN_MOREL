using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Areas.Admin.Pages.Shared.Categories
{
    public class EditModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        /// <summary>
        ///  Initialise la page web Edit avec les services nécessaires.
        /// </summary>
        /// <param name="context">Le contexte de la base de données</param>
        public EditModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Categorie Categorie { get; set; } = default!;

        /// <summary>
        /// Charge le formulaire de modification d'une catégorie.
        /// </summary>
        /// <param name="id">L'id de la catégorie à modifier</param>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie =  await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (categorie == null)
            {
                return NotFound();
            }
            Categorie = categorie;
            return Page();
        }

        /// <summary>
        /// Valide les informations de la catégorie, modifie celle-ci dans la base de données, et redirige vers la page des catégories disponibles.
        /// </summary>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Categorie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorieExists(Categorie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["Confirmation"] = "La catégorie a bien été modifiée";
            return RedirectToPage("./Index");
        }

        /// <summary>
        /// Vérifie si une catégorie existe dans la base de données.
        /// </summary>
        /// <param name="id">L'id de la catégorie à rechercher.</param>
        /// <returns>Vrai si la catégorie existe, faux autrement.</returns>
        private bool CategorieExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
