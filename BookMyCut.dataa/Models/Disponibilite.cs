using System.ComponentModel.DataAnnotations;

namespace BookMyCut.Data.Models
{
    public class Disponibilite
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CoiffeurId { get; set; }
        public Utilisateur? Coiffeur { get; set; } // Le coiffeur est un Utilisateur avec le rôle Coiffeur

        [Required]
        public DateTime Debut { get; set; }
        [Required]
        public DateTime Fin { get; set; }

        public bool EstReserve { get; set; } = false;
    }
}