using BookMyCut.ViewModels;
using System.Windows;

namespace BookMyCut.Views
{
   
    public partial class BookingView : Window
    {
        public BookingView(BookingViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        // Permet fermer fenêtre manuellement si on clique sur Annuler
        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}