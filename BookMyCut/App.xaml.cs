using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using BookMyCut.Data.Data;
using BookMyCut.Data.Repositories;
using BookMyCut.ViewModels;
using BookMyCut.Views;

namespace BookMyCut
{
    public partial class App : Application
    {
        // Le ServiceProvider = accéder aux services partout dans app
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services); //configure tout en premier
            ServiceProvider = services.BuildServiceProvider(); // construit catalogue(fin)
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // enregistre la Base de Données
            services.AddDbContext<BookMyCutContext>();

            //  enregistre les Repositories
            services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IRendezVousRepository, RendezVousRepository>();

            //enregistre les ViewModels
            services.AddTransient<ConnexionViewModel>();
            services.AddTransient<InscriptionViewModel>();
            services.AddTransient<AdminRolesViewModel>();
            services.AddTransient<GestionServicesViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<BookingViewModel>();

            //enregistre les Vues
            services.AddTransient<ConnexionView>();
            services.AddTransient<InscriptionView>();
            services.AddTransient<AdminRolesView>();
            services.AddTransient<GestionServicesView>();
            services.AddTransient<HomeView>();      
            services.AddTransient<MainWindow>();
            services.AddTransient<BookingView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                var loginWindow = ServiceProvider.GetRequiredService<ConnexionView>();
                if (loginWindow != null)
                {
                    loginWindow.Show();
                } // Accolade fermante du IF ajoutée
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException?.Message);
            }
        }
    }
}