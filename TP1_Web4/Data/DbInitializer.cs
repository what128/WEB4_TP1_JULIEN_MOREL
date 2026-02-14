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

            // =============================
            // CRÉATION DES UTILISATEURS
            // =============================
            if (!await userManager.Users.AnyAsync())
            {
                var utilisateurs = new List<Utilisateur>
                {
                    new Utilisateur
                    {
                        UserName = "admin",
                        Email = "admin@cegepgarneau.ca",
                        Nom = "Admin",
                        Prenom = "Principal",
                        CodePostal = "G1K3B8",
                        Addresse = "100 rue Admin",
                        Ville = "Québec"
                    },
                    new Utilisateur
                    {
                        UserName = "user1",
                        Email = "user1@test.com",
                        Nom = "Tremblay",
                        Prenom = "Jean",
                        CodePostal = "G1H5B9",
                        Addresse = "200 rue A",
                        Ville = "Québec"
                    },
                    new Utilisateur
                    {
                        UserName = "user2",
                        Email = "user2@test.com",
                        Nom = "Gagnon",
                        Prenom = "Marie",
                        CodePostal = "G1E3G1",
                        Addresse = "300 rue B",
                        Ville = "Lévis"
                    },
                    new Utilisateur
                    {
                        UserName = "user3",
                        Email = "user3@test.com",
                        Nom = "Roy",
                        Prenom = "Alex",
                        CodePostal = "G1S4K2",
                        Addresse = "400 rue C",
                        Ville = "Sainte-Foy"
                    },
                    new Utilisateur
                    {
                        UserName = "user4",
                        Email = "user4@test.com",
                        Nom = "Bouchard",
                        Prenom = "Sophie",
                        CodePostal = "G1M2L3",
                        Addresse = "500 rue D",
                        Ville = "Victoriaville"
                    }
                };

                foreach (var user in utilisateurs)
                {
                    await userManager.CreateAsync(user, "Password123!");
                    usrs.Add(user);
                }

                // Attribution des rôles
                await userManager.AddToRoleAsync(utilisateurs[0], "Admin");
                for (int i = 1; i < utilisateurs.Count; i++)
                    await userManager.AddToRoleAsync(utilisateurs[i], "Utilisateur");
            }
            else
            {
                usrs = await userManager.Users.ToListAsync();
            }

            // =============================
            // CATÉGORIES (min 5)
            // =============================
            if (!await context.Categories.AnyAsync())
            {
                await context.AddRangeAsync(
                    new Categorie { Nom = "Spectacle" },
                    new Categorie { Nom = "Fête" },
                    new Categorie { Nom = "Funérailles" },
                    new Categorie { Nom = "Concert" },
                    new Categorie { Nom = "Exposition" }
                );

                await context.SaveChangesAsync();
            }

            // =============================
            // ÉVÉNEMENTS (2 par catégorie)
            // =============================
            if (!await context.Evenements.AnyAsync())
            {
                Random rnd = new Random();
                var categories = await context.Categories.ToListAsync();
                List<Evenement> evenements = new List<Evenement>();

                foreach (var cat in categories)
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        var auteur = usrs[rnd.Next(usrs.Count)];

                        evenements.Add(new Evenement
                        {
                            Titre = $"{cat.Nom} #{i}",
                            Description = $"Description de {cat.Nom} #{i}",
                            Image = "default.jpg",
                            CategorieId = cat.Id,
                            AuteurId = auteur.Id,
                            Annule = false,
                            DateDebut = DateTime.Now.AddDays(rnd.Next(1, 60)),
                            DateFin = DateTime.Now.AddDays(rnd.Next(61, 120)),
                            Addresse = $"{rnd.Next(100, 999)} rue Exemple",
                            Ville = "Québec",
                            CodePostal = "G1K3B8",
                            Participants = new List<string> {auteur.Id}
                        });
                    }
                }

                // Sauvegarder d'abord pour générer les Id
                await context.Evenements.AddRangeAsync(evenements);
                await context.SaveChangesAsync();

                var allEvents = await context.Evenements.ToListAsync();

                foreach (var user in usrs)
                {
                    var selectedEvents = allEvents
                        .OrderBy(x => rnd.Next())
                        .Where(x=> x.Participants.Contains(user.Id) == false)
                        .Take(2)
                        .ToList();

                    foreach (var ev in selectedEvents)
                    {
                        if (!ev.Participants.Contains(user.Id))
                            ev.Participants.Add(user.Id);
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
