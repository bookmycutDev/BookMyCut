using System.Collections.ObjectModel;
using System.Windows;
using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookMyCut.ViewModels
{
    public partial class BookingViewModel : ObservableObject
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly IUtilisateurRepository _userRepo;
        private readonly IRendezVousRepository _rdvRepo;
        private readonly IDisponibiliteRepository _dispoRepo;

        private RendezVous? _rdvAModifier;

        [ObservableProperty] private ObservableCollection<Service> _services = new();
        [ObservableProperty] private ObservableCollection<Utilisateur> _coiffeurs = new();

        // MASTER
        [ObservableProperty] private ObservableCollection<string> _heuresDisponibles = new();

        // JONATHAN2.0
        [ObservableProperty] private ObservableCollection<Disponibilite> _creneauxDisponibles = new();

        [ObservableProperty] private Service? _serviceSelectionne;
        [ObservableProperty] private Utilisateur? _coiffeurSelectionne;
        [ObservableProperty] private DateTime _dateSelectionnee = DateTime.Now.AddDays(1);

        // MASTER
        [ObservableProperty] private string? _heureSelectionnee;

        // JONATHAN2.0
        [ObservableProperty] private Disponibilite? _creneauSelectionne;

        // MASTER
        [ObservableProperty] private string _titreFenetre = "PRENDRE UN RENDEZ-VOUS";
        [ObservableProperty] private string _texteBouton = "CONFIRMER LA RÉSERVATION";

        public Action? SurReservationReussie { get; set; }

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

        private async Task ChargerDonneesAsync()
        {
            var servicesBase = await _serviceRepo.ObtenirTousAsync();
            Services = new ObservableCollection<Service>(servicesBase);

            var tousLesUsers = await _userRepo.ObtenirTousAsync();
            var listeCoiffeurs = tousLesUsers
                .Where(u => u.Role == RoleUtilisateur.Coiffeur)
                .ToList();

            Coiffeurs = new ObservableCollection<Utilisateur>(listeCoiffeurs);

            await ChargerCreneauxAsync();
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

            await ChargerCreneauxAsync();

            // Compatibilité avec l'ancien système d'heures
            if (HeureSelectionnee != null && !HeuresDisponibles.Contains(HeureSelectionnee))
                HeuresDisponibles.Add(HeureSelectionnee);

            // Compatibilité avec le nouveau système de créneaux
            CreneauSelectionne = CreneauxDisponibles.FirstOrDefault(c => c.Debut == rdv.DateHeure);

            if (CreneauSelectionne == null)
            {
                CreneauSelectionne = new Disponibilite
                {
                    Id = 0,
                    CoiffeurId = rdv.CoiffeurId,
                    Debut = rdv.DateHeure,
                    Fin = rdv.DateHeure.AddMinutes(ServiceSelectionne?.DureeMinutes ?? 30),
                    EstReserve = true
                };

                CreneauxDisponibles.Add(CreneauSelectionne);
            }
        }

        partial void OnServiceSelectionneChanged(Service? value)
        {
            _ = ChargerCreneauxAsync();
        }

        partial void OnCoiffeurSelectionneChanged(Utilisateur? value)
        {
            _ = ChargerCreneauxAsync();
        }

        partial void OnDateSelectionneeChanged(DateTime value)
        {
            _ = ChargerCreneauxAsync();
        }

        partial void OnCreneauSelectionneChanged(Disponibilite? value)
        {
            if (value != null)
                HeureSelectionnee = value.Debut.ToString("HH:mm");
        }

        partial void OnHeureSelectionneeChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var creneau = CreneauxDisponibles.FirstOrDefault(c => c.Debut.ToString("HH:mm") == value);
            if (creneau != null)
                CreneauSelectionne = creneau;
        }

        private async Task ChargerCreneauxAsync()
        {
            HeureSelectionnee = null;
            CreneauSelectionne = null;
            HeuresDisponibles.Clear();
            CreneauxDisponibles.Clear();

            if (ServiceSelectionne == null || CoiffeurSelectionne == null)
                return;

            var liste = await _dispoRepo.ObtenirDisponiblesParCoiffeurEtDateAsync(
                CoiffeurSelectionne.Id,
                DateSelectionnee);

            var creneauxValides = new List<Disponibilite>();

            foreach (var creneau in liste.OrderBy(c => c.Debut))
            {
                bool disponible = await _rdvRepo.EstCreneauDisponibleAsync(
                    CoiffeurSelectionne.Id,
                    creneau.Debut,
                    ServiceSelectionne.DureeMinutes,
                    _rdvAModifier?.Id
                );

                if (disponible)
                {
                    creneauxValides.Add(creneau);
                }
            }

            CreneauxDisponibles = new ObservableCollection<Disponibilite>(creneauxValides);

            foreach (var creneau in creneauxValides)
            {
                HeuresDisponibles.Add(creneau.Debut.ToString("HH:mm"));
            }
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

            DateTime dateHeure;

            // Priorité au nouveau système de créneaux
            if (CreneauSelectionne != null)
            {
                dateHeure = CreneauSelectionne.Debut;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(HeureSelectionnee))
                {
                    MessageBox.Show("Veuillez sélectionner un créneau disponible.");
                    return;
                }

                dateHeure = DateSelectionnee.Date.Add(TimeSpan.Parse(HeureSelectionnee));
            }

            if (dateHeure <= DateTime.Now)
            {
                MessageBox.Show(
                    "Impossible de choisir une date ou une heure déjà passée.",
                    "Date invalide",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

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

                if (CreneauSelectionne != null && CreneauSelectionne.Id > 0)
                {
                    CreneauSelectionne.EstReserve = true;
                    await _dispoRepo.MarquerCommeReserveAsync(CreneauSelectionne.Id);
                }

                MessageBox.Show("Rendez-vous enregistré avec succès !");
            }
            else
            {
                //Sauvegarder aciennes valeur avant de les ecraser.
                int ancienCoiffeurId = _rdvAModifier.CoiffeurId;
                DateTime ancienneDateHeure = _rdvAModifier.DateHeure;

                _rdvAModifier.CoiffeurId = CoiffeurSelectionne.Id;
                _rdvAModifier.ServiceId = ServiceSelectionne.Id;
                _rdvAModifier.DateHeure = dateHeure;
                _rdvAModifier.Statut = "Confirmé";

                await _rdvRepo.ModifierAsync(_rdvAModifier);

                //liberer l'acien creneau
                await _dispoRepo.LibererParCoiffeurEtDebutAsync(ancienCoiffeurId, ancienneDateHeure);

                //Marquer le Nouveau creneau comme reserver
                if (CreneauSelectionne != null && CreneauSelectionne.Id > 0)
                {
                    CreneauSelectionne.EstReserve = true;
                    await _dispoRepo.MarquerCommeReserveAsync(CreneauSelectionne.Id);
                }

                MessageBox.Show("Rendez-vous modifié avec succès !");
            }

            await ChargerCreneauxAsync();
            SurReservationReussie?.Invoke();
        }
    }
}
