using BookMyCut.Data.Models;
using BookMyCut.Utils;
using Microsoft.EntityFrameworkCore;

namespace BookMyCut.Data.Data
{
    public class BookMyCutContext : DbContext
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<RendezVous> RendezVous { get; set; }
        public DbSet<Disponibilite> Disponibilites { get; set; }

        public BookMyCutContext(DbContextOptions<BookMyCutContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public BookMyCutContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite("Data Source=bookmycut.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Garde index unique
            modelBuilder.Entity<Utilisateur>()
                .HasIndex(u => u.Email)
                .IsUnique();

            //SEEDING 
            modelBuilder.Entity<Utilisateur>().HasData(new Utilisateur
            {
                Id = 1,
                NomComplet = "Administrateur",
                Email = "admin@test.com",
                // Hachage du mot de passe pour que la connexion fonctionne
                MotDePasse = Hash.HashPassword("admin123"),
                Role = RoleUtilisateur.Admin
            },
            new Utilisateur
            {
                Id = 2,
                NomComplet = "Aymen Kermezli",
                Email = "aymen@test.com",
                MotDePasse = Hash.HashPassword("123"),
                Role = RoleUtilisateur.Coiffeur
            },
            new Utilisateur
            {
                Id = 3,
                NomComplet = "Dusly Nestor",
                Email = "dusly@test.com",
                MotDePasse = Hash.HashPassword("123"),
                Role = RoleUtilisateur.Coiffeur
            },
                new Utilisateur
                {
                    Id = 4,
                    NomComplet = "Jonathan Riquelme",
                    Email = "jonathan@test.com",
                    MotDePasse = Hash.HashPassword("123"),
                    Role = RoleUtilisateur.Coiffeur
                },
                new Utilisateur
                {
                    Id = 5,
                    NomComplet = "Alben Matonde",
                    Email = "alben@test.com",
                    MotDePasse = Hash.HashPassword("123"),
                    Role = RoleUtilisateur.Coiffeur
                },
                new Utilisateur
                {
                    Id = 6,
                    NomComplet = "Diego Fanara",
                    Email = "diego@test.com",
                    MotDePasse = Hash.HashPassword("123"),
                    Role = RoleUtilisateur.Coiffeur
                }

         );
        }
    }
}