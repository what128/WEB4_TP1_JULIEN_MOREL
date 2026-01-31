using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP1_Web4.Data;
using TP1_Web4.Models;
using Microsoft.EntityFrameworkCore;

namespace TP1_Web4.Pages.Evenements
{
    public class IndexModel : PageModel
    {

        public List<Evenement> Evenements { get; set; }

        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task OnGet()
        {
            Evenements = await _context.Evenements.Include(a => a.Categorie).ToListAsync();
        }
    }
}
