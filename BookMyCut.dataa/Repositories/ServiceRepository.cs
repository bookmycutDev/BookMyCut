using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyCut.Data.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly BookMyCutContext _db;

        public ServiceRepository(BookMyCutContext db)
        {
            _db = db;
        }

        public async Task<List<Service>> ObtenirTousAsync()
        {
            return await _db.Services.ToListAsync();
        }

        public async Task AjouterAsync(Service service)
        {
            _db.Services.Add(service);
            await _db.SaveChangesAsync();
        }

        public async Task ModifierAsync(Service service)
        {
            _db.Services.Update(service);
            await _db.SaveChangesAsync();
        }

        public async Task SupprimerAsync(int id)
        {
            var rdv = await _db.RendezVous.FindAsync(id);

            if (rdv != null)
            {
                rdv.Statut = "Annulé";
                await _db.SaveChangesAsync();
            }
        }
    }
}