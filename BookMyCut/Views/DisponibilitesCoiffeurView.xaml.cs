using System.Windows;
using BookMyCut.ViewModels;

namespace BookMyCut.Views
{
    public partial class DisponibilitesCoiffeurView : Window
    {
        // ViewModel injecté
        public DisponibilitesCoiffeurView(DisponibilitesCoiffeurViewModel vm)
        {
            InitializeComponent();
            DataContext = vm; // Branche le ViewModel à la vue
        }
    }
}
