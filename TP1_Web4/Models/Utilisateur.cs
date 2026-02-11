using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace TP1_Web4.Models
{
    public class Utilisateur: IdentityUser
    {
        [Required(ErrorMessage = "Le nom est requis")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        public string Prenom { get; set; }

        [RegularExpression(@"^[A-Z]\d[A-Z]\d[A-Z]\d$", ErrorMessage = "Le texte entré ne correspond pas à un code postal.")]
        [Required(ErrorMessage = "Le code postal est requis")]
        public string CodePostal { get; set; }

    }
}
