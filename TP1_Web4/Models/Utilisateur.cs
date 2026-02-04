using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace TP1_Web4.Models
{
    public class Utilisateur: IdentityUser
    {
        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string CodePostal { get; set; }
    }
}
