using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BookMyCut.ViewModels
{
    public partial class AdminRolesViewModel : ObservableObject
    {
        private readonly BookMyCutContext _db = new BookMyCutContext();

        // Liste update automatiquement
        [ObservableProperty]
        private ObservableCollection<Utilisateur> _utilisateurs = new();

        //utilisateur dans tableau
        [ObservableProperty]
        private Utilisateur? _utilisateurSelectionne;

        public AdminRolesViewModel()
        {
            ChargerDonnees();
        }

        //tous utilisateurs de SQLite
        private void ChargerDonnees()
        {
            try
            {
                var liste = _db.Utilisateurs.ToList();
                Utilisateurs = new ObservableCollection<Utilisateur>(liste);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de base de données : {ex.Message}", "Erreur");
            }
        }
        [RelayCommand]
        private void ModifierRole(RoleUtilisateur nouveauRole)
        {
            if (UtilisateurSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur dans la liste.");
                return;
            }

            // Mise à jour dans la base de données
            UtilisateurSelectionne.Role = nouveauRole;
            _db.Utilisateurs.Update(UtilisateurSelectionne);
            _db.SaveChanges();

            MessageBox.Show($"{UtilisateurSelectionne.NomComplet} est maintenant {nouveauRole}.");

            // Rafraîchissement visuel
            ChargerDonnees();
        }
    }
}