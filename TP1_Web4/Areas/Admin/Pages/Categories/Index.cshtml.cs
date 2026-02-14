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
    public class IndexModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        /// <summary>
        ///  Initialise la page web Index avec les services nécessaires.
        /// </summary>
        /// <param name="context">Le contexte de la base de données</param>
        public IndexModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Categorie> Categorie { get;set; } = default!;

        /// <summary>
        /// Charge la liste des catégories disponibles.
        /// </summary>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task OnGetAsync()
        {
            Categorie = await _context.Categories.Where(c => c.Archive == false).ToListAsync();
        }
    }
}
