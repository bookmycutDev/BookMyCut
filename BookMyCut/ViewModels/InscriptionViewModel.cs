using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BookMyCut.Data;
using BookMyCut.Models;
using System.Windows;

namespace BookMyCut.ViewModels
{
    public partial class InscriptionViewModel : ObservableObject
    {
        private readonly BookMyCutContext _db = new BookMyCutContext();

        [ObservableProperty]
        private string _prenom = string.Empty;

        [ObservableProperty]
        private string _nom = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _motDePasse = string.Empty;

        [ObservableProperty]
        private string _confirmationMotDePasse = string.Empty;

        [RelayCommand]
        private void CreerCompte()
        {
            // Utilise _prenom, _nom, etc. (avec underscore) à l'intérieur du ViewModel
            if (string.IsNullOrWhiteSpace(_prenom) ||
                string.IsNullOrWhiteSpace(_nom) ||
                string.IsNullOrWhiteSpace(_email) ||
                string.IsNullOrWhiteSpace(_motDePasse) ||
                string.IsNullOrWhiteSpace(_confirmationMotDePasse))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Champs manquants",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_motDePasse != _confirmationMotDePasse)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas.", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_db.Utilisateurs.Any(u => u.Email == _email))
            {
                MessageBox.Show("Cet email est déjà associé à un compte.", "Email existant",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var nouvelUtilisateur = new Utilisateur
            {
                NomComplet = $"{_prenom} {_nom}",
                Email = _email,
                MotDePasse = _motDePasse,
                Role = RoleUtilisateur.Client
            };

            _db.Utilisateurs.Add(nouvelUtilisateur);
            _db.SaveChanges();

            MessageBox.Show($"Compte créé ! Bienvenue {nouvelUtilisateur.NomComplet}.",
                "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

            foreach (Window w in Application.Current.Windows)
            {
                if (w is BookMyCut.Views.InscriptionView)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}


    