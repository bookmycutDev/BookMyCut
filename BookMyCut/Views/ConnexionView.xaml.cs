using System.Windows;
using System.Windows.Controls;
using BookMyCut.ViewModels;

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

        //méthode chaque fois tape caractère
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //vérifie fenêtre utilise ConnexionViewModel 
            if (this.DataContext is ConnexionViewModel vm)
            {
                //récupère texte tapé 
                //envoie manuellement "MotDePasse" de ViewModel
                vm.MotDePasse = ((PasswordBox)sender).Password;
            }
        }
    }
}