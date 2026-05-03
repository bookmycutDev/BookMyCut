using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using BookMyCut.Views;
using BookMyCut.Utils;

namespace BookMyCut.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty] private object? _currentContent;
        [ObservableProperty] private string _messageBienvenue;

        public MainViewModel()
        {
            // Initialisation du nom de l'utilisateur
            MessageBienvenue = SessionUtilisateur.Instance.EstConnecte
                ? $"Bonjour, {SessionUtilisateur.Instance.UtilisateurConnecte.NomComplet} !"
                : "Bonjour, Utilisateur !";

            // Charger l'accueil par défaut
            AfficherAccueil();
        }

        [RelayCommand]
        public void AfficherAccueil()
        {
            var homeView = App.ServiceProvider.GetRequiredService<HomeView>();
            // On s'assure que les données sont fraîches
            if (homeView.DataContext is HomeViewModel vm)
            {
                _ = vm.ChargerRendezVousAsync();
            }
            CurrentContent = homeView;
        }

        [RelayCommand]
        public void AfficherServices()
        {
            // On affiche enfin la vraie vue des services au lieu du MessageBox !
            CurrentContent = App.ServiceProvider.GetRequiredService<ServiceView>();
        }
    }
}