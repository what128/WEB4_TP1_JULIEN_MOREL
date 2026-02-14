using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    [Authorize]
    public class ParticiperModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        /// <summary>
        /// Initialise la page web Participer avec les services nécessaires.
        /// </summary>
        /// <param name="context">L'accès à la base de données.</param>
        /// <param name="userManager">Le gestionnaire d'utilisateur</param>
        public ParticiperModel(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Evenement? Evenement { get; set; }

        /// <summary>
        /// Ajoute l'id de l'utilisateur dans la liste des participants d'un évènement, s'il n'est pas encore présent.
        /// </summary>
        /// <param name="id">L'id de l'évènement auquel participer</param>
        /// <returns>La page web vers laquelle rediriger après l'ajout de la participation à l'évènement</returns>
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            Utilisateur user = await _userManager.GetUserAsync(User);
            Evenement = await _context.Evenements.FirstOrDefaultAsync(e => e.Id == id);
            if (Evenement is not null)
            {
                if (!(Evenement.Participants.Contains(user.Id)))
                    Evenement.Participants.Add(user.Id);
            }
            _context.Attach(Evenement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["Confirmation"] = "Vous participez désormais à cet évènement.";
            return RedirectToPage("../Index");
        }
    }
}
