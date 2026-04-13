using System.Text;
using System.Windows;

namespace BookMyCut
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BtnInscriptionClick(object sender, RoutedEventArgs e)
        {
            Inscription fenetre = new Inscription();
            fenetre.Show();
        }

        private void BtnServices_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Découvrez nos services bientôt ici ! Utilisez 'PRENDRE RENDEZ-VOUS' sur l'accueil.");

        private void BtnCoiffeurs_Click(object sender, RoutedEventArgs e) => MessageBox.Show("À venir...");
        private void BtnContact_Click(object sender, RoutedEventArgs e) => MessageBox.Show("À venir...");




    }
}