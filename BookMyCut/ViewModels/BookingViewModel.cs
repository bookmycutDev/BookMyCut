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

        private RendezVous? _rdvAModifier;

        [ObservableProperty] private ObservableCollection<Service> _services = new();
        [ObservableProperty] private ObservableCollection<Utilisateur> _coiffeurs = new();
        [ObservableProperty] private ObservableCollection<string> _heuresDisponibles = new();

        [ObservableProperty] private Service? _serviceSelectionne;
        [ObservableProperty] private Utilisateur? _coiffeurSelectionne;
        [ObservableProperty] private DateTime _dateSelectionnee = DateTime.Now.AddDays(1);
        [ObservableProperty] private string? _heureSelectionnee;

        [ObservableProperty] private string _titreFenetre = "PRENDRE UN RENDEZ-VOUS";
        [ObservableProperty] private string _texteBouton = "CONFIRMER LA RÉSERVATION";

        public Action? SurReservationReussie { get; set; }

        public BookingViewModel(IServiceRepository serviceRepo, IUtilisateurRepository userRepo, IRendezVousRepository rdvRepo)
        {
            _serviceRepo = serviceRepo;
            _userRepo = userRepo;
            _rdvRepo = rdvRepo;

            _ = ChargerDonneesAsync();
        }

        private async Task ChargerDonneesAsync()
        {
            var servicesBase = await _serviceRepo.ObtenirTousAsync();
            Services = new ObservableCollection<Service>(servicesBase);

            var tousLesUsers = await _userRepo.ObtenirTousAsync();
            var listeCoiffeurs = tousLesUsers
                .Where(u => u.Role == RoleUtilisateur.Coiffeur)
                .ToList();

            Coiffeurs = new ObservableCollection<Utilisateur>(listeCoiffeurs);

            AjouterHeuresParDefaut();
        }

        public async Task InitialiserModificationAsync(RendezVous rdv)
        {
            _rdvAModifier = rdv;

            TitreFenetre = "MODIFIER LE RENDEZ-VOUS";
            TexteBouton = "CONFIRMER LA MODIFICATION";

            if (!Services.Any() || !Coiffeurs.Any())
                await ChargerDonneesAsync();

            ServiceSelectionne = Services.FirstOrDefault(s => s.Id == rdv.ServiceId);
            CoiffeurSelectionne = Coiffeurs.FirstOrDefault(c => c.Id == rdv.CoiffeurId);
            DateSelectionnee = rdv.DateHeure.Date;
            HeureSelectionnee = rdv.DateHeure.ToString("HH:mm");

            await ChargerHeuresDisponiblesAsync();

            if (HeureSelectionnee != null && !HeuresDisponibles.Contains(HeureSelectionnee))
                HeuresDisponibles.Add(HeureSelectionnee);
        }

        partial void OnServiceSelectionneChanged(Service? value)
        {
            _ = ChargerHeuresDisponiblesAsync();
        }

        partial void OnCoiffeurSelectionneChanged(Utilisateur? value)
        {
            _ = ChargerHeuresDisponiblesAsync();
        }

        partial void OnDateSelectionneeChanged(DateTime value)
        {
            _ = ChargerHeuresDisponiblesAsync();
        }

        private async Task ChargerHeuresDisponiblesAsync()
        {
            HeuresDisponibles.Clear();

            if (ServiceSelectionne == null || CoiffeurSelectionne == null)
            {
                AjouterHeuresParDefaut();
                return;
            }

            for (int h = 9; h <= 17; h++)
            {
                foreach (int minute in new[] { 0, 30 })
                {
                    var debut = DateSelectionnee.Date.Add(new TimeSpan(h, minute, 0));

                    bool disponible = await _rdvRepo.EstCreneauDisponibleAsync(
                        CoiffeurSelectionne.Id,
                        debut,
                        ServiceSelectionne.DureeMinutes,
                        _rdvAModifier?.Id
                    );

                    if (disponible)
                        HeuresDisponibles.Add(debut.ToString("HH:mm"));
                }
            }
        }

        private void AjouterHeuresParDefaut()
        {
            HeuresDisponibles.Clear();

            for (int h = 9; h <= 17; h++)
            {
                HeuresDisponibles.Add($"{h:00}:00");
                HeuresDisponibles.Add($"{h:00}:30");
            }
        }

        [RelayCommand]
        private async Task ConfirmerRendezVous()
        {
            if (ServiceSelectionne == null || CoiffeurSelectionne == null || string.IsNullOrWhiteSpace(HeureSelectionnee))
            {
                MessageBox.Show("Veuillez sélectionner un service, un coiffeur et une heure.");
                return;
            }

            var dateHeure = DateSelectionnee.Date.Add(TimeSpan.Parse(HeureSelectionnee));

            bool disponible = await _rdvRepo.EstCreneauDisponibleAsync(
                CoiffeurSelectionne.Id,
                dateHeure,
                ServiceSelectionne.DureeMinutes,
                _rdvAModifier?.Id
            );

            if (!disponible)
            {
                MessageBox.Show("Ce créneau n'est plus disponible.");
                return;
            }

            if (_rdvAModifier == null)
            {
                var rdv = new RendezVous
                {
                    ClientId = SessionUtilisateur.Instance.UtilisateurConnecte.Id,
                    CoiffeurId = CoiffeurSelectionne.Id,
                    ServiceId = ServiceSelectionne.Id,
                    DateHeure = dateHeure,
                    Statut = "Confirmé"
                };

                await _rdvRepo.AjouterAsync(rdv);
                MessageBox.Show("Rendez-vous enregistré avec succès !");
            }
            else
            {
                _rdvAModifier.CoiffeurId = CoiffeurSelectionne.Id;
                _rdvAModifier.ServiceId = ServiceSelectionne.Id;
                _rdvAModifier.DateHeure = dateHeure;
                _rdvAModifier.Statut = "Confirmé";

                await _rdvRepo.ModifierAsync(_rdvAModifier);
                MessageBox.Show("Rendez-vous modifié avec succès !");
            }

            SurReservationReussie?.Invoke();
        }
    }
}