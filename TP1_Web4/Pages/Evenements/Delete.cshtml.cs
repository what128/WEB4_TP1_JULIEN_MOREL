using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        public DeleteModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Evenement Evenement { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenements.FirstOrDefaultAsync(m => m.Id == id);

            if (evenement is not null)
            {
                Evenement = evenement;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenements.FindAsync(id);
            if (evenement != null)
            {
                Evenement = evenement;
                _context.Evenements.Remove(Evenement);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
