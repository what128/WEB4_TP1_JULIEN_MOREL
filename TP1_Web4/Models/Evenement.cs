namespace TP1_Web4.Models
{
    public class Evenement
    {
        public Guid Id { get; set; }

        public string Titre { get; set; }

        public string Description { get; set; }

        public string? Image { get; set; }

        public string? Auteur { get; set; }

        public List<string>? Participants { get; set; }

        public int CategorieId { get; set; }

        public Categorie? Categorie { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public string Addresse { get; set; }

        public string Ville { get; set; }

        public string CodePostal { get; set; }
    }
}
