using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using BookMyCut.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace BookMyCut.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IRendezVousRepository _rdvRepo;

        [ObservableProperty] private string _nomClient;
        [ObservableProperty] private ObservableCollection<RendezVous> _mesRendezVous = new();
        [ObservableProperty] private bool _hasNoAppointments;

        public HomeViewModel(IRendezVousRepository rdvRepo)
        {
            _rdvRepo = rdvRepo;
            NomClient = SessionUtilisateur.Instance.UtilisateurConnecte?.NomComplet ?? "Client";
            _ = ChargerRendezVousAsync();
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
        private async Task SupprimerRendezVous(RendezVous rdv)
        {
            if (rdv == null) return;

            await _rdvRepo.SupprimerAsync(rdv.Id);
            await ChargerRendezVousAsync();
        }
    }
}