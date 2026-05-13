using BookMyCut.Data.Repositories;
using BookMyCut.ViewModels;
using Moq;
using Xunit;

namespace BookMyCut.Tests.ViewModels
{
    public class ConnexionViewModelTests
    {
        [Fact]
        public void ChampsVides()
        {
            var repo = new Mock<IUtilisateurRepository>();

            var connexionViewModel = new ConnexionViewModel(repo.Object);

            Assert.Equal(string.Empty, connexionViewModel.Email);
            Assert.Equal(string.Empty, connexionViewModel.MotDePasse);
        }
    }
}