using Microsoft.AspNetCore.Authorization;
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

        public ParticipationsModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Evenement> Evenement { get; set; } = default!;

        public Utilisateur Utilisateur { get; set; }

        public async Task OnGet()
        {
            Evenement = await _context.Evenements
               .Include(e => e.Categorie).Where(e => e.Participants.Contains(User.Identity.Name) == true
               && e.Auteur != User.Identity.Name
               && e.Annule == false
               && e.DateFin < DateTime.Now).ToListAsync();
        }
    }
}
