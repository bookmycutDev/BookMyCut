using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using BookMyCut.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookMyCut.ViewModels
{
    public partial class GestionServicesViewModel : ObservableObject
    {
        private readonly IServiceRepository _serviceRepo;

        [ObservableProperty] private ObservableCollection<Service> _services = new();
        [ObservableProperty] private string _nom = string.Empty;
        [ObservableProperty] private string _description = string.Empty;
        [ObservableProperty] private string _prix = string.Empty;
        [ObservableProperty] private string _duree = string.Empty;
        [ObservableProperty] private Service? _serviceSelectionne;

        // Ajout Jonathan2.0
        [ObservableProperty] private bool _estEnModeEdition;

        public bool PeutAjouter => !EstEnModeEdition;
        public bool PeutModifierOuAnnuler => EstEnModeEdition;

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

        partial void OnServiceSelectionneChanged(Service? value)
        {
            if (value == null)
            {
                EstEnModeEdition = false;
                return;
            }

            Nom = value.Nom;
            Description = value.Description ?? string.Empty;
            Prix = value.Prix.ToString("0.##");
            Duree = value.DureeMinutes.ToString();
            EstEnModeEdition = true;
        }

        partial void OnEstEnModeEditionChanged(bool value)
        {
            OnPropertyChanged(nameof(PeutAjouter));
            OnPropertyChanged(nameof(PeutModifierOuAnnuler));
        }

        [RelayCommand]
        private async Task AjouterService()
        {
            if (!PeutAjouter)
            {
                MessageBox.Show("Vous êtes en mode édition. Cliquez sur Modifier ou Annuler.");
                return;
            }

            if (!ValiderChamps(out decimal prixDecimal, out int dureeInt))
                return;

            var nouveauService = new Service
            {
                Nom = Nom,
                Description = Description,
                Prix = prixDecimal,
                DureeMinutes = dureeInt
            };

            await _serviceRepo.AjouterAsync(nouveauService);
            ViderChamps();
            await ChargerServicesAsync();
            MessageBox.Show("Service ajouté !");
        }

        [RelayCommand]
        private async Task ModifierService()
        {
            if (ServiceSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un service à modifier.");
                return;
            }

            if (!ValiderChamps(out decimal prixDecimal, out int dureeInt))
                return;

            ServiceSelectionne.Nom = Nom;
            ServiceSelectionne.Description = Description;
            ServiceSelectionne.Prix = prixDecimal;
            ServiceSelectionne.DureeMinutes = dureeInt;

            await _serviceRepo.ModifierAsync(ServiceSelectionne);

            ViderChamps();
            await ChargerServicesAsync();
            MessageBox.Show("Service modifié !");
        }

        [RelayCommand]
        private async Task SupprimerService()
        {
            if (ServiceSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un service à supprimer.");
                return;
            }

            var resultat = MessageBox.Show(
                $"Supprimer le service {ServiceSelectionne.Nom} ?",
                "Confirmation",
                MessageBoxButton.YesNo);

            if (resultat == MessageBoxResult.Yes)
            {
                await _serviceRepo.SupprimerAsync(ServiceSelectionne.Id);
                ViderChamps();
                await ChargerServicesAsync();
            }
        }

        [RelayCommand]
        private void AnnulerEdition()
        {
            ViderChamps();
        }

        private bool ValiderChamps(out decimal prixDecimal, out int dureeInt)
        {
            prixDecimal = 0;
            dureeInt = 0;

            if (string.IsNullOrWhiteSpace(Nom))
            {
                MessageBox.Show("Le nom du service est obligatoire.");
                return false;
            }

            if (!decimal.TryParse(Prix, out prixDecimal))
            {
                MessageBox.Show("Veuillez entrer des informations valides (Prix doit être un nombre).");
                return false;
            }

            if (!int.TryParse(Duree, out dureeInt))
            {
                MessageBox.Show("Veuillez entrer des informations valides (Durée doit être un nombre entier).");
                return false;
            }

            return true;
        }

        private void ViderChamps()
        {
            Nom = string.Empty;
            Description = string.Empty;
            Prix = string.Empty;
            Duree = string.Empty;
            ServiceSelectionne = null;
            EstEnModeEdition = false;
        }

        [RelayCommand]
        private void Deconnexion()
        {
            SessionUtilisateur.Instance.Deconnecter();
            var loginView = App.ServiceProvider.GetRequiredService<ConnexionView>();
            loginView.Show();

            foreach (Window win in Application.Current.Windows)
            {
                if (win != loginView) win.Close();
            }
        }
    }
}
