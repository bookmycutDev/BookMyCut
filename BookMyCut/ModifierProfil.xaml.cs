using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using BookMyCut.Data;
using BookMyCut.Models;

namespace BookMyCut
{
    public partial class ModifierProfil : Window
    {
        private Utilisateur utilisateurActuel;

        public ModifierProfil(Utilisateur utilisateur)
        {
            InitializeComponent();

            utilisateurActuel = utilisateur;

            txtNomComplet.Text = utilisateurActuel.NomComplet;
            txtEmail.Text = utilisateurActuel.Email;
        }

        private void BtnEnregistrerClick(object sender, RoutedEventArgs e)
        {
            string nom = txtNomComplet.Text.Trim();
            string email = txtEmail.Text.Trim();
            string motDePasse = txtMotDePasse.Password.Trim();
            string confirmerMotDePasse = txtConfirmerMotDePasse.Password.Trim();

            if (nom == "" || email == "")
            {
                MessageBox.Show("Tous les champs obligatoires doivent être remplis.");
                return;
            }

            var validationEmail = new EmailAddressAttribute();
            if (!validationEmail.IsValid(email))
            {
                MessageBox.Show("Format d'email invalide.");
                return;
            }

            if (motDePasse != "" || confirmerMotDePasse != "")
            {
                if (motDePasse == "" || confirmerMotDePasse == "")
                {
                    MessageBox.Show("Veuillez remplir les deux champs du mot de passe.");
                    return;
                }

                if (motDePasse.Length < 6)
                {
                    MessageBox.Show("Le mot de passe doit contenir au moins 6 caractères.");
                    return;
                }

                if (motDePasse != confirmerMotDePasse)
                {
                    MessageBox.Show("Les mots de passe ne correspondent pas.");
                    return;
                }
            }

            using (var context = new BookMyCutContext())
            {
                bool emailDejaUtilise = context.Utilisateurs
                    .Any(u => u.Email == email && u.Id != utilisateurActuel.Id);

                if (emailDejaUtilise)
                {
                    MessageBox.Show("Cet email est déjà utilisé par un autre compte.");
                    return;
                }

                var utilisateurBD = context.Utilisateurs
                    .FirstOrDefault(u => u.Id == utilisateurActuel.Id);

                if (utilisateurBD == null)
                {
                    MessageBox.Show("Utilisateur introuvable.");
                    return;
                }

                utilisateurBD.NomComplet = nom;
                utilisateurBD.Email = email;

                if (motDePasse != "")
                {
                    utilisateurBD.MotDePasse = motDePasse;
                }

                context.SaveChanges();

                MessageBox.Show("Profil mis à jour avec succès.");
                Close();
            }
        }
    }
}