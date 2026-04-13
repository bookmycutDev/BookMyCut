using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using BookMyCut.Data.Data;
using BookMyCut.Data.Repositories;
using BookMyCut.ViewModels;
using BookMyCut.Views;
using BookMyCut.Data.Models;

namespace BookMyCut
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookMyCutContext>();

            services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IRendezVousRepository, RendezVousRepository>();

            services.AddTransient<ConnexionViewModel>();
            services.AddTransient<InscriptionViewModel>();
            services.AddTransient<AdminRolesViewModel>();
            services.AddTransient<GestionServicesViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<BookingViewModel>();

            services.AddTransient<ConnexionView>();
            services.AddTransient<InscriptionView>();
            services.AddTransient<AdminRolesView>();
            services.AddTransient<GestionServicesView>();
            services.AddTransient<HomeView>();
            services.AddTransient<MainWindow>();
            services.AddTransient<BookingView>();

            // AJOUTE ÇA
            services.AddTransient<ModifierProfil>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var loginWindow = ServiceProvider.GetRequiredService<ConnexionView>();
                loginWindow?.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException?.Message);
            }
        }
    }
}