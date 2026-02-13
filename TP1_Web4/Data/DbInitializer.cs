using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Models;

namespace TP1_Web4.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(
           ApplicationDbContext context,
           UserManager<Utilisateur> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Utilisateur"));
            }
            List<Utilisateur> usrs = new List<Utilisateur>();
            if (!await userManager.Users.AnyAsync())
            {
                Utilisateur user = new Utilisateur()
                {
                    UserName = "rickastley69",
                    Email = "rick@rick.com",
                    Nom = "Astley",
                    Prenom = "Rick",
                    CodePostal = "G1K3B8",
                    Addresse = "126 rue principale",
                    Ville = "Québec"
                };
                await userManager.CreateAsync(user, "Password123!");
                await userManager.AddToRoleAsync(user, "Admin");
                usrs.Add(user);

                Utilisateur user2 = new Utilisateur()
                {
                    UserName = "jumor010",
                    Email = "jumor010@edu.cegepgarneau.ca",
                    Nom = "Morel",
                    Prenom = "Julien",
                    CodePostal = "G1H5B9",
                    Addresse = "453 avenue Nérée-Tremblay",
                    Ville = "Sainte-Foy"
                };
                await userManager.CreateAsync(user2, "Password123!");
                await userManager.AddToRoleAsync(user2, "Admin");
                usrs.Add(user2);

                Utilisateur user3 = new Utilisateur()
                {
                    UserName = "loremipsum69",
                    Email = "lorem@ipsum.com",
                    Nom = "Ipsum",
                    Prenom = "Lorem",
                    CodePostal = "G1E3G1",
                    Addresse = "1660 boul de l'entente",
                    Ville = "Sainte-Foy"
                };
                await userManager.CreateAsync(user3, "Password123!");
                await userManager.AddToRoleAsync(user3, "Utilisateur");
                usrs.Add(user3);

                Utilisateur user4 = new Utilisateur()
                {
                    UserName = "dolorismet420",
                    Email = "dolor@ismet.com",
                    Nom = "Ismet",
                    Prenom = "Dolor",
                    CodePostal = "G1E2H3",
                    Addresse = "123 rue principale",
                    Ville = "Victoriaville"
                };
                await userManager.CreateAsync(user4, "Password123!");
                await userManager.AddToRoleAsync(user4, "Utilisateur");
                usrs.Add(user4);
            }

            if (!await context.Categories.AnyAsync())
            {
                await context.AddRangeAsync(new Categorie { Nom = "Spectacle" },
                    new Categorie { Nom = "Fête" },
                    new Categorie { Nom = "Funérailles" },
                    new Categorie { Nom = "Concert" },
                    new Categorie { Nom = "Exposition" });
                await context.SaveChangesAsync();
            }


            if (!await context.Evenements.AnyAsync())
            {
                Random rnd = new Random();
                var categorieIds = await context.Categories.Select(c => c.Id).ToListAsync();
                context.Evenements.AddRange(
                    new Evenement
                    {
                        Titre = "Lorem Ipsum",
                        Description = "Dolor sit amet",
                        Image = "default.jpg",
                        CategorieId = categorieIds[rnd.Next(categorieIds.Count)],
                        AuteurId = usrs[0].Id,
                        Participants = new List<string> { usrs[0].Id, usrs[1].Id },
                        Annule = false,
                        DateDebut = new DateTime(2026, 2, 24, 3, 52, 0),
                        DateFin = new DateTime(2026, 2, 26, 5, 52, 0),
                        Addresse = "330 52e rue ouest",
                        Ville = "Québec",
                        CodePostal = "G1H5B9"
                    },
                     new Evenement
                     {
                         Titre = "Lorem Ipsum 2",
                         Description = "Dolor sit amet 2",
                         Image = "default.jpg",
                         CategorieId = categorieIds[rnd.Next(categorieIds.Count)],
                         AuteurId = usrs[1].Id,
                         Participants = new List<string> { usrs[0].Id, usrs[1].Id },
                         Annule = false,
                         DateDebut = new DateTime(2026, 3, 11, 4, 53, 0),
                         DateFin = new DateTime(2026, 3, 11, 7, 54, 0),
                         Addresse = "1660 boul de l'Entente",
                         Ville = "Sainte-Foy",
                         CodePostal = "G1S4K2"
                     }
                    );
                await context.SaveChangesAsync();
            }
        }
    }
}
