using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookMyCut.ViewModels
{
    public partial class BookingViewModel : ObservableObject
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly IUtilisateurRepository _userRepo;
        private readonly IRendezVousRepository _rdvRepo;

        [ObservableProperty] private ObservableCollection<Service> _services = new();
        [ObservableProperty] private ObservableCollection<Utilisateur> _coiffeurs = new();

        [ObservableProperty] private Service? _serviceSelectionne;
        [ObservableProperty] private Utilisateur? _coiffeurSelectionne;
        [ObservableProperty] private DateTime _dateSelectionnee = DateTime.Now.AddDays(1);

        public BookingViewModel(IServiceRepository serviceRepo, IUtilisateurRepository userRepo, IRendezVousRepository rdvRepo)
        {
            _serviceRepo = serviceRepo;
            _userRepo = userRepo;
            _rdvRepo = rdvRepo;
            _ = ChargerDonneesAsync();
        }

        private async Task ChargerDonneesAsync()
        {
            // Charger les services
            var servicesBase = await _serviceRepo.ObtenirTousAsync();
            Services = new ObservableCollection<Service>(servicesBase);

            // Charger uniquement les utilisateurs qui sont des coiffeurs
            var tousLesUsers = await _userRepo.ObtenirTousAsync();
            var listeCoiffeurs = tousLesUsers.Where(u => u.Role == RoleUtilisateur.Coiffeur).ToList();
            Coiffeurs = new ObservableCollection<Utilisateur>(listeCoiffeurs);
        }

        public Action? SurReservationReussie { get; set; } //signal

        [RelayCommand]
        private async Task ConfirmerRendezVous()
        {
            if (ServiceSelectionne == null || CoiffeurSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un service et un coiffeur.");
                return;
            }

            var rdv = new RendezVous
            {
                ClientId = SessionUtilisateur.Instance.UtilisateurConnecte.Id,
                CoiffeurId = CoiffeurSelectionne.Id,
                ServiceId = ServiceSelectionne.Id,
                DateHeure = DateSelectionnee,
                Statut = "Confirmé"
            };

            await _rdvRepo.AjouterAsync(rdv);
            MessageBox.Show("Rendez-vous enregistré avec succès !");

            //?rediriger vers l'accueil ou vider les champs
            SurReservationReussie?.Invoke();
        }
    }
}