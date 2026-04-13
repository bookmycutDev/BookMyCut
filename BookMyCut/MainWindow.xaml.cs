using BookMyCut.Utils;
using BookMyCut.ViewModels;
using BookMyCut.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace BookMyCut
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RafraichirInterface();
            // Charger la vue d'accueil par défaut
            ChargerAccueil();
        }

        private void RafraichirInterface()
        {
            if (SessionUtilisateur.Instance.EstConnecte)
            {
                txtUser.Text = $"Bonjour, {SessionUtilisateur.Instance.UtilisateurConnecte.NomComplet} !";
            }
        }

        private void ChargerAccueil()
        {
            // vue au ServiceProvider pour injecté ViewModel automatiquement
            var homeView = App.ServiceProvider.GetRequiredService<HomeView>();

            //force rafraîchissement 
            if (homeView.DataContext is HomeViewModel vm)
            {
                // appelle méthode chargement 
                _ = vm.ChargerRendezVousAsync();
            }

            MainContent.Content = homeView;
        }

        private void BtnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            SessionUtilisateur.Instance.Deconnecter();
            var login = App.ServiceProvider.GetRequiredService<ConnexionView>();
            login.Show();
            this.Close();
        }

        private void BtnAccueil_Click(object sender, RoutedEventArgs e) => ChargerAccueil();

        public void NaviguerVersBooking()
        {
            var bookingView = App.ServiceProvider.GetRequiredService<BookingView>();

            if (bookingView.DataContext is BookingViewModel vm)
            {
                // abonne action dans le ViewModel
                vm.SurReservationReussie = () =>
                {
                    // Quand action invoquée, on revient à l'accueil
                    ChargerAccueil();
                };
            }

            MainContent.Content = bookingView;
        }

        private void BtnServices_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Découvrez nos services bientôt ici ! Utilisez 'PRENDRE RENDEZ-VOUS' sur l'accueil.");

        private void BtnCoiffeurs_Click(object sender, RoutedEventArgs e) => MessageBox.Show("À venir...");
        private void BtnContact_Click(object sender, RoutedEventArgs e) => MessageBox.Show("À venir...");




    }
}