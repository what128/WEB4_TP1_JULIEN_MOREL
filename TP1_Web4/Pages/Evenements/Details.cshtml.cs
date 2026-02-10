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

        public DetailsModel(TP1_Web4.Data.ApplicationDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public ApiEvenement Distance { get; set; }
        public Utilisateur Utilisateur { get; set; }

        public string ImageFolder { get; set; }

        public Evenement Evenement { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenements.Include(e => e.Categorie).FirstOrDefaultAsync(m => m.Id == id);

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
                        $"={codePostalUtilisateur}&tozipcode={codePostalEvenement}&key=UQWFBF1XPN4DGNP6P7G2";

                    Distance = await _httpClient.GetFromJsonAsync<ApiEvenement>(apiUrl);
                    }

                return Page();
            }
            else
            {
                return NotFound();
            }                
        }
    }
}
