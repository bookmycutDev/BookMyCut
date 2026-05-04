using System.Windows;

namespace BookMyCut.Views
{
    public partial class ContactView : Window
    {
        public ContactView()
        {
            InitializeComponent();
        }

        private void BtnFermerClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}