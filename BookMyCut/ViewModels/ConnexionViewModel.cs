using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using BookMyCut.Views;
using System.Linq;
using System.Windows;
using BookMyCut.Utils;
using System.Collections.Generic;

namespace BookMyCut.ViewModels
{
    public partial class ConnexionViewModel : ObservableObject
    {
        private readonly BookMyCutContext _db = new BookMyCutContext();

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _motDePasse = string.Empty;

        [RelayCommand]
        private void SeConnecter()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(MotDePasse))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string mdpSaisiHache = Hash.HashPassword(MotDePasse);

            var user = _db.Utilisateurs
                .FirstOrDefault(u => u.Email == Email && u.MotDePasse == mdpSaisiHache);

            if (user != null)
            {
                MessageBox.Show($"Bienvenue, {user.NomComplet} !", "Connexion réussie");

                // 1. Déterminer la page de destination
                Window prochainePage;
                if (user.Role == RoleUtilisateur.Admin)
                {
                    prochainePage = new AdminRolesView();
                }
                else
                {
                    prochainePage = new MainWindow();
                }

                // 2. Afficher la nouvelle page
                prochainePage.Show();

                // 3. Fermer proprement TOUTES les autres fenêtres (Accueil + Login)
                // On récupère la liste des fenêtres actuelles AVANT de boucler
                var fenetresAFermer = Application.Current.Windows.Cast<Window>()
                    .Where(w => w != prochainePage).ToList();

                foreach (var window in fenetresAFermer)
                {
                    window.Close();
                }
            }
            else
            {
                MessageBox.Show("Email ou mot de passe incorrect.", "Erreur d'authentification", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}