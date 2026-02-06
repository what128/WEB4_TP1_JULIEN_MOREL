using Microsoft.AspNetCore.Authorization;
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

        public AnnulerParticipationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Evenement? Evenement { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            Evenement = _context.Evenements.FirstOrDefault(e => e.Id == id);
            if (Evenement is not null)
            {
                Evenement.Participants.Remove(User.Identity.Name);
            }
            _context.Attach(Evenement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("../Index");
        }
    }
}
