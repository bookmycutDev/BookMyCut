using BookMyCut.Utils;
using BookMyCut.ViewModels;
using BookMyCut.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace BookMyCut
{
    
    public partial class MainWindow : Window
    {
        // MainViewModel injecté  grâce configuration App.xaml.cs
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();

            //DataContext pour permettre les Bindings dans le XAML
            this.DataContext = vm;
        }

       
        private void BtnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialise la session globale
            SessionUtilisateur.Instance.Deconnecter();

            // Récupère une nouvelle instance de la vue de connexion via le ServiceProvider
            var login = App.ServiceProvider.GetRequiredService<ConnexionView>();

            // Affiche la fenêtre de connexion et ferme la fenêtre principale
            login.Show();
            this.Close();
        }
    }
}