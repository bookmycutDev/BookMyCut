using BookMyCut.Data.Models;

namespace BookMyCut.Data.Repositories
{
    public interface IRendezVousRepository
    {
        Task AjouterAsync(RendezVous rdv);
        Task<List<RendezVous>> ObtenirParClientIdAsync(int clientId);
        Task SupprimerAsync(int id);

        Task ModifierAsync(RendezVous rdv);
        Task<bool> EstCreneauDisponibleAsync(int coiffeurId, DateTime debut, int dureeMinutes, int? rdvIdIgnore = null);
    }
}