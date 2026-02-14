using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    public class ApiDistanceModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public ApiEvenement Distance { get; set; }
        /// <summary>
        /// Initialise la page web ApiDistance avec les services nécessaires.
        /// </summary>
        /// <param name="httpClient">Un objet client http pour pouvoir effectuer des requêtes.</param>
        public ApiDistanceModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retire l'id de l'utilisateur dans la liste des participants d'un évènement, s'il est présent.
        /// </summary>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task OnGetAsync()
        {
            string apiUrl = $"https://api.zip-codes.com/ZipCodesAPI.svc/1.0/CalculateDistance/ByZip?fromzipcode={1}&tozipcode={1}&key=UQWFBF1XPN4DGNP6P7G2";

            Distance = await _httpClient.GetFromJsonAsync<ApiEvenement>(apiUrl);

            double distance = Distance.DistanceInKm;
        }
    }
}
