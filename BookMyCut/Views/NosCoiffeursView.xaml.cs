using System.Windows;
using BookMyCut.ViewModels;

namespace BookMyCut.Views
{
    public partial class NosCoiffeursView : Window
    {
        // ViewModel injecté
        public NosCoiffeursView(NosCoiffeursViewModel vm)
        {
            InitializeComponent();
            DataContext = vm; // Branche le ViewModel à la vue
        }
    }
}
