using System.Windows;
using BookMyCut.Data;
using Microsoft.EntityFrameworkCore;
using BookMyCut.Views;

namespace BookMyCut
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new BookMyCutContext())
            {
                try
                {
                    db.Database.Migrate();
                }
                catch (Microsoft.Data.Sqlite.SqliteException ex) when (ex.Message.Contains("already exists"))
                {
                    // ignore si la table existe déjà
                }
            }

            ConnexionView fenetre = new ConnexionView();
            fenetre.Show();
        }
    }
}