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
    public class IndexModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        public IndexModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Evenement> Evenement { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Evenement = await _context.Evenements
                .Include(e => e.Categorie).Where(e => e.Auteur == User.Identity.Name).ToListAsync();
        }
    }
}
