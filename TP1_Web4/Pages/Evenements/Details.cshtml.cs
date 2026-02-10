using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    public class DetailsModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        public DetailsModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Evenement Evenement { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenements.Include(e => e.Categorie).FirstOrDefaultAsync(m => m.Id == id);

            if (evenement is not null)
            {
                Evenement = evenement;

                return Page();
            }

            return NotFound();
        }
    }
}
