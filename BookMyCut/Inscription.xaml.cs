using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookMyCut
{
    /// <summary>
    /// Logique d'interaction pour Inscription.xaml
    /// </summary>
    public partial class Inscription : Window
    {
        public Inscription()
        {
            InitializeComponent();
        }
        private void BtnCreerClick(object sender, RoutedEventArgs e)
        {
            if (txtPrenom.Text == "" || txtNom.Text == "" || txtEmail.Text == "" || txtMdp.Password == "" || txtValidMdp.Password == "" )
            {
                MessageBox.Show("Remplis tous les champs");
            }
            else
            {
                MessageBox.Show("Compte créé");
                Close();
            }
        }
        private void BtnFermerClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
