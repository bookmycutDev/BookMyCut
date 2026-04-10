using System.Windows;
using System.Windows.Controls;
using BookMyCut.ViewModels;

namespace BookMyCut.Views
{
    public partial class InscriptionView : Window
    {
        public InscriptionView(InscriptionViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is InscriptionViewModel vm)
                vm.MotDePasse = ((PasswordBox)sender).Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is InscriptionViewModel vm)
                vm.ConfirmationMotDePasse = ((PasswordBox)sender).Password;
        }
    }
}