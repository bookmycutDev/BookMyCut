using BookMyCut.Data.Models;
using Xunit;

namespace BookMyCut.Tests.Models
{
    public class UtilisateurTests
    {
        [Fact]
        public void Test_Utilisateur()
        {
            var utilisateur = new Utilisateur
            {
                NomComplet = "test test",
                Email = "test@test.com",
                MotDePasse = "123",
                Role = RoleUtilisateur.Coiffeur
            };

            Assert.Equal("test test", utilisateur.NomComplet);
            Assert.Equal("test@test.com", utilisateur.Email);
            Assert.Equal("123", utilisateur.MotDePasse);
            Assert.Equal(RoleUtilisateur.Coiffeur, utilisateur.Role);
        }

        [Fact]
        public void Role_Client()
        {
            var utilisateur = new Utilisateur
            {
                NomComplet = "Test User",
                Email = "test@test.com",
                MotDePasse = "pass"
            };

            Assert.Equal(RoleUtilisateur.Client, utilisateur.Role);
        }
    }
}