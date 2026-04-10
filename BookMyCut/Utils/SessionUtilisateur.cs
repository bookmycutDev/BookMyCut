
using BookMyCut.Data.Models;

namespace BookMyCut.Utils
{
    public class SessionUtilisateur
    {
        // Instance unique
        private static SessionUtilisateur _instance;
        private static readonly object _lock = new object();

        // L'utilisateur actuellement connecté
        public Utilisateur UtilisateurConnecte { get; private set; }

        // Vrai si quelqu'un est connecté
        public bool EstConnecte => UtilisateurConnecte != null;

        // Constructeur privé — empêche new SessionUtilisateur()
        private SessionUtilisateur() { }

        // Seul point d'accès global
        public static SessionUtilisateur Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new SessionUtilisateur();
                    }
                }
                return _instance;
            }
        }

        // Appelé après connexion réussie
        public void Connecter(Utilisateur utilisateur)
        {
            UtilisateurConnecte = utilisateur;
        }

        // Appelé lors de la déconnexion
        public void Deconnecter()
        {
            UtilisateurConnecte = null;
        }
    }
}