using System.ComponentModel.DataAnnotations;

namespace BookMyCut.Models
{
    public enum RoleUtilisateur { Client, Coiffeur, Admin }

    public class Utilisateur
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string NomComplet { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [StringLength(150, ErrorMessage = "L'email ne peut pas dépasser 150 caractères")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string MotDePasse { get; set; } = string.Empty;

        public RoleUtilisateur Role { get; set; } = RoleUtilisateur.Client;
    }
}
