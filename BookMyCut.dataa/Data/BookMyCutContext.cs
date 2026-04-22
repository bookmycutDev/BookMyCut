using Microsoft.EntityFrameworkCore;
using BookMyCut.Data.Models;

namespace BookMyCut.Data.Data
{
    public class BookMyCutContext : DbContext
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=bookmycut.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utilisateur>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}