using BookMyCut.Data.Models;

namespace BookMyCut.Data.Repositories
{
    // Contrat 
    public interface IUtilisateurRepository
    {
        //Récupère utilisateurs
        Task<List<Utilisateur>> ObtenirTousAsync();

        //Récupère utilisateur par ID
        Task<Utilisateur?> ObtenirParIdAsync(int id);

        //Récupère utilisateur par email et mot de passe 
        Task<Utilisateur?> ObtenirParEmailEtMotDePasseAsync(string email, string motDePasse);

        //Vérifie email existe déjà
        Task<bool> EmailExisteAsync(string email);

        //Ajoute nouvel utilisateur
        Task AjouterAsync(Utilisateur utilisateur);

        //Met à jour un 
        Task ModifierAsync(Utilisateur utilisateur);
    }
}