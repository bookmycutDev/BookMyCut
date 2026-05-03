using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BookMyCut.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IRendezVousRepository _rdvRepo;

        [ObservableProperty] private string _nomClient = string.Empty;
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
            if (!SessionUtilisateur.Instance.EstConnecte)
                return;

            int currentUserId = SessionUtilisateur.Instance.UtilisateurConnecte.Id;

            var liste = await _rdvRepo.ObtenirParClientIdAsync(currentUserId);

            var listeConfirmee = liste
                .Where(r => r.Statut == "Confirmé")
                .OrderBy(r => r.DateHeure)
                .ToList();

            MesRendezVous = new ObservableCollection<RendezVous>(listeConfirmee);
            HasNoAppointments = MesRendezVous.Count == 0;
        }
    }
}