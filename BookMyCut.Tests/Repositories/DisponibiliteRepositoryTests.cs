using BookMyCut.Data.Data;
using BookMyCut.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookMyCut.Tests.Repositories
{
    public class DisponibiliteRepositoryTests
    {
        private BookMyCutContext CreerContexte()
        {
            var options = new DbContextOptionsBuilder<BookMyCutContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BookMyCutContext(options);
        }

        [Fact]
        public async Task AjouterPlage()
        {
            using var contexte = CreerContexte();
            var repo = new DisponibiliteRepository(contexte);

            await repo.AjouterPlageAsync(
                1,
                new DateTime(2026, 5, 11, 9, 0, 0),
                new DateTime(2026, 5, 11, 10, 0, 0),
                30
            );

            var liste = await repo.ObtenirParCoiffeurAsync(1);

            Assert.Equal(2, liste.Count);
        }

        [Fact]
        public async Task Test_ChevauchementExisteAsync()
        {
            using var contexte = CreerContexte();
            var repo = new DisponibiliteRepository(contexte);

            await repo.AjouterPlageAsync(
                1,
                new DateTime(2026, 5, 11, 9, 0, 0),
                new DateTime(2026, 5, 11, 10, 0, 0),
                30
            );

            var existe = await repo.ChevauchementExisteAsync(
                1,
                new DateTime(2026, 5, 11, 9, 30, 0),
                new DateTime(2026, 5, 11, 10, 30, 0)
            );

            Assert.True(existe);
        }

        [Fact]
        public async Task ObtenirDisponiblesParCoiffeurEtDate()
        {
            using var contexte = CreerContexte();
            var repo = new DisponibiliteRepository(contexte);

            await repo.AjouterPlageAsync(
                1,
                new DateTime(2026, 5, 11, 9, 0, 0),
                new DateTime(2026, 5, 11, 10, 0, 0),
                30
            );

            var liste = await repo.ObtenirDisponiblesParCoiffeurEtDateAsync(
                1,
                new DateTime(2026, 5, 11)
            );

            Assert.Equal(2, liste.Count);
        }

        [Fact]
        public async Task MarquerCommeReserve()
        {
            using var contexte = CreerContexte();
            var repo = new DisponibiliteRepository(contexte);

            await repo.AjouterPlageAsync(
                1,
                new DateTime(2026, 5, 11, 9, 0, 0),
                new DateTime(2026, 5, 11, 9, 30, 0),
                30
            );

            var liste = await repo.ObtenirParCoiffeurAsync(1);
            var dispo = liste.First();

            await repo.MarquerCommeReserveAsync(dispo.Id);

            var listeDispos = await repo.ObtenirDisponiblesParCoiffeurEtDateAsync(
                1,
                new DateTime(2026, 5, 11)
            );

            Assert.Empty(listeDispos);
        }

    }
}