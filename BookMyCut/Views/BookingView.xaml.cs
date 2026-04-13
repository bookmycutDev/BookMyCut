using BookMyCut.ViewModels;
using System.Windows.Controls;

namespace BookMyCut.Views
{
    public partial class BookingView : UserControl
    {
        public BookingView(BookingViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}