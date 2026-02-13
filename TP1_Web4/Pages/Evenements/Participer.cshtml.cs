using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    [Authorize]
    public class ParticiperModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public ParticiperModel(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Evenement? Evenement { get; set; }
        public async Task<IActionResult> OnGet(Guid? id)
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
            return RedirectToPage("../Index");
        }
    }
}
