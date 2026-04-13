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
    public partial class InscriptionViewModel : ObservableObject
    {
        private readonly IUtilisateurRepository _repo; // On garde _repo ici aussi

        [ObservableProperty] private string _prenom = string.Empty;
        [ObservableProperty] private string _nom = string.Empty;
        [ObservableProperty] private string _email = string.Empty;
        [ObservableProperty] private string _motDePasse = string.Empty;
        [ObservableProperty] private string _confirmationMotDePasse = string.Empty;

        public InscriptionViewModel(IUtilisateurRepository repo)
        {
            _repo = repo;
        }

        [RelayCommand]
        private async Task CreerCompte()
        {
            if (string.IsNullOrWhiteSpace(_email) || _motDePasse != _confirmationMotDePasse)
            {
                MessageBox.Show("Données invalides.");
                return;
            }

            // Utilise _repo pour vérifier l'email
            bool emailExiste = await _repo.EmailExisteAsync(_email);

            if (emailExiste)
            {
                MessageBox.Show("Cet email est déjà utilisé.");
                return;
            }

            var nouvelUtilisateur = new Utilisateur
            {
                NomComplet = $"{_prenom} {_nom}",
                Email = _email,
                MotDePasse = Hash.HashPassword(_motDePasse),
                Role = RoleUtilisateur.Client
            };

            await _repo.AjouterAsync(nouvelUtilisateur);
            MessageBox.Show("Compte créé !");

            // Utilise l'injection pour ouvrir la connexion
            var loginWindow = App.ServiceProvider.GetRequiredService<ConnexionView>();
            loginWindow.Show();

            Application.Current.Windows.OfType<InscriptionView>().FirstOrDefault()?.Close();
        }
        [RelayCommand]
        private void AllerAConnexion()
        {
            var connexionView = App.ServiceProvider.GetRequiredService<ConnexionView>();
            connexionView.Show();

            Application.Current.Windows.OfType<InscriptionView>().FirstOrDefault()?.Close();
        }
    }
}