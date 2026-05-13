using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookMyCut.Tests.Repositories
{
    public class RendezVousRepositoryTests
    {
        private BookMyCutContext CreerContexte()
        {
            var options = new DbContextOptionsBuilder<BookMyCutContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BookMyCutContext(options);
        }

        [Fact]
        public async Task AjouterRendezVous()
        {
            using var contexte = CreerContexte();
            var repo = new RendezVousRepository(contexte);

            var rdv = new RendezVous
            {
                ClientId = 1,
                CoiffeurId = 2,
                ServiceId = 1,
                DateHeure = new DateTime(2026, 5, 11, 14, 0, 0),
                Statut = "Confirmé"
            };

            await repo.AjouterAsync(rdv);

            Assert.Equal(1, contexte.RendezVous.Count());
        }

        [Fact]
        public async Task ObtenirRendezVousParClient()
        {
            using var contexte = CreerContexte();
            var repo = new RendezVousRepository(contexte);

            var service = new Service
            {
                Nom = "Coupe",
                DureeMinutes = 30,
                Prix = 25m
            };

            var coiffeur = new Utilisateur
            {
                NomComplet = "Coiffeur Test",
                Email = "coiffeur@test.com",
                MotDePasse = "123",
                Role = RoleUtilisateur.Coiffeur
            };

            contexte.Services.Add(service);
            contexte.Utilisateurs.Add(coiffeur);
            await contexte.SaveChangesAsync();

            await repo.AjouterAsync(new RendezVous
            {
                ClientId = 1,
                CoiffeurId = coiffeur.Id,
                ServiceId = service.Id,
                DateHeure = new DateTime(2026, 5, 11, 14, 0, 0),
                Statut = "Confirmé"
            });

            var liste = await repo.ObtenirParClientIdAsync(1);

            Assert.Single(liste);
        }

        [Fact]
        public async Task AnnulerRendezVous()
        {
            using var contexte = CreerContexte();
            var repo = new RendezVousRepository(contexte);

            var rdv = new RendezVous
            {
                ClientId = 1,
                CoiffeurId = 2,
                ServiceId = 1,
                DateHeure = new DateTime(2026, 5, 11, 14, 0, 0),
                Statut = "Confirmé"
            };

            await repo.AjouterAsync(rdv);
            await repo.AnnulerAsync(rdv.Id);

            var resultat = await contexte.RendezVous.FindAsync(rdv.Id);

            Assert.NotNull(resultat);
            Assert.Equal("Annulé", resultat.Statut);
        }

        [Fact]
        public async Task ModifierRendezVous()
        {
            using var contexte = CreerContexte();
            var repo = new RendezVousRepository(contexte);

            var rdv = new RendezVous
            {
                ClientId = 1,
                CoiffeurId = 2,
                ServiceId = 1,
                DateHeure = new DateTime(2026, 5, 11, 14, 0, 0),
                Statut = "Confirmé"
            };

            await repo.AjouterAsync(rdv);

            rdv.ServiceId = 2;
            rdv.DateHeure = new DateTime(2026, 5, 12, 15, 0, 0);

            await repo.ModifierAsync(rdv);

            var resultat = await contexte.RendezVous.FindAsync(rdv.Id);

            Assert.NotNull(resultat);
            Assert.Equal(2, resultat.ServiceId);
            Assert.Equal(new DateTime(2026, 5, 12, 15, 0, 0), resultat.DateHeure);
        }

        
    }
}
