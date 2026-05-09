using BookMyCut.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BookMyCut.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView(HomeViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void BtnPrendreRDV_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.NaviguerVersBooking();
        }
    }
}
