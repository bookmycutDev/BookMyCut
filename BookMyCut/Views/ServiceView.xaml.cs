using BookMyCut.ViewModels;
using System.Windows.Controls;

namespace BookMyCut.Views
{
    public partial class ServiceView : UserControl
    {
        public ServiceView(ServiceViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}