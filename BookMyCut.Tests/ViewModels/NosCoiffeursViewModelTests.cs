using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.ViewModels;
using Moq;
using Xunit;

namespace BookMyCut.Tests.ViewModels
{
    public class NosCoiffeursViewModelTests
    {
        [Fact]
        public async Task ChargeSeulementLesCoiffeurs()
        {
            var utilisateurRepository = new Mock<IUtilisateurRepository>();

            utilisateurRepository.Setup(repository => repository.ObtenirTousAsync())
                .ReturnsAsync(new List<Utilisateur>
                {
                    new Utilisateur
                    {
                        NomComplet = "coiffeur Test",
                        Email = "coiffeur@test.com",
                        MotDePasse = "123",
                        Role = RoleUtilisateur.Coiffeur
                    },
                    new Utilisateur
                    {
                        NomComplet = "Client Test",
                        Email = "client@test.com",
                        MotDePasse = "123",
                        Role = RoleUtilisateur.Client
                    }
                });

            var viewModel = new NosCoiffeursViewModel(utilisateurRepository.Object);

            await Task.Delay(100);

            Assert.Single(viewModel.Coiffeurs);
            Assert.Equal("coiffeur Test", viewModel.Coiffeurs.First().NomComplet);
            Assert.Equal("CT", viewModel.Coiffeurs.First().Initiales);
        }

        [Fact]
        public async Task AucunCoiffeur()
        {
            var utilisateurRepository = new Mock<IUtilisateurRepository>();

            utilisateurRepository.Setup(repository => repository.ObtenirTousAsync())
                .ReturnsAsync(new List<Utilisateur>());

            var viewModel = new NosCoiffeursViewModel(utilisateurRepository.Object);

            await Task.Delay(100);

            Assert.Empty(viewModel.Coiffeurs);
            Assert.True(viewModel.AucunCoiffeur);
        }
    }
}