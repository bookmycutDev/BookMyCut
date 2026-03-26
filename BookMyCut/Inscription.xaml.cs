using System.Linq;
using System.Windows;
using BookMyCut.Data;
using BookMyCut.Models;

namespace BookMyCut
{
    public partial class Inscription : Window
    {
        private readonly BookMyCutContext _db = new BookMyCutContext();

        public Inscription()
        {
            InitializeComponent();
        }

        private void BtnCreerClick(object sender, RoutedEventArgs e)
        {
            string prenom = txtPrenom.Text.Trim();
            string nom = txtNom.Text.Trim();
            string email = txtEmail.Text.Trim();
            string motDePasse = txtMdp.Password.Trim();
            string confirmation = txtValidMdp.Password.Trim();

            if (prenom == "" || nom == "" || email == "" || motDePasse == "" || confirmation == "")
            {
                MessageBox.Show("Remplis tous les champs");
                return;
            }

            if (motDePasse != confirmation)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas");
                return;
            }

            bool emailExiste = _db.Utilisateurs.Any(u => u.Email.ToLower() == email.ToLower());

            if (emailExiste)
            {
                MessageBox.Show("Cet email existe déjà");
                return;
            }

            Utilisateur nouvelUtilisateur = new Utilisateur
            {
                NomComplet = prenom + " " + nom,
                Email = email,
                MotDePasse = motDePasse,
                Role = RoleUtilisateur.Client
            };

            _db.Utilisateurs.Add(nouvelUtilisateur);
            _db.SaveChanges();

            MessageBox.Show("Compte créé");
            Close();
        }

        private void BtnFermerClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}