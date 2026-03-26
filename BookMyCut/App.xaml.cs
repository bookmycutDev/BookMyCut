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
                db.Database.Migrate();
            }

            ConnexionView fenetre = new ConnexionView();
            fenetre.Show();
        }
    }
}