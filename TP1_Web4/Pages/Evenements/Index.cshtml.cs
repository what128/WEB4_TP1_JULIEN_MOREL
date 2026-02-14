using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        /// <summary>
        ///  Initialise la page web Index avec les services nécessaires.
        /// </summary>
        /// <param name="context">Le contexte de la base de données</param>
        /// <param name="userManager">Le gestionnaire des utilisateurs.</param>
        public IndexModel(TP1_Web4.Data.ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Evenement> Evenement { get;set; } = default!;

        public Utilisateur Utilisateur { get; set; }

        /// <summary>
        /// Charge la liste des évènements créées par l'utilisateur.
        /// </summary>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task OnGetAsync()
        {
            Utilisateur user = await _userManager.GetUserAsync(User);
            Evenement = await _context.Evenements
                .Include(e => e.Categorie).Include(e => e.Auteur).Where(e => e.AuteurId == user.Id).ToListAsync();
        }
    }
}
