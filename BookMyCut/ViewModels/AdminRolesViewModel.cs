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
    public partial class AdminRolesViewModel : ObservableObject
    {
        private readonly IUtilisateurRepository _repo;

        [ObservableProperty] private ObservableCollection<Utilisateur> _utilisateurs = new();
        [ObservableProperty] private Utilisateur? _utilisateurSelectionne;

        public AdminRolesViewModel(IUtilisateurRepository repo)
        {
            _repo = repo;
            _ = ChargerDonneesAsync();
        }

        private async Task ChargerDonneesAsync()
        {
            try
            {
                var liste = await _repo.ObtenirTousAsync();
                Utilisateurs = new ObservableCollection<Utilisateur>(liste);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task ModifierRole(RoleUtilisateur nouveauRole)
        {
            if (UtilisateurSelectionne == null) return;

            UtilisateurSelectionne.Role = nouveauRole;
            await _repo.ModifierAsync(UtilisateurSelectionne);

            MessageBox.Show($"{UtilisateurSelectionne.NomComplet} est maintenant {nouveauRole}.");
            await ChargerDonneesAsync();
        }

        [RelayCommand]
        private void OuvrirGestionServices()
        {
            var servicesView = App.ServiceProvider.GetRequiredService<GestionServicesView>();
            servicesView.Show();
        }

        [RelayCommand]
        private void Deconnexion()
        {

            SessionUtilisateur.Instance.Deconnecter();

            // Retour à login
            var loginView = App.ServiceProvider.GetRequiredService<ConnexionView>();
            loginView.Show();


            foreach (Window win in Application.Current.Windows)
            {
                if (win != loginView) win.Close();
            }
        }
    }
}