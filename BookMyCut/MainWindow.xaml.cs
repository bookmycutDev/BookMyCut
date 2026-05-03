using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using BookMyCut.Utils;
using BookMyCut.Views;
using BookMyCut.ViewModels;
using BookMyCut.Data.Models;

namespace BookMyCut
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MettreAJourNomUtilisateur();
            AfficherVueInitialeSelonRole();
        }

        public void MettreAJourNomUtilisateur()
        {
            var utilisateur = SessionUtilisateur.Instance.UtilisateurConnecte;

            txtUser.Text = utilisateur != null
                ? $"Bonjour, {utilisateur.NomComplet} !"
                : "Bonjour !";
        }

        private void AfficherVueInitialeSelonRole()
        {
            var utilisateur = SessionUtilisateur.Instance.UtilisateurConnecte;

            if (utilisateur != null && utilisateur.Role == RoleUtilisateur.Coiffeur)
            {
                AfficherDisponibilitesCoiffeur();
            }
            else
            {
                AfficherAccueil();
            }
        }

        private void AfficherAccueil()
        {
            var homeView = App.ServiceProvider.GetRequiredService<HomeView>();
            var homeVm = App.ServiceProvider.GetRequiredService<HomeViewModel>();

            homeView.DataContext = homeVm;
            MainContent.Content = homeView;
        }

        public void AfficherAccueilPublic()
        {
            var utilisateur = SessionUtilisateur.Instance.UtilisateurConnecte;

            if (utilisateur != null && utilisateur.Role == RoleUtilisateur.Coiffeur)
            {
                AfficherDisponibilitesCoiffeur();
            }
            else
            {
                AfficherAccueil();
            }
        }

        private void AfficherServices()
        {
            var bookingView = App.ServiceProvider.GetRequiredService<BookingView>();
            var bookingVm = App.ServiceProvider.GetRequiredService<BookingViewModel>();

            bookingVm.SurReservationReussie = () =>
            {
                AfficherAccueilPublic();
            };

            bookingView.DataContext = bookingVm;
            MainContent.Content = bookingView;
        }

        private void AfficherModifierProfil()
        {
            var profilView = App.ServiceProvider.GetRequiredService<ModifierProfilView>();
            MainContent.Content = profilView;
        }

        private void AfficherNosCoiffeurs()
        {
            var view = App.ServiceProvider.GetRequiredService<NosCoiffeursView>();
            var vm = App.ServiceProvider.GetRequiredService<NosCoiffeursViewModel>();

            view.DataContext = vm;
            MainContent.Content = view;
        }

        private void AfficherDisponibilitesCoiffeur()
        {
            var view = App.ServiceProvider.GetRequiredService<DisponibilitesCoiffeurView>();
            var vm = App.ServiceProvider.GetRequiredService<DisponibilitesCoiffeurViewModel>();

            view.DataContext = vm;
            MainContent.Content = view;
        }

        public void NaviguerVersBooking()
        {
            AfficherServices();
        }

        private void BtnAccueil_Click(object sender, RoutedEventArgs e)
        {
            AfficherAccueilPublic();
        }

        private void BtnServices_Click(object sender, RoutedEventArgs e)
        {
            AfficherServices();
        }

        private void BtnModifierProfil_Click(object sender, RoutedEventArgs e)
        {
            AfficherModifierProfil();
        }

        private void BtnNosCoiffeurs_Click(object sender, RoutedEventArgs e)
        {
            AfficherNosCoiffeurs();
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