using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public IndexModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _roleManager = roleManager;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public string ImageFolder { get; set; }

        public async Task OnGet()
        {
            ImageFolder = _configuration["Images:PublicUrl"];
            Evenements = await _context.Evenements.Include(e => e.Categorie).Where(e => e.DateDebut > DateTime.Now && e.Annule == false)
                .OrderBy(e => e.DateDebut).ThenBy(e => e.Titre).ToListAsync();

            Utilisateur = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (Utilisateur is not null)
            {
                string codePostalUtilisateur = Utilisateur.CodePostal;
                if (Evenements.Count > 0)
                {
                    Distances = new Dictionary<Guid, double>();
                    foreach (Evenement e in Evenements)
                    {
                        string codePostalEvenement = e.CodePostal;
                        string apiUrl = $"https://api.zip-codes.com/ZipCodesAPI.svc/1.0/CalculateDistance/ByZip?fromzipcode" +
                            $"={codePostalUtilisateur}&tozipcode={codePostalEvenement}&key=UQWFBF1XPN4DGNP6P7G2";

                        Distance = await _httpClient.GetFromJsonAsync<ApiEvenement>(apiUrl);

                        double distance = Distance.DistanceInKm;
                        Distances.Add(e.Id, distance);
                    }
                }
            }
            //var result = await _roleManager.CreateAsync(new IdentityRole("Utilisateur"));
            //var result2 = await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }
    }
}
