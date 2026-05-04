using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using BookMyCut.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BookMyCut.ViewModels
{
    public partial class ServiceViewModel : ObservableObject
    {
        private readonly IServiceRepository _serviceRepo;

        [ObservableProperty] private ObservableCollection<Service> _services = new();

        public ServiceViewModel(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
            _ = ChargerServicesAsync();
        }

        private async Task ChargerServicesAsync()
        {
            var liste = await _serviceRepo.ObtenirTousAsync();
            Services = new ObservableCollection<Service>(liste);
        }

        [RelayCommand]
        private void ReserverService(Service serviceSelectionne)
        {
            // récupère fenêtre réservation
            var bookingView = App.ServiceProvider.GetRequiredService<BookingView>();

            if (bookingView.DataContext is BookingViewModel bookingVm)
            {
                // service cliqué
                bookingVm.ServiceSelectionne = serviceSelectionne;

                // fermeture 
                bookingVm.SurReservationReussie = () =>
                {
                    bookingView.Close();
                    // Optionnel : Message de succès ou redirection
                };
            }

            bookingView.ShowDialog();
        }
    }
}