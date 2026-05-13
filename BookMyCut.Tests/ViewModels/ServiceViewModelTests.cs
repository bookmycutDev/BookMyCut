using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.ViewModels;
using Moq;
using Xunit;

namespace BookMyCut.Tests.ViewModels
{
    public class ServiceViewModelTests
    {
        [Fact]
        public async Task ChargerServices()
        {
            var serviceRepository = new Mock<IServiceRepository>();

            serviceRepository.Setup(repository => repository.ObtenirTousAsync())
                .ReturnsAsync(new List<Service>
                {
                    new Service
                    {
                        Id = 1,
                        Nom = "Coupe",
                        DureeMinutes = 30,
                        Prix = 25m
                    }
                });

            var serviceViewModel = new ServiceViewModel(serviceRepository.Object);

            await Task.Delay(100);

            Assert.Single(serviceViewModel.Services);
            Assert.Equal("Coupe", serviceViewModel.Services.First().Nom);
        }
    }
}
