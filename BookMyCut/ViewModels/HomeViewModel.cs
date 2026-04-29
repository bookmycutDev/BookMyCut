using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using BookMyCut.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyCut.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IRendezVousRepository _rdvRepo;

        [ObservableProperty] private string _nomClient;
        [ObservableProperty] private ObservableCollection<RendezVous> _mesRendezVous = new();
        [ObservableProperty] private bool _hasNoAppointments;

        // Change le constructeur pour injecter le repository
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

            // filtre par statut 
            var listeConfirmee = liste.Where(r => r.Statut == "Confirmé").OrderBy(r => r.DateHeure).ToList();

            // On met à jour l'interface
            MesRendezVous = new ObservableCollection<RendezVous>(listeConfirmee);
            HasNoAppointments = (MesRendezVous.Count == 0);
        }

        [RelayCommand]
        private void OuvrirBooking()
        {
            // Ouvre la fenêtre de réservation via l'injection
            var bookingView = App.ServiceProvider.GetRequiredService<BookingView>();

            if (bookingView.DataContext is BookingViewModel bookingVm)
            {
                // Action déclenchée quand le RDV est validé
                bookingVm.SurReservationReussie = async () =>
                {
                    await ChargerRendezVousAsync(); // Rafraîchit la liste
                    bookingView.Close();           // Ferme la fenêtre
                };
            }

            bookingView.ShowDialog();
        }

    }
}