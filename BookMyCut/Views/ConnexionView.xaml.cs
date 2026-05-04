using System;
using System.Linq;
using System.Windows;
using BookMyCut.Data;
using BookMyCut.Models;

namespace BookMyCut.Views
{
    public partial class ConnexionView : Window
    {
        // ViewModel injecté 
        public ConnexionView(ConnexionViewModel vm)
        {
            InitializeComponent();
            DataContext = vm; // Branche  ViewModel a vue
        }

        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string motDePasse = txtPassword.Password.Trim();

            txtMessage.Visibility = Visibility.Collapsed;
            txtMessage.Text = "";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(motDePasse))
            {
                AfficherErreur("Tous les champs sont obligatoires.");
                return;
            }

            if (!EmailValide(email))
            {
                AfficherErreur("Veuillez entrer un email valide.");
                return;
            }

            Utilisateur? user = _db.Utilisateurs
                .FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                AfficherErreur("Utilisateur inexistant.");
                return;
            }

            if (user.MotDePasse != motDePasse)
            {
                AfficherErreur("Mot de passe incorrect.");
                return;
            }

            MessageBox.Show(
                $"Bienvenue {user.NomComplet} ({user.Role})",
                "Connexion réussie",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }

        private void AfficherErreur(string message)
        {
            txtMessage.Text = message;
            txtMessage.Visibility = Visibility.Visible;
        }

        private bool EmailValide(string email)
        {
            return !string.IsNullOrWhiteSpace(email)
                && email.Contains("@")
                && email.Contains(".")
                && !email.Contains(" ")
                && email.IndexOf("@") > 0
                && email.LastIndexOf(".") > email.IndexOf("@") + 1
                && email.LastIndexOf(".") < email.Length - 1;
        }

        private void Inscription_Click(object sender, RoutedEventArgs e)
        {
            Inscription fenetre = new Inscription();
            fenetre.Show();
            Close();
        }

        private void MotDePasseOublie_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonction à ajouter plus tard.");
        }
    }
}