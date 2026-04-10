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
        // Le ServiceProvider permet d'accéder aux services partout dans l'app
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // 1. Enregistre la Base de Données
            services.AddDbContext<BookMyCutContext>();

            // 2. Enregistre le Repository (Scoped = une instance par session)
            services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

            // 3. Enregistre les ViewModels
            services.AddTransient<ConnexionViewModel>();
            services.AddTransient<InscriptionViewModel>();
            services.AddTransient<AdminRolesViewModel>();

            // 4. Enregistre les Vues (Fenêtres)
            services.AddTransient<ConnexionView>();
            services.AddTransient<InscriptionView>();
            services.AddTransient<AdminRolesView>();
            services.AddTransient<MainWindow>();

            services.AddScoped<IServiceRepository, ServiceRepository>();

            services.AddTransient<GestionServicesViewModel>();
            services.AddTransient<GestionServicesView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // On lance la première fenêtre via le ServiceProvider
            var loginWindow = ServiceProvider.GetRequiredService<ConnexionView>();
            loginWindow.Show();
        }
    }
}