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
        private readonly IDisponibiliteRepository _dispoRepo;

        [ObservableProperty] private ObservableCollection<Service> _services = new();
        [ObservableProperty] private ObservableCollection<Utilisateur> _coiffeurs = new();
        [ObservableProperty] private ObservableCollection<Disponibilite> _creneauxDisponibles = new();

        [ObservableProperty] private Service? _serviceSelectionne;
        [ObservableProperty] private Utilisateur? _coiffeurSelectionne;
        [ObservableProperty] private Disponibilite? _creneauSelectionne;
        [ObservableProperty] private DateTime _dateSelectionnee = DateTime.Today.AddDays(1);

        public BookingViewModel(
            IServiceRepository serviceRepo,
            IUtilisateurRepository userRepo,
            IRendezVousRepository rdvRepo,
            IDisponibiliteRepository dispoRepo)
        {
            _serviceRepo = serviceRepo;
            _userRepo = userRepo;
            _rdvRepo = rdvRepo;
            _dispoRepo = dispoRepo;

            _ = ChargerDonneesAsync();
        }

        public Action? SurReservationReussie { get; set; }

        private async Task ChargerDonneesAsync()
        {
            var servicesBase = await _serviceRepo.ObtenirTousAsync();
            Services = new ObservableCollection<Service>(servicesBase);

            var tousLesUsers = await _userRepo.ObtenirTousAsync();
            var listeCoiffeurs = tousLesUsers
                .Where(u => u.Role == RoleUtilisateur.Coiffeur)
                .ToList();

            Coiffeurs = new ObservableCollection<Utilisateur>(listeCoiffeurs);
        }

        partial void OnCoiffeurSelectionneChanged(Utilisateur? value)
        {
            _ = ChargerCreneauxAsync();
        }

        partial void OnDateSelectionneeChanged(DateTime value)
        {
            _ = ChargerCreneauxAsync();
        }

        private async Task ChargerCreneauxAsync()
        {
            CreneauSelectionne = null;
            CreneauxDisponibles.Clear();

            if (CoiffeurSelectionne == null)
                return;

            var liste = await _dispoRepo.ObtenirDisponiblesParCoiffeurEtDateAsync(
                CoiffeurSelectionne.Id,
                DateSelectionnee);

            CreneauxDisponibles = new ObservableCollection<Disponibilite>(liste);
        }

        [RelayCommand]
        private async Task ConfirmerRendezVous()
        {
            if (ServiceSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un service.");
                return;
            }

            if (CoiffeurSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un coiffeur.");
                return;
            }

            if (CreneauSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un créneau disponible.");
                return;
            }

            var rdv = new RendezVous
            {
                ClientId = SessionUtilisateur.Instance.UtilisateurConnecte.Id,
                CoiffeurId = CoiffeurSelectionne.Id,
                ServiceId = ServiceSelectionne.Id,
                DateHeure = CreneauSelectionne.Debut,
                Statut = "Confirmé"
            };

            await _rdvRepo.AjouterAsync(rdv);

            CreneauSelectionne.EstReserve = true;
            await _dispoRepo.MarquerCommeReserveAsync(CreneauSelectionne.Id);

            MessageBox.Show("Rendez-vous enregistré avec succès !");
            await ChargerCreneauxAsync();

            SurReservationReussie?.Invoke();
        }
    }
}