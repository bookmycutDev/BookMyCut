using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using BookMyCut.Data.Data;
using BookMyCut.Data.Repositories;
using BookMyCut.ViewModels;
using BookMyCut.Views;
using BookMyCut.Data.Models; // Ajouté pour les modèles
using BookMyCut.Utils;       // Ajouté pour le hachage
using System.Linq;

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
            services.AddTransient<ServiceViewModel>();
            services.AddTransient<MainViewModel>();

            //enregistre les Vues
            services.AddTransient<ConnexionView>();
            services.AddTransient<InscriptionView>();
            services.AddTransient<AdminRolesView>();
            services.AddTransient<GestionServicesView>();
            services.AddTransient<HomeView>();
            services.AddTransient<MainWindow>();
            services.AddTransient<BookingView>();
            services.AddTransient<ServiceView>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                
                // Scoped au démarrage
                using (var scope = ServiceProvider.CreateScope())
                {
                    await SeedDataAsync(scope.ServiceProvider);
                }

                var loginWindow = ServiceProvider.GetRequiredService<ConnexionView>();
                if (loginWindow != null)
                {
                    loginWindow.Show();
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException?.Message);
            }
        }

        private async Task SeedDataAsync(IServiceProvider sp)
        {
            var serviceRepo = sp.GetRequiredService<IServiceRepository>();
            var userRepo = sp.GetRequiredService<IUtilisateurRepository>();

            // SERVICES (si liste vide)
            var servicesExistants = await serviceRepo.ObtenirTousAsync();
            if (!servicesExistants.Any())
            {
                await serviceRepo.AjouterAsync(new Service { Nom = "Coupe Classique", Description = "Shampoing, coupe et coiffage", Prix = 25.00m, DureeMinutes = 30 });
                await serviceRepo.AjouterAsync(new Service { Nom = "Taille de Barbe", Description = "Tracé et entretien barbe", Prix = 15.00m, DureeMinutes = 20 });
                await serviceRepo.AjouterAsync(new Service { Nom = "Forfait Complet", Description = "La totale : Coupe + Barbe", Prix = 35.00m, DureeMinutes = 50 });
            }

            //COIFFEURS (si aucun coiffeur existe)
            var utilisateurs = await userRepo.ObtenirTousAsync();
            if (!utilisateurs.Any(u => u.Role == RoleUtilisateur.Coiffeur))
            {
                string mdpHache = Hash.HashPassword("123");

                await userRepo.AjouterAsync(new Utilisateur
                {
                   
                    NomComplet = "Dusly Nestor",
                    Email = "dusly@test.com",
                    MotDePasse = mdpHache,
                    Role = RoleUtilisateur.Coiffeur
                });

                await userRepo.AjouterAsync(new Utilisateur
                {
 
                    NomComplet = "Jonathan Riquelme",
                    Email = "jonathan@test.com",
                    MotDePasse = mdpHache,
                    Role = RoleUtilisateur.Coiffeur
                });
            }
        }
    }
}