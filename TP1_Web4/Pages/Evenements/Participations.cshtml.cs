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

        public ParticipationsModel(TP1_Web4.Data.ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Evenement> Evenement { get; set; } = default!;

        public Utilisateur Utilisateur { get; set; }

        public async Task OnGet()
        {
            Utilisateur usr = await _userManager.GetUserAsync(User);
            Evenement = await _context.Evenements
               .Include(e => e.Categorie).Include(e => e.Auteur).Where(e => e.Participants.Contains(usr.Id) == true
               && e.AuteurId != usr.Id
               && e.Annule == false
               && e.DateFin > DateTime.Now).ToListAsync();
        }
    }
}
