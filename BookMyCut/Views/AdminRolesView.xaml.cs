using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using BookMyCut.ViewModels;
using System.Windows;

namespace BookMyCut.Views
{
    public partial class AdminRolesView : Window
    {
        public AdminRolesView(AdminRolesViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}