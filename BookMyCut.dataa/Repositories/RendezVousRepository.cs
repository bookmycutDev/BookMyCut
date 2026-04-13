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
    }
}