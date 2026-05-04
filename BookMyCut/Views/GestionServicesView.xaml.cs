using BookMyCut.ViewModels;
using System.Windows;

namespace BookMyCut.Views
{
    public partial class GestionServicesView : Window 
    {
        public GestionServicesView(GestionServicesViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm; //active les boutons 
        }
    }
}