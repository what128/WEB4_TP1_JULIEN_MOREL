using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Models;

namespace TP1_Web4.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public DbSet<Evenement> Evenements { get; set; }

        public DbSet<Categorie> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
