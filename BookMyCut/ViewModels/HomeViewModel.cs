using System.Collections.ObjectModel;
using System.Windows;
using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using BookMyCut.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace BookMyCut.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IRendezVousRepository _rdvRepo;
        private readonly IDisponibiliteRepository _dispoRepo;
        private readonly IUtilisateurRepository _userRepo;

        [ObservableProperty] private string _nomClient;
        [ObservableProperty] private ObservableCollection<RendezVous> _mesRendezVous = new();
        [ObservableProperty] private bool _hasNoAppointments;
        [ObservableProperty] private ObservableCollection<Utilisateur> _coiffeursListe = new();
        [ObservableProperty] private Utilisateur? _coiffeurFiltre;
        [ObservableProperty] private DateTime _dateFiltreDispos = DateTime.Today;
        [ObservableProperty] private ObservableCollection<Disponibilite> _disposTempsReel = new();

        public HomeViewModel(IRendezVousRepository rdvRepo, IDisponibiliteRepository dispoRepo, IUtilisateurRepository userRepo)
        {
            _rdvRepo = rdvRepo;
            _dispoRepo = dispoRepo;
            _userRepo = userRepo;
            NomClient = SessionUtilisateur.Instance.UtilisateurConnecte?.NomComplet ?? "Client";
            _ = ChargerRendezVousAsync();
            _ = ChargerCoiffeursAsync();

        }

        public async Task ChargerRendezVousAsync()
        {
            if (!SessionUtilisateur.Instance.EstConnecte) return;

            int currentUserId = SessionUtilisateur.Instance.UtilisateurConnecte.Id;

            var liste = await _rdvRepo.ObtenirParClientIdAsync(currentUserId);

            var listeConfirmee = liste
                .Where(r => r.Statut == "Confirmé")
                .OrderBy(r => r.DateHeure)
                .ToList();

            MesRendezVous = new ObservableCollection<RendezVous>(listeConfirmee);
            HasNoAppointments = MesRendezVous.Count == 0;
        }

        [RelayCommand]
        private void OuvrirBooking()
        {
            var bookingView = App.ServiceProvider.GetRequiredService<BookingView>();

            if (bookingView.DataContext is BookingViewModel bookingVm)
            {
                bookingVm.SurReservationReussie = async () =>
                {
                    await ChargerRendezVousAsync();
                    bookingView.Close();
                };
            }

            bookingView.ShowDialog();
        }

        [RelayCommand]
        private async Task ModifierRendezVous(RendezVous rdv)
        {
            if (rdv == null) return;

            var bookingView = App.ServiceProvider.GetRequiredService<BookingView>();

            if (bookingView.DataContext is BookingViewModel bookingVm)
            {
                await bookingVm.InitialiserModificationAsync(rdv);

                bookingVm.SurReservationReussie = async () =>
                {
                    await ChargerRendezVousAsync();
                    bookingView.Close();
                };
            }

            bookingView.ShowDialog();
        }

        [RelayCommand]
        private async Task AnnulerRendezVous(RendezVous rdv)
        {
            if (rdv == null)
                return;

            var result = MessageBox.Show(
                "Voulez-vous vraiment annuler ce rendez-vous ?",
                "Confirmation d'annulation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes)
                return;

            await _rdvRepo.AnnulerAsync(rdv.Id);

            //Liberer le creneau chez le coiffeur
            await _dispoRepo.LibererParCoiffeurEtDebutAsync(rdv.CoiffeurId, rdv.DateHeure);

            MessageBox.Show(
                "Votre rendez-vous a été annulé avec succès.",
                "Annulation confirmée",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

            await ChargerRendezVousAsync();
        }

        private async Task ChargerCoiffeursAsync()
        {
            var tous = await _userRepo.ObtenirTousAsync();
            CoiffeursListe = new ObservableCollection<Utilisateur>(
                tous.Where(u => u.Role == RoleUtilisateur.Coiffeur)
            );
        }

        partial void OnCoiffeurFiltreChanged(Utilisateur? value)
        {
            _ = ChargerDisposTempsReelAsync();
        }

        partial void OnDateFiltreDisposChanged(DateTime value)
        {
            _ = ChargerDisposTempsReelAsync();
        }

        private async Task ChargerDisposTempsReelAsync()
        {
            if (CoiffeurFiltre == null)
            {
                DisposTempsReel = new ObservableCollection<Disponibilite>();
                return;
            }

            var liste = await _dispoRepo.ObtenirDisponiblesParCoiffeurEtDateAsync(
                CoiffeurFiltre.Id, DateFiltreDispos);

            DisposTempsReel = new ObservableCollection<Disponibilite>(liste);
        }
    }

}