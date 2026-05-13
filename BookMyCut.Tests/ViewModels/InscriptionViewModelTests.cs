using BookMyCut.Data.Repositories;
using BookMyCut.Services;
using BookMyCut.ViewModels;
using Moq;
using Xunit;

namespace BookMyCut.Tests.ViewModels
{
    public class InscriptionViewModelTests
    {
        [Fact]
        public async Task DonneesInvalides()
        {
            var repo = new Mock<IUtilisateurRepository>();
            var dialogue = new Mock<IDialogService>();

            var vm = new InscriptionViewModel(repo.Object, dialogue.Object);

            vm.Email = "";
            vm.MotDePasse = "123";
            vm.ConfirmationMotDePasse = "456";

            await vm.CreerCompteCommand.ExecuteAsync(null);

            dialogue.Verify(d => d.AfficherMessage("Données invalides."), Times.Once);
            repo.Verify(r => r.AjouterAsync(It.IsAny<BookMyCut.Data.Models.Utilisateur>()), Times.Never);
        }

        [Fact]
        public async Task EmailDejaUtilise()
        {
            var repo = new Mock<IUtilisateurRepository>();
            var dialogue = new Mock<IDialogService>();

            repo.Setup(r => r.EmailExisteAsync("test@test.com"))
                .ReturnsAsync(true);

            var vm = new InscriptionViewModel(repo.Object, dialogue.Object);

            vm.Prenom = "test";
            vm.Nom = "test";
            vm.Email = "test@test.com";
            vm.MotDePasse = "123";
            vm.ConfirmationMotDePasse = "123";

            await vm.CreerCompteCommand.ExecuteAsync(null);

            dialogue.Verify(d => d.AfficherMessage("Cet email est déjà utilisé."), Times.Once);
            repo.Verify(r => r.AjouterAsync(It.IsAny<BookMyCut.Data.Models.Utilisateur>()), Times.Never);
        }
    }
}