using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using BookMyCut.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace BookMyCut.ViewModels
{
    public partial class ConnexionViewModel : ObservableObject
    {
        private readonly IUtilisateurRepository _repo; 

        [ObservableProperty] private string _email = string.Empty;
        [ObservableProperty] private string _motDePasse = string.Empty;

        public ConnexionViewModel(IUtilisateurRepository repo)
        {
            _repo = repo;
        }

        [RelayCommand]
        private async Task SeConnecter()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(MotDePasse))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            string mdpHache = Hash.HashPassword(MotDePasse);

            // Utilise _repo (le repository injecté)
            var user = await _repo.ObtenirParEmailEtMotDePasseAsync(Email, mdpHache);

            if (user != null)
            {
                SessionUtilisateur.Instance.Connecter(user);
                MessageBox.Show($"Bienvenue, {user.NomComplet} !");

                Window prochainePage;
                if (user.Role == RoleUtilisateur.Admin)
                    prochainePage = App.ServiceProvider.GetRequiredService<AdminRolesView>();
                else
                    prochainePage = App.ServiceProvider.GetRequiredService<MainWindow>();

                prochainePage.Show();
                Application.Current.Windows.OfType<ConnexionView>().FirstOrDefault()?.Close();
            }
            else
            {
                MessageBox.Show("Email ou mot de passe incorrect.");
            }
        }

        [RelayCommand]
        private void AllerAInscription() // Maintenant elle est bien à l'extérieur
        {
            var inscriptionView = App.ServiceProvider.GetRequiredService<InscriptionView>();
            inscriptionView.Show();

            Application.Current.Windows.OfType<ConnexionView>().FirstOrDefault()?.Close();
        }

    }
}