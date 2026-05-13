using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookMyCut.Tests.Repositories
{
    public class UtilisateurRepositoryTests
    {
        private BookMyCutContext CreerContexte()
        {
            var options = new DbContextOptionsBuilder<BookMyCutContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BookMyCutContext(options);
        }

        [Fact]
        public async Task AjouterUtilisateur()
        {
            using var contexte = CreerContexte();
            var repo = new UtilisateurRepository(contexte);

            var utilisateur = new Utilisateur
            {
                NomComplet = "test test",
                Email = "test@test.com",
                MotDePasse = "123",
                Role = RoleUtilisateur.Client
            };

            await repo.AjouterAsync(utilisateur);

            var existe = await repo.EmailExisteAsync("test@test.com");

            Assert.True(existe);
        }

        [Fact]
        public async Task ObtenirUtilisateurParEmailEtMotDePasse()
        {
            using var contexte = CreerContexte();
            var repo = new UtilisateurRepository(contexte);

            var utilisateur = new Utilisateur
            {
                NomComplet = "test test",
                Email = "test@test.com",
                MotDePasse = "123",
                Role = RoleUtilisateur.Client
            };

            await repo.AjouterAsync(utilisateur);

            var resultat = await repo.ObtenirParEmailEtMotDePasseAsync("test@test.com", "123");

            Assert.NotNull(resultat);
            Assert.Equal("test test", resultat.NomComplet);
        }

        [Fact]
        public async Task ObtenirUtilisateurParId()
        {
            using var contexte = CreerContexte();
            var repo = new UtilisateurRepository(contexte);

            var utilisateur = new Utilisateur
            {
                NomComplet = "test test",
                Email = "test@test.com",
                MotDePasse = "123",
                Role = RoleUtilisateur.Client
            };

            await repo.AjouterAsync(utilisateur);

            var resultat = await repo.ObtenirParIdAsync(utilisateur.Id);

            Assert.NotNull(resultat);
            Assert.Equal("test@test.com", resultat.Email);
        }

        [Fact]
        public async Task ModifierUtilisateur()
        {
            using var contexte = CreerContexte();
            var repo = new UtilisateurRepository(contexte);

            var utilisateur = new Utilisateur
            {
                NomComplet = "test test",
                Email = "test@test.com",
                MotDePasse = "123",
                Role = RoleUtilisateur.Client
            };

            await repo.AjouterAsync(utilisateur);

            utilisateur.NomComplet = "nouveau nom";
            await repo.ModifierAsync(utilisateur);

            var resultat = await repo.ObtenirParIdAsync(utilisateur.Id);

            Assert.NotNull(resultat);
            Assert.Equal("nouveau nom", resultat.NomComplet);
        }
    }
}