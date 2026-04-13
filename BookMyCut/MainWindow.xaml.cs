using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using BookMyCut.Utils;
using BookMyCut.Views;
using BookMyCut.ViewModels;

namespace BookMyCut
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChargerUtilisateurConnecte();
            AfficherAccueil();
        }

        private void ChargerUtilisateurConnecte()
        {
            var utilisateur = SessionUtilisateur.Instance.UtilisateurConnecte;

            txtUser.Text = utilisateur != null
                ? $"Bonjour, {utilisateur.NomComplet} !"
                : "Bonjour !";
        }

        private void AfficherAccueil()
        {
            var homeView = App.ServiceProvider.GetRequiredService<HomeView>();
            var homeVm = App.ServiceProvider.GetRequiredService<HomeViewModel>();

            homeView.DataContext = homeVm;
            MainContent.Content = homeView;
        }

        private void AfficherServices()
        {
            var bookingView = App.ServiceProvider.GetRequiredService<BookingView>();
            var bookingVm = App.ServiceProvider.GetRequiredService<BookingViewModel>();

            bookingVm.SurReservationReussie = () =>
            {
                AfficherAccueil();
            };

            bookingView.DataContext = bookingVm;
            MainContent.Content = bookingView;
        }

        public void NaviguerVersBooking()
        {
            AfficherServices();
        }

        private void BtnAccueil_Click(object sender, RoutedEventArgs e)
        {
            AfficherAccueil();
        }

        private void BtnServices_Click(object sender, RoutedEventArgs e)
        {
            AfficherServices();
        }

        private void BtnModifierProfil_Click(object sender, RoutedEventArgs e)
        {
            var fenetreModifierProfil = App.ServiceProvider.GetRequiredService<ModifierProfil>();
            fenetreModifierProfil.Owner = this;

            bool? resultat = fenetreModifierProfil.ShowDialog();

            if (resultat == true)
            {
                ChargerUtilisateurConnecte();
                AfficherAccueil();
            }
        }

        private void BtnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            SessionUtilisateur.Instance.Deconnecter();

            var connexionView = App.ServiceProvider.GetRequiredService<ConnexionView>();
            connexionView.Show();

            Close();
        }
    }
}