using System.Windows;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using BookMyCut.Utils;
using System.Windows.Controls;

namespace BookMyCut.Views
{
    public partial class ModifierProfilView : UserControl
    {
        private readonly BookMyCutContext _db;
        private Utilisateur? _utilisateurConnecte;

        public ModifierProfilView(BookMyCutContext db)
        {
            InitializeComponent();
            _db = db;
            ChargerInformationsUtilisateur();
        }

        private void ChargerInformationsUtilisateur()
        {
            if (!SessionUtilisateur.Instance.EstConnecte || SessionUtilisateur.Instance.UtilisateurConnecte == null)
            {
                AfficherMessage("Aucun utilisateur connecté.", false);
                return;
            }

            _utilisateurConnecte = SessionUtilisateur.Instance.UtilisateurConnecte;

            txtNomComplet.Text = _utilisateurConnecte.NomComplet;
            txtEmail.Text = _utilisateurConnecte.Email;

            CacherMessage();
        }

        private void AfficherMessage(string message, bool succes)
        {
            txtMessageInline.Text = message;
            MessageBoxInline.Visibility = Visibility.Visible;

            if (succes)
            {
                MessageBoxInline.Background = new SolidColorBrush(Color.FromArgb(60, 46, 204, 113));
                MessageBoxInline.BorderBrush = new SolidColorBrush(Color.FromRgb(46, 204, 113));
                txtMessageInline.Foreground = new SolidColorBrush(Color.FromRgb(46, 204, 113));
            }
            else
            {
                MessageBoxInline.Background = new SolidColorBrush(Color.FromArgb(60, 231, 76, 60));
                MessageBoxInline.BorderBrush = new SolidColorBrush(Color.FromRgb(231, 76, 60));
                txtMessageInline.Foreground = new SolidColorBrush(Color.FromRgb(255, 140, 140));
            }
        }

        private void CacherMessage()
        {
            txtMessageInline.Text = string.Empty;
            MessageBoxInline.Visibility = Visibility.Collapsed;
        }

        private async void BtnEnregistrerClick(object sender, RoutedEventArgs e)
        {
            CacherMessage();

            if (_utilisateurConnecte == null)
            {
                AfficherMessage("Utilisateur introuvable.", false);
                return;
            }

            string nomComplet = txtNomComplet.Text.Trim();
            string nouveauMotDePasse = txtMotDePasse.Password;
            string confirmationMotDePasse = txtConfirmerMotDePasse.Password;

            if (string.IsNullOrWhiteSpace(nomComplet))
            {
                AfficherMessage("Le nom complet est obligatoire.", false);
                return;
            }

            if (!string.IsNullOrWhiteSpace(nouveauMotDePasse))
            {
                if (nouveauMotDePasse.Length < 6)
                {
                    AfficherMessage("Le mot de passe doit contenir au moins 6 caractères.", false);
                    return;
                }

                if (nouveauMotDePasse != confirmationMotDePasse)
                {
                    AfficherMessage("La confirmation du mot de passe ne correspond pas.", false);
                    return;
                }
            }

            var utilisateurEnBase = await _db.Utilisateurs
                .FirstOrDefaultAsync(u => u.Id == _utilisateurConnecte.Id);

            if (utilisateurEnBase == null)
            {
                AfficherMessage("Impossible de retrouver l'utilisateur dans la base de données.", false);
                return;
            }

            utilisateurEnBase.NomComplet = nomComplet;

            if (!string.IsNullOrWhiteSpace(nouveauMotDePasse))
            {
                utilisateurEnBase.MotDePasse = Hash.HashPassword(nouveauMotDePasse);
            }

            await _db.SaveChangesAsync();

            SessionUtilisateur.Instance.Connecter(utilisateurEnBase);

            txtMotDePasse.Password = string.Empty;
            txtConfirmerMotDePasse.Password = string.Empty;

            AfficherMessage("Profil modifié avec succès.", true);

            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.MettreAJourNomUtilisateur();
        }

        private void BtnRetourClick(object sender, RoutedEventArgs e)
        {
            CacherMessage();

            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.AfficherAccueilPublic();
        }
    }
}
