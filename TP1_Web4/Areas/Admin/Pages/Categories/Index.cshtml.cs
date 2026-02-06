using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Areas.Admin.Pages.Shared.Categories
{
    public class IndexModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        public IndexModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Categorie> Categorie { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Categorie = await _context.Categories.ToListAsync();
        }
    }
}
