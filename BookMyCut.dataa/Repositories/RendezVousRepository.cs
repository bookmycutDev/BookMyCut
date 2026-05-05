using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyCut.Data.Repositories
{
    public class RendezVousRepository : IRendezVousRepository
    {
        private readonly BookMyCutContext _db;

        public RendezVousRepository(BookMyCutContext db) => _db = db;

        public async Task AjouterAsync(RendezVous rdv)
        {
            _db.RendezVous.Add(rdv);
            await _db.SaveChangesAsync();
        }

        public async Task<List<RendezVous>> ObtenirParClientIdAsync(int clientId)
        {
            return await _db.RendezVous
                .AsNoTracking()
                .Include(r => r.Service)
                .Include(r => r.Coiffeur)
                .Where(rdv => rdv.ClientId == clientId)
                .ToListAsync();
        }

        public async Task SupprimerAsync(int id)
        {
            var rdv = await _db.RendezVous.FindAsync(id);

            if (rdv != null)
            {
                _db.RendezVous.Remove(rdv);
                await _db.SaveChangesAsync();
            }
        }

        public async Task ModifierAsync(RendezVous rdv)
        {
            var rdvExistant = await _db.RendezVous
                .FirstOrDefaultAsync(r => r.Id == rdv.Id);

            if (rdvExistant == null)
                return;

            rdvExistant.ServiceId = rdv.ServiceId;
            rdvExistant.CoiffeurId = rdv.CoiffeurId;
            rdvExistant.ClientId = rdv.ClientId;
            rdvExistant.DateHeure = rdv.DateHeure;
            rdvExistant.Statut = rdv.Statut;

            await _db.SaveChangesAsync();
        }

        public async Task<bool> EstCreneauDisponibleAsync(int coiffeurId, DateTime debut, int dureeMinutes, int? rdvIdIgnore = null)
        {
            DateTime fin = debut.AddMinutes(dureeMinutes);

            var rdvs = await _db.RendezVous
                .Include(r => r.Service)
                .Where(r => r.CoiffeurId == coiffeurId && r.Statut == "Confirmé")
                .ToListAsync();

            return !rdvs.Any(r =>
                r.Id != rdvIdIgnore &&
                debut < r.DateHeure.AddMinutes(r.Service != null ? r.Service.DureeMinutes : 30) &&
                fin > r.DateHeure
            );
        }
    }
}