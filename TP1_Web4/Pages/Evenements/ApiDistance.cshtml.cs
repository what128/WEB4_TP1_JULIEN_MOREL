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
        public ApiDistanceModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task OnGet()
        {
            string apiUrl = $"https://api.zip-codes.com/ZipCodesAPI.svc/1.0/CalculateDistance/ByZip?fromzipcode={1}&tozipcode={1}&key=UQWFBF1XPN4DGNP6P7G2";

            Distance = await _httpClient.GetFromJsonAsync<ApiEvenement>(apiUrl);

            double distance = Distance.DistanceInKm;
        }
    }
}
