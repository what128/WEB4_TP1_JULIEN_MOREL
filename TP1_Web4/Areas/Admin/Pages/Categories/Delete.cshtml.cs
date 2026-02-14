using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Areas.Admin.Pages.Shared.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        /// <summary>
        ///  Initialise la page web Delete avec les services nécessaires.
        /// </summary>
        /// <param name="context">Le contexte de la base de données</param>
        public DeleteModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Categorie Categorie { get; set; } = default!;

        /// <summary>
        /// Charge le formulaire de confirmation de suppression d'une catégorie.
        /// </summary>
        /// <param name="id">L'id de la catégorie à archiver.</param>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (categorie is not null)
            {
                Categorie = categorie;

                return Page();
            }

            return NotFound();
        }

        /// <summary>
        /// Archive la catégorie dans la base de données, et redirige vers la page des catégories disponibles.
        /// </summary>
        /// <param name="id">L'id de la catégorie à archiver.</param>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await _context.Categories.FindAsync(id);
            if (categorie != null)
            {
                Categorie = categorie;
                List<Evenement> evenements = await _context.Evenements.Where(e =>e.DateDebut > DateTime.Now
                && e.Categorie.Nom == Categorie.Nom).ToListAsync();
                if (evenements.Count > 0)
                {
                    TempData["Erreur"] = "Impossible de supprimer la catégorie, car elle est utilisée par un évènement futur.";
                    return RedirectToPage("./Index");
                }

                Categorie.Archive = true;
                _context.Attach(Categorie).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            TempData["Confirmation"] = "La catégorie a bien été supprimée";
            return RedirectToPage("./Index");
        }
    }
}
