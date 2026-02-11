using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP1_Web4.Models
{
    public class Evenement
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Le titre de l'évènement est requis.")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "La description de l'évènement est requise.")]
        public string Description { get; set; }

        [DisplayName("Annulé")]
        public bool? Annule { get; set; }

        public string? Image { get; set; }

        [DisplayName("Organisateur")]
        public string? Auteur { get; set; }

        public List<string>? Participants { get; set; }

        public int CategorieId { get; set; }

        public Categorie? Categorie { get; set; }

        [DisplayName("Date de début")]
        public DateTime DateDebut { get; set; }

        [DisplayName("Date de fin")]
        public DateTime DateFin { get; set; }

        [Required(ErrorMessage = "L'addresse de l'évènement est requise.")]
        public string Addresse { get; set; }

        [Required(ErrorMessage = "La ville de l'évènement est requise.")]
        public string Ville { get; set; }

        [RegularExpression(@"^[A-Z]\d[A-Z]\d[A-Z]\d$", ErrorMessage = "Le texte entré ne correspond pas à un code postal.")]
        [DisplayName("Code postal")]
        public string CodePostal { get; set; }
    }
}
