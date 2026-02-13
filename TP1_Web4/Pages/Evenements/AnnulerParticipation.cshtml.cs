using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    public class AnnulerParticipationModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public AnnulerParticipationModel(ApplicationDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Evenement? Evenement { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            Utilisateur user = await _userManager.GetUserAsync(User);
            Evenement = _context.Evenements.FirstOrDefault(e => e.Id == id);
            if (Evenement is not null)
            {
                Evenement.Participants.Remove(user.Id);
            }
            _context.Attach(Evenement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["Confirmation"] = "Votre participation à l'évènement a bien été retirée.";
            return RedirectToPage("../Index");
        }
    }
}
