using System.Windows;
using System.Windows.Controls;

namespace BookMyCut.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void BtnPrendreRDV_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
     
                mainWindow.NaviguerVersBooking();
            }
        }
    }
}