using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookMyCut.Tests.Repositories
{
    public class ServiceRepositoryTests
    {
        private BookMyCutContext CreerContexte()
        {
            var options = new DbContextOptionsBuilder<BookMyCutContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BookMyCutContext(options);
        }

        [Fact]
        public async Task AjouterService()
        {
            using var contexte = CreerContexte();
            var repo = new ServiceRepository(contexte);

            var service = new Service
            {
                Nom = "Coupe",
                DureeMinutes = 30,
                Prix = 25m
            };

            await repo.AjouterAsync(service);

            Assert.Equal(1, contexte.Services.Count());
        }

        [Fact]
        public async Task ObtenirTousServices()
        {
            using var contexte = CreerContexte();
            var repo = new ServiceRepository(contexte);

            await repo.AjouterAsync(new Service
            {
                Nom = "Coupe",
                DureeMinutes = 30,
                Prix = 25m
            });

            var liste = await repo.ObtenirTousAsync();

            Assert.Single(liste);
        }

        [Fact]
        public async Task ModifierService()
        {
            using var contexte = CreerContexte();
            var repo = new ServiceRepository(contexte);

            var service = new Service
            {
                Nom = "Coupe",
                DureeMinutes = 30,
                Prix = 25m
            };

            await repo.AjouterAsync(service);

            service.Nom = "coupe";

            await repo.ModifierAsync(service);

            var resultat = await contexte.Services.FindAsync(service.Id);

            Assert.NotNull(resultat);
            Assert.Equal("coupe", resultat.Nom);
        }

        [Fact]
        public async Task SupprimerService()
        {
            using var contexte = CreerContexte();
            var repo = new ServiceRepository(contexte);

            var rdv = new RendezVous
            {
                ClientId = 1,
                CoiffeurId = 2,
                ServiceId = 1,
                DateHeure = new DateTime(2026, 5, 11, 14, 0, 0),
                Statut = "Confirmé"
            };

            contexte.RendezVous.Add(rdv);
            await contexte.SaveChangesAsync();

            await repo.SupprimerAsync(rdv.Id);

            var resultat = await contexte.RendezVous.FindAsync(rdv.Id);

            Assert.NotNull(resultat);
            Assert.Equal("Annulé", resultat.Statut);
        }
    }
}