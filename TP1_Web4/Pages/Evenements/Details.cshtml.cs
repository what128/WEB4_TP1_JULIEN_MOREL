using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    public class DetailsModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly UserManager<Utilisateur> _userManager;

        /// <summary>
        ///  Initialise la page web Details avec les services nécessaires.
        /// </summary>
        /// <param name="context">Le contexte de la base de données</param>
        /// <param name="configuration">la configuration de l'environnement hôte</param>
        /// <param name="httpClient">Une variable client http qui permet d'effectuer des requêtes avec ce protocole.</param>
        /// <param name="userManager">Le gestionnaire des utilisateurs.</param>
        public DetailsModel(TP1_Web4.Data.ApplicationDbContext context, HttpClient httpClient,
            IConfiguration configuration, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
            _userManager = userManager;
        }

        [BindProperty]
        public int Id { get; set; }

        public ApiEvenement Distance { get; set; }
        public Utilisateur Utilisateur { get; set; }

        public  Utilisateur Usr { get; set; }

        public string ImageFolder { get; set; }

        public Evenement Evenement { get; set; } = default!;

        /// <summary>
        /// Retire l'id de l'utilisateur dans la liste des participants d'un évènement, s'il est présent.
        /// </summary>
        /// <param name="id">L'id de l'évènement auquel annuler la participation de l'utilisateur.</param>
        /// <returns>La page web vers laquelle rediriger après la suppression de la participation à l'évènement</returns>
        public async Task<IActionResult> OnPostAnnulerParticipation(Guid id)
        {
            Utilisateur user = await _userManager.GetUserAsync(User);
            Evenement = _context.Evenements.FirstOrDefault(e => e.Id == id);
            if (Evenement is not null)
            {
                Evenement.Participants.Remove(user.Id);
            }
            _context.Attach(Evenement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["Confirmation"] = "Votre participation à l'évènement a bien été retirée.";
            return Redirect("/");
        }

        /// <summary>
        /// Ajoute l'id de l'utilisateur dans la liste des participants d'un évènement, s'il n'est pas encore présent.
        /// </summary>
        /// <param name="id">L'id de l'évènement auquel participer</param>
        /// <returns>La page web vers laquelle rediriger après l'ajout de la participation à l'évènement</returns>
        public async Task<IActionResult> OnPostParticiper(Guid id)
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
            return Redirect("/");
        }
        /// <summary>
        /// Charge la page web des détails d'un évènement.
        /// </summary>
        /// <param name="id">L'id de l'évènement à consulter.</param>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenements.Include(e => e.Categorie).Include(e => e.Auteur).FirstOrDefaultAsync(m => m.Id == id);

            if (evenement is not null)
            {
                ImageFolder = _configuration["Images:PublicUrl"];
                Evenement = evenement;

                Utilisateur = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                if (Utilisateur is not null)
                {
                    string codePostalUtilisateur = Utilisateur.CodePostal;
                    string codePostalEvenement = Evenement.CodePostal;
                    string apiUrl = $"https://api.zip-codes.com/ZipCodesAPI.svc/1.0/CalculateDistance/ByZip?fromzipcode" +
                        $"={codePostalUtilisateur}&tozipcode={codePostalEvenement}&key=7MQORTSRX4LNLDH42T0F";

                    try
                    {
                        Distance = await _httpClient.GetFromJsonAsync<ApiEvenement>(apiUrl);
                        Distance.DistanceInKm = (float)Math.Round(Distance.DistanceInKm, 2);
                    }
                    catch (Exception)
                    {
                        Distance = new ApiEvenement();
                        Distance.DistanceInKm = -1;
                    }
                    }
                if (User.Identity.IsAuthenticated)
                    Usr = await _userManager.GetUserAsync(User);
                return Page();
            }
            else
            {
                return NotFound();
            }                
        }
    }
}
