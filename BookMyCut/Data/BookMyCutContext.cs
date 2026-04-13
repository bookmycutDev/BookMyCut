using Microsoft.EntityFrameworkCore;
using BookMyCut.Models;

namespace BookMyCut.Data
{
    /// Classe de contexte pour Entity Framework Core.
    public class BookMyCutContext : DbContext
    {
        //table "Utilisateurs" dans BD
        //requêtes LINQ 
        public DbSet<Utilisateur> Utilisateurs { get; set; }

        ///connexion a BD
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Définit l'utilisation SQLite et nom fichier de BD
            
            options.UseSqlite("Data Source=bookmycut.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Empêche la création de deux comptes avec la même adresse email au niveau de la base.
            modelBuilder.Entity<Utilisateur>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}