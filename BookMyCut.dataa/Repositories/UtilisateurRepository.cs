using BookMyCut.Data.Data;
using BookMyCut.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyCut.Data.Repositories
{
    //Repository
    //EF Core
    public class UtilisateurRepository : IUtilisateurRepository
    {
        //accès à la BD
        private readonly BookMyCutContext _db;

        public UtilisateurRepository(BookMyCutContext db)
        {
            _db = db;
        }

        // Récupère utilisateurs BD
        public async Task<List<Utilisateur>> ObtenirTousAsync()
        {
            return await _db.Utilisateurs.ToListAsync();
        }

        // Récupère utilisateur par ID
        public async Task<Utilisateur?> ObtenirParIdAsync(int id)
        {
            return await _db.Utilisateurs.FindAsync(id);
        }

        // Cherche utilisateur  email + mot de passe (connexion)
        public async Task<Utilisateur?> ObtenirParEmailEtMotDePasseAsync(string email, string motDePasse)
        {
            return await _db.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == email && u.MotDePasse == motDePasse);
        }

        // Vérifie email utilisé (inscription)
        public async Task<bool> EmailExisteAsync(string email)
        {
            return await _db.Utilisateurs
                .AnyAsync(u => u.Email == email);
        }

        // Ajoute nouvel utilisateur et sauvegarde
        public async Task AjouterAsync(Utilisateur utilisateur)
        {
            _db.Utilisateurs.Add(utilisateur);
            await _db.SaveChangesAsync();
        }

        // Met à jour un utilisateur et sauvegarde
        public async Task ModifierAsync(Utilisateur utilisateur)
        {
            _db.Utilisateurs.Update(utilisateur);
            await _db.SaveChangesAsync();
        }
    }
}