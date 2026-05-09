using BookMyCut.Data.Models;
using BookMyCut.Data.Repositories;
using BookMyCut.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookMyCut.ViewModels
{
    public partial class DisponibilitesCoiffeurViewModel : ObservableObject
    {
        private readonly IDisponibiliteRepository _repo;

        [ObservableProperty] private DateTime _dateSelectionnee = DateTime.Today.AddDays(1);
        [ObservableProperty] private string _heureDebut = "09:00";
        [ObservableProperty] private string _heureFin = "17:00";
        [ObservableProperty] private ObservableCollection<Disponibilite> _mesDisponibilites = new();
        [ObservableProperty] private Disponibilite? _disponibiliteSelectionnee;

        public DisponibilitesCoiffeurViewModel(IDisponibiliteRepository repo)
        {
            _repo = repo;
            _ = ChargerMesDisponibilitesAsync();
        }

        [RelayCommand]
        private async Task CreerDisponibilite()
        {
            if (!TimeSpan.TryParse(HeureDebut, out var hd) || !TimeSpan.TryParse(HeureFin, out var hf))
            {
                MessageBox.Show("Heures invalides. Format attendu : HH:mm");
                return;
            }

            var debut = DateSelectionnee.Date.Add(hd);
            var fin = DateSelectionnee.Date.Add(hf);

            try
            {
                await _repo.AjouterPlageAsync(
                    SessionUtilisateur.Instance.UtilisateurConnecte.Id,
                    debut,
                    fin,
                    30);

                await ChargerMesDisponibilitesAsync();
                MessageBox.Show("Disponibilités créées avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [RelayCommand]
        private async Task SupprimerDisponibilite()
        {
            if (DisponibiliteSelectionnee == null)
                return;

            try
            {
                await _repo.SupprimerAsync(DisponibiliteSelectionnee.Id);
                await ChargerMesDisponibilitesAsync();
                MessageBox.Show("Créneau supprimé.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task ChargerMesDisponibilitesAsync()
        {
            var liste = await _repo.ObtenirParCoiffeurAsync(SessionUtilisateur.Instance.UtilisateurConnecte.Id);
            MesDisponibilites = new ObservableCollection<Disponibilite>(liste);
        }
    }
}