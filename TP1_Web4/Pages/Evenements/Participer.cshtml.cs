using Microsoft.AspNetCore.Authorization;
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

        public ParticiperModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Evenement? Evenement { get; set; }
        public async Task<IActionResult> OnGet(Guid? id)
        {
            Evenement = await _context.Evenements.FirstOrDefaultAsync(e => e.Id == id);
            if (Evenement is not null)
            {
                if (!(Evenement.Participants.Contains(User.Identity.Name)))
                    Evenement.Participants.Add(User.Identity.Name);
            }
            _context.Attach(Evenement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("../Index");
        }
    }
}
