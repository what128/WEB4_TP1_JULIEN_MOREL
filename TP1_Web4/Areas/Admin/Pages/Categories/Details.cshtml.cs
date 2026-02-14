using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Areas.Admin.Pages.Shared.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        /// <summary>
        ///  Initialise la page web Details avec les services nécessaires.
        /// </summary>
        /// <param name="context">Le contexte de la base de données</param>
        public DetailsModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Categorie Categorie { get; set; } = default!;

        /// <summary>
        /// Charge la page web des détails d'une catégorie.
        /// </summary>
        /// <param name="id">L'id de la catégorie à consulter.</param>
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
    }
}
