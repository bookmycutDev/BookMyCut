using System.Windows;
using Microsoft.EntityFrameworkCore;
using BookMyCut.Data.Data;
using BookMyCut.Utils;
using BookMyCut.Data.Models;

namespace BookMyCut
{
    public partial class ModifierProfil : Window
    {
        private readonly BookMyCutContext _db;
        private Utilisateur? _utilisateurConnecte;

        public ModifierProfil(BookMyCutContext db)
        {
            InitializeComponent();
            _db = db;
            ChargerInformationsUtilisateur();
        }

        private void ChargerInformationsUtilisateur()
        {
            if (!SessionUtilisateur.Instance.EstConnecte || SessionUtilisateur.Instance.UtilisateurConnecte == null)
            {
                MessageBox.Show("Aucun utilisateur connecté.");
                Close();
                return;
            }

            _utilisateurConnecte = SessionUtilisateur.Instance.UtilisateurConnecte;

            txtNomComplet.Text = _utilisateurConnecte.NomComplet;
            txtEmail.Text = _utilisateurConnecte.Email;
        }

        private async void BtnEnregistrerClick(object sender, RoutedEventArgs e)
        {
            if (_utilisateurConnecte == null)
            {
                MessageBox.Show("Utilisateur introuvable.");
                return;
            }

            string nomComplet = txtNomComplet.Text.Trim();
            string email = txtEmail.Text.Trim();
            string nouveauMotDePasse = txtMotDePasse.Password;
            string confirmationMotDePasse = txtConfirmerMotDePasse.Password;

            if (string.IsNullOrWhiteSpace(nomComplet) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Le nom complet et l'email sont obligatoires.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(nouveauMotDePasse))
            {
                if (nouveauMotDePasse.Length < 6)
                {
                    MessageBox.Show("Le mot de passe doit contenir au moins 6 caractères.");
                    return;
                }

                if (nouveauMotDePasse != confirmationMotDePasse)
                {
                    MessageBox.Show("La confirmation du mot de passe ne correspond pas.");
                    return;
                }
            }

            bool emailDejaUtilise = await _db.Utilisateurs
                .AnyAsync(u => u.Email == email && u.Id != _utilisateurConnecte.Id);

            if (emailDejaUtilise)
            {
                MessageBox.Show("Cet email est déjà utilisé par un autre compte.");
                return;
            }

            var utilisateurEnBase = await _db.Utilisateurs
                .FirstOrDefaultAsync(u => u.Id == _utilisateurConnecte.Id);

            if (utilisateurEnBase == null)
            {
                MessageBox.Show("Impossible de retrouver l'utilisateur dans la base de données.");
                return;
            }

            utilisateurEnBase.NomComplet = nomComplet;
            utilisateurEnBase.Email = email;

            if (!string.IsNullOrWhiteSpace(nouveauMotDePasse))
            {
                utilisateurEnBase.MotDePasse = Hash.HashPassword(nouveauMotDePasse);
            }

            await _db.SaveChangesAsync();

            SessionUtilisateur.Instance.Connecter(utilisateurEnBase);

            MessageBox.Show("Profil modifié avec succès.");
            DialogResult = true;
            Close();
        }
    }
}