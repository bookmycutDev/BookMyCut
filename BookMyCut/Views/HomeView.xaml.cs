using System.Windows.Controls;
using BookMyCut.ViewModels;

namespace BookMyCut.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView(HomeViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

    }
}