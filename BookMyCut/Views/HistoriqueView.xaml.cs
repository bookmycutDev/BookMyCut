using BookMyCut.ViewModels;
using System.Windows.Controls;

namespace BookMyCut.Views
{
    public partial class HistoriqueView : UserControl
    {
        public HistoriqueView(HistoriqueViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}