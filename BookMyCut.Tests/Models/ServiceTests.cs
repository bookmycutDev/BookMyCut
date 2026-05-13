using BookMyCut.Data.Models;
using Xunit;

namespace BookMyCut.Tests.Models
{
    public class ServiceTests
    {
        [Fact]
        public void Test_Service()
        {
            var service = new Service
            {
                Id = 1,
                Nom = "Coupe Classique",
                Description = "Coupe simple",
                DureeMinutes = 30,
                Prix = 25.00m
            };

            Assert.Equal(1, service.Id);
            Assert.Equal("Coupe Classique", service.Nom);
            Assert.Equal("Coupe simple", service.Description);
            Assert.Equal(30, service.DureeMinutes);
            Assert.Equal(25.00m, service.Prix);
        }
    }
}