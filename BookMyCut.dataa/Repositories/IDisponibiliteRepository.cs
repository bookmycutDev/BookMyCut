using BookMyCut.Data.Models;

namespace BookMyCut.Data.Repositories
{
    public interface IDisponibiliteRepository
    {
        Task AjouterPlageAsync(int coiffeurId, DateTime debut, DateTime fin, int dureeCreneauMinutes);
        Task<List<Disponibilite>> ObtenirParCoiffeurAsync(int coiffeurId);
        Task<List<Disponibilite>> ObtenirDisponiblesParCoiffeurEtDateAsync(int coiffeurId, DateTime date);
        Task SupprimerAsync(int disponibiliteId);
        Task<bool> ChevauchementExisteAsync(int coiffeurId, DateTime debut, DateTime fin);
        Task MarquerCommeReserveAsync(int disponibiliteId);
    }
}
