using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyCut.Data.Repositories
{
    public class DisponibiliteRepository : IDisponibiliteRepository
    {
        private readonly BookMyCutContext _db;

        public DisponibiliteRepository(BookMyCutContext db)
        {
            _db = db;
        }

        public async Task<bool> ChevauchementExisteAsync(int coiffeurId, DateTime debut, DateTime fin)
        {
            return await _db.Disponibilites.AnyAsync(d =>
                d.CoiffeurId == coiffeurId &&
                debut < d.Fin &&
                fin > d.Debut);
        }

        public async Task AjouterPlageAsync(int coiffeurId, DateTime debut, DateTime fin, int dureeCreneauMinutes)
        {
            if (debut >= fin)
                throw new Exception("L'heure de début doit être avant l'heure de fin.");

            bool chevauche = await ChevauchementExisteAsync(coiffeurId, debut, fin);
            if (chevauche)
                throw new Exception("Cette plage chevauche une disponibilité existante.");

            var curseur = debut;

            while (curseur.AddMinutes(dureeCreneauMinutes) <= fin)
            {
                _db.Disponibilites.Add(new Disponibilite
                {
                    CoiffeurId = coiffeurId,
                    Debut = curseur,
                    Fin = curseur.AddMinutes(dureeCreneauMinutes),
                    EstReserve = false
                });

                curseur = curseur.AddMinutes(dureeCreneauMinutes);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<List<Disponibilite>> ObtenirParCoiffeurAsync(int coiffeurId)
        {
            return await _db.Disponibilites
                .Where(d => d.CoiffeurId == coiffeurId)
                .OrderBy(d => d.Debut)
                .ToListAsync();
        }

        public async Task<List<Disponibilite>> ObtenirDisponiblesParCoiffeurEtDateAsync(int coiffeurId, DateTime date)
        {
            var jour = date.Date;
            var lendemain = jour.AddDays(1);

            return await _db.Disponibilites
                .Where(d => d.CoiffeurId == coiffeurId
                         && d.Debut >= jour
                         && d.Debut < lendemain
                         && !d.EstReserve)
                .OrderBy(d => d.Debut)
                .ToListAsync();
        }

        public async Task SupprimerAsync(int disponibiliteId)
        {
            var dispo = await _db.Disponibilites.FindAsync(disponibiliteId);

            if (dispo == null)
                return;

            if (dispo.EstReserve)
                throw new Exception("Impossible de supprimer un créneau déjà réservé.");

            _db.Disponibilites.Remove(dispo);
            await _db.SaveChangesAsync();
        }

        public async Task MarquerCommeReserveAsync(int disponibiliteId)
        {
            var dispo = await _db.Disponibilites.FindAsync(disponibiliteId);

            if (dispo == null)
                return;

            dispo.EstReserve = true;
            await _db.SaveChangesAsync();
        }
    }
}
