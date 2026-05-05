using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BookMyCut.ViewModels
{
    public partial class HistoriqueViewModel : ObservableObject
    {
        private readonly IRendezVousRepository _rdvRepo;

        [ObservableProperty]
        private ObservableCollection<RendezVous> _rendezVousPasses = new();

        [ObservableProperty]
        private ObservableCollection<RendezVous> _rendezVousFuturs = new();

        [ObservableProperty]
        private bool _hasNoHistory;

        public HistoriqueViewModel(IRendezVousRepository rdvRepo)
        {
            _rdvRepo = rdvRepo;
            _ = ChargerHistoriqueAsync();
        }

        public async Task ChargerHistoriqueAsync()
        {
            if (!SessionUtilisateur.Instance.EstConnecte)
                return;

            int clientId = SessionUtilisateur.Instance.UtilisateurConnecte.Id;

            var liste = await _rdvRepo.ObtenirParClientIdAsync(clientId);

            RendezVousFuturs = new ObservableCollection<RendezVous>(
                liste.Where(r => r.DateHeure >= DateTime.Now && r.Statut != "Annulé")
                     .OrderBy(r => r.DateHeure)
            );

            RendezVousPasses = new ObservableCollection<RendezVous>(
                liste.Where(r => r.DateHeure < DateTime.Now || r.Statut == "Annulé")
                     .OrderByDescending(r => r.DateHeure)
            );

            HasNoHistory = RendezVousPasses.Count == 0 && RendezVousFuturs.Count == 0;
        }
    }
}