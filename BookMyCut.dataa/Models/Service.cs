using System.ComponentModel.DataAnnotations;

namespace BookMyCut.Data.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nom { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public int DureeMinutes { get; set; }
        [Required]
        public decimal Prix { get; set; }
    }
}