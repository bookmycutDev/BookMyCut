using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookMyCut.ViewModels
{
    public partial class NosCoiffeursViewModel : ObservableObject
    {
        private readonly IUtilisateurRepository _repo;

        [ObservableProperty]
        private ObservableCollection<CoiffeurCarte> _coiffeurs = new();

        [ObservableProperty]
        private bool _aucunCoiffeur;

        public NosCoiffeursViewModel(IUtilisateurRepository repo)
        {
            _repo = repo;
            _ = ChargerCoiffeursAsync();
        }

        private async Task ChargerCoiffeursAsync()
        {
            var utilisateurs = await _repo.ObtenirTousAsync();

            var coiffeursDb = utilisateurs
                .Where(u => u.Role == RoleUtilisateur.Coiffeur)
                .ToList();

            var liste = new List<CoiffeurCarte>();

            for (int i = 0; i < coiffeursDb.Count; i++)
            {
                var coiffeur = coiffeursDb[i];

                liste.Add(new CoiffeurCarte
                {
                    NomComplet = coiffeur.NomComplet,
                    Initiales = ExtraireInitiales(coiffeur.NomComplet),
                    Description = GenererDescription(i)
                });
            }

            Coiffeurs = new ObservableCollection<CoiffeurCarte>(liste);
            AucunCoiffeur = Coiffeurs.Count == 0;
        }

        private string ExtraireInitiales(string nomComplet)
        {
            if (string.IsNullOrWhiteSpace(nomComplet))
                return "?";

            var parties = nomComplet
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Take(2)
                .Select(p => char.ToUpper(p[0]));

            return string.Concat(parties);
        }

        private string GenererDescription(int index)
        {
            string[] descriptions =
            {
                "Spécialiste des coupes modernes, dégradés propres et finitions précises.",
                "Passionné par les styles classiques et les looks soignés adaptés à chaque client.",
                "Expert en transformation capillaire avec une approche attentive et professionnelle.",
                "Toujours à l’écoute pour offrir une coupe nette, stylée et adaptée à votre visage.",
                "Coiffeur minutieux, idéal pour un résultat propre, actuel et bien structuré."
            };

            return descriptions[index % descriptions.Length];
        }
    }

    public class CoiffeurCarte
    {
        public string NomComplet { get; set; } = string.Empty;
        public string Initiales { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}