using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TP1_Web4.Data;
using TP1_Web4.Data.Migrations;
using TP1_Web4.Models;

namespace TP1_Web4.Pages
{
    public class IndexModel : PageModel
    {
        public List<Evenement> Evenements { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public ApiEvenement Distance { get; set; }

        public Utilisateur Utilisateur { get; set; }
        public Dictionary<Guid, double> Distances { get; set; }
        
        /// <summary>
        /// Initialise le modèle de la page de la liste des évènements avec les services requis.
        /// </summary>
        /// <param name="context">le contexte de la base de données.</param>
        /// <param name="roleManager">Le gestionnaire des rôles.</param>
        /// <param name="configuration">La configuration de l'enviornement hôte.</param>
        /// <param name="httpClient">Un objet client http, pour pouvoir effectuer des requêtes avec ce protocole.</param>
        public IndexModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _roleManager = roleManager;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public string ImageFolder { get; set; }

        public List<Categorie> Categories { get; set; }

        [BindProperty]
        public int CategorieId { get; set; }

        [BindProperty]
        public string Contenu { get; set; }

        /// <summary>
        /// Méthode exécutée lors de la recherche des évènements
        /// </summary>
        /// <returns>La page rafraîchie avec les éléments qui correspondent aux critères de recherche.</returns>
        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(Contenu))
                return Page();
            TempData["CategorieIdRecherche"] = CategorieId;
            TempData["ContenuRecherche"] = Contenu;
            return RedirectToPage($"/Index");
        }

        /// <summary>
        /// Charge le contenu de la page d'accueil, en ayant filtré les résultats de recherche, les évènements qui sont dans le futur
        /// et ceux qui ne sont pas annulés.
        /// </summary>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task OnGetAsync()
        {
            int? idCategorie = null;
            string? contenu = "";
            if (TempData["CategorieIdRecherche"] is not null && TempData["ContenuRecherche"] is not null)
            {
                idCategorie = (int)TempData["CategorieIdRecherche"];
                contenu = TempData["ContenuRecherche"].ToString();
            }
            ImageFolder = _configuration["Images:PublicUrl"];
            Categories = await _context.Categories.Where(c => c.Archive == false).ToListAsync();
            if (idCategorie is not null && !(string.IsNullOrEmpty(contenu)))
            {
                if (idCategorie == -1)
                {
                    Evenements = await _context.Evenements.Include(e => e.Categorie).Include(e => e.Auteur).Where(e => e.DateDebut > DateTime.Now
                      && e.Annule == false && (e.Description.ToLower().Contains(contenu.ToLower()) ||
                      (e.Titre.ToLower().Contains(contenu.ToLower()))))
                      .OrderBy(e => e.DateDebut).ThenBy(e => e.Titre).ToListAsync();
                }
                else
                {
                    Evenements = await _context.Evenements.Include(e => e.Categorie).Include(e => e.Auteur).Where(e => e.DateDebut > DateTime.Now
                     && e.Annule == false && e.CategorieId == idCategorie && (e.Description.ToLower().Contains(contenu.ToLower()) ||
                     (e.Titre.ToLower().Contains(contenu.ToLower()))))
                     .OrderBy(e => e.DateDebut).ThenBy(e => e.Titre).ToListAsync();
                }
            }
            else
            { 
                Evenements = await _context.Evenements.Include(e => e.Categorie).Include(e => e.Auteur).Where(e => e.DateDebut > DateTime.Now
                    && e.Annule == false)
                    .OrderBy(e => e.DateDebut).ThenBy(e => e.Titre).ToListAsync();
            }

            Utilisateur = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (Utilisateur is not null)
            {
                Distances = new Dictionary<Guid, double>();
                foreach (Evenement e in Evenements)
                {
                    string codePostalUtilisateur = Utilisateur.CodePostal;
                    string codePostalEvenement = e.CodePostal;
                    string apiUrl = $"https://api.zip-codes.com/ZipCodesAPI.svc/1.0/CalculateDistance/ByZip?fromzipcode" +
                        $"={codePostalUtilisateur}&tozipcode={codePostalEvenement}&key=7MQORTSRX4LNLDH42T0F";

                    try
                    {
                        Distance = await _httpClient.GetFromJsonAsync<ApiEvenement>(apiUrl);
                    }
                    catch (Exception)
                    {
                        Distance = new ApiEvenement();
                        Distance.DistanceInKm = -1;
                    }
                    Distances.Add(e.Id, Math.Round(Distance.DistanceInKm, 2));
                }
            }
        }
    }
}
