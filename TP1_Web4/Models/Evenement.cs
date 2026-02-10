using System.ComponentModel;

namespace TP1_Web4.Models
{
    public class Evenement
    {
        public Guid Id { get; set; }

        public string Titre { get; set; }

        public string Description { get; set; }

        [DisplayName("Annulé")]
        public bool? Annule { get; set; }

        public string? Image { get; set; }

        public string? Auteur { get; set; }

        public List<string>? Participants { get; set; }

        public int CategorieId { get; set; }

        public Categorie? Categorie { get; set; }

        [DisplayName("Date de début")]
        public DateTime DateDebut { get; set; }

        [DisplayName("Date de fin")]
        public DateTime DateFin { get; set; }

        public string Addresse { get; set; }

        public string Ville { get; set; }

        [DisplayName("Code postal")]
        public string CodePostal { get; set; }
    }
}
