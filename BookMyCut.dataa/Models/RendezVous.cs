using System.ComponentModel.DataAnnotations;

namespace BookMyCut.Data.Models
{
    public class RendezVous
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Utilisateur? Client { get; set; }

        [Required]
        public int CoiffeurId { get; set; }
        public Utilisateur? Coiffeur { get; set; }

        [Required]
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        [Required]
        public DateTime DateHeure { get; set; }

        public string Statut { get; set; } = "Confirmé"; // Confirmé, Annulé, Terminé
    }
}