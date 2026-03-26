using System.Windows;
using BookMyCut.Models;

namespace BookMyCut
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Utilisateur utilisateurTest = new Utilisateur
            {
                Id = 1,
                NomComplet = "Utilisateur Test",
                Email = "test@bookmycut.com",
                MotDePasse = "123456",
                Role = RoleUtilisateur.Client
            };

            ModifierProfil fenetre = new ModifierProfil(utilisateurTest);
            fenetre.Show();
        }
    }
}