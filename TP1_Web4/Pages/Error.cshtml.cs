using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TP1_Web4.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public int StatusCode { get; set; }

        /// <summary>
        /// Charge la page d'erreur à partir du code d'erreur passé en paramètre. Si le code d'erreur n'est pas reconnu,
        /// cette page web utilisera le code 500.
        /// </summary>
        /// <param name="code">Le code de l'erreur qui est survenue.</param>
        public void OnGet(int? code)
        {
            StatusCode = code ?? 500;
        }
    }

}
