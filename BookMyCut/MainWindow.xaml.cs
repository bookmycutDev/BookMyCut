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
    }
}