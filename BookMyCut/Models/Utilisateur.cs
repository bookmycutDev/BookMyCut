using System.ComponentModel.DataAnnotations;

namespace BookMyCut.Models
{
    public enum RoleUtilisateur { Client, Coiffeur, Admin }

    public class Utilisateur
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string NomComplet { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string MotDePasse { get; set; }

        public RoleUtilisateur Role { get; set; } = RoleUtilisateur.Client;
    }
}