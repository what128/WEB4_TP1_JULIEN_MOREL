using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Data;
using TP1_Web4.Data.Migrations;
using TP1_Web4.Models;

namespace TP1_Web4.Pages
{
    public class IndexModel : PageModel
    {
        public List<Evenement> Evenements { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public IndexModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public string ImageFolder { get; set; }

        public async Task OnGet()
        {
            ImageFolder = _configuration["Images:PublicUrl"];
            Evenements = await _context.Evenements.Include(a => a.Categorie).ToListAsync();
            //var result = await _roleManager.CreateAsync(new IdentityRole("Utilisateur"));
            //var result2 = await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }
    }
}
