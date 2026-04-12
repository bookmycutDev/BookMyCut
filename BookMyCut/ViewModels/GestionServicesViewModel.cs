using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookMyCut.ViewModels
{
    public partial class GestionServicesViewModel : ObservableObject
    {
        private readonly IServiceRepository _serviceRepo;

        // Liste affichée dans le DataGrid
        [ObservableProperty]
        private ObservableCollection<Service> _services = new();

        // Champs pour l'ajout d'un nouveau service
        [ObservableProperty] private string _nom = string.Empty;
        [ObservableProperty] private string _description = string.Empty;
        [ObservableProperty] private string _prix = string.Empty;
        [ObservableProperty] private string _Duree = string.Empty;

        // Service sélectionné dans la liste
        [ObservableProperty]
        private Service? _serviceSelectionne;

        public GestionServicesViewModel(IServiceRepository serviceRepo)
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
        private async Task AjouterService()
        {
            // Validation simple
            if (string.IsNullOrWhiteSpace(Nom) || !decimal.TryParse(Prix, out decimal prixDecimal) || !int.TryParse(Duree, out int dureeInt))
            {
                MessageBox.Show("Veuillez entrer des informations valides (Prix et Durée doivent être des nombres).");
                return;
            }

            var nouveauService = new Service
            {
                Nom = Nom,
                Description = Description,
                Prix = prixDecimal,
                DureeMinutes = dureeInt
            };

            await _serviceRepo.AjouterAsync(nouveauService);

            // Réinitialiser les champs
            Nom = Description = Prix = Duree = string.Empty;

            await ChargerServicesAsync();
            MessageBox.Show("Service ajouté !");
        }

        [RelayCommand]
        private async Task SupprimerService()
        {
            if (ServiceSelectionne == null) return;

            var resultat = MessageBox.Show($"Supprimer le service {ServiceSelectionne.Nom} ?", "Confirmation", MessageBoxButton.YesNo);
            if (resultat == MessageBoxResult.Yes)
            {
                await _serviceRepo.SupprimerAsync(ServiceSelectionne.Id);
                await ChargerServicesAsync();
            }
        }
    }
}