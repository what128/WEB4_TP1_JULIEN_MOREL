using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    [Authorize]
    public class ParticipationsModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        ///  Initialise la page web Participations avec les services nécessaires.
        /// </summary>
        /// <param name="context">Le contexte de la base de données</param>
        /// <param name="userManager">Le gestionnaire des utilisateurs.</param>
        /// /// <param name="configuration">La configuration de l'rodinateur hôte.</param>
        public ParticipationsModel(TP1_Web4.Data.ApplicationDbContext context, UserManager<Utilisateur> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public IList<Evenement> Evenement { get; set; } = default!;

        public string ImageFolder { get; set; }

        public Utilisateur Utilisateur { get; set; }

        /// <summary>
        /// Retire l'id de l'utilisateur dans la liste des participants d'un évènement, s'il est présent.
        /// </summary>
        /// <param name="id">L'id de l'évènement auquel annuler la participation de l'utilisateur.</param>
        /// <returns>La page web vers laquelle rediriger après la suppression de la participation à l'évènement</returns>
        public async Task<IActionResult> OnPostAnnulerParticipation(Guid id)
        {
            Utilisateur user = await _userManager.GetUserAsync(User);
            Evenement unEvenement = _context.Evenements.FirstOrDefault(e => e.Id == id);
            if (Evenement is not null)
            {
                unEvenement.Participants.Remove(user.Id);
            }
            _context.Attach(unEvenement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["Confirmation"] = "Votre participation à l'évènement a bien été retirée.";
            return Redirect("/");
        }

        /// <summary>
        /// Charge le formulaire de participations à des évènements de l'utilisateur.
        /// </summary>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task OnGetAsync()
        {
            ImageFolder = _configuration["Images:PublicUrl"];
            Utilisateur usr = await _userManager.GetUserAsync(User);
            Evenement = await _context.Evenements
               .Include(e => e.Categorie).Include(e => e.Auteur).Where(e => e.Participants.Contains(usr.Id) == true
               && e.AuteurId != usr.Id
               && e.Annule == false
               && e.DateFin > DateTime.Now).ToListAsync();
        }
    }
}
