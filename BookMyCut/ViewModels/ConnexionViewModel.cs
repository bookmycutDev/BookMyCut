using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BookMyCut.Data;
using BookMyCut.Models;
using BookMyCut.Views;
using System.Linq;
using System.Windows;

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
                MessageBox.Show("Veuillez remplir tous les champs.",
                                "Champs manquants",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            var user = _db.Utilisateurs
                .FirstOrDefault(u => u.Email == Email && u.MotDePasse == MotDePasse);

            if (user != null)
            {
                
                MessageBox.Show($"Bienvenue, {user.NomComplet} !", "Connexion réussie");

                
                Window prochainePage = new MainWindow();
                prochainePage.Show();

               
                Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w is ConnexionView)
                    ?.Close();
            }
            else
            {
             
                MessageBox.Show("Email ou mot de passe incorrect.",
                                "Erreur d'authentification",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}