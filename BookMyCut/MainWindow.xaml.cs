using System;
using System.Windows;
using System.Windows.Controls;
using BookMyCut.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BookMyCut
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cbService.SelectedIndex = 0;
            cbCoiffeur.SelectedIndex = 0;
            dpDate.SelectedDate = DateTime.Today;
        }

        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            var fenetre = App.ServiceProvider.GetRequiredService<ConnexionView>();
            fenetre.Show();
        }

        private void BtnInscription_Click(object sender, RoutedEventArgs e)
        {
            var fenetre = App.ServiceProvider.GetRequiredService<InscriptionView>();
            fenetre.Show();
        }

        private void BtnAccueil_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vous êtes déjà sur la page d'accueil.", "Accueil");
        }

        private void BtnReservationForm_Click(object sender, RoutedEventArgs e)
        {
            if (cbService.SelectedIndex <= 0 || cbCoiffeur.SelectedIndex <= 0 || dpDate.SelectedDate == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Champs manquants",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string service = ((ComboBoxItem)cbService.SelectedItem).Content.ToString();
            string coiffeur = ((ComboBoxItem)cbCoiffeur.SelectedItem).Content.ToString();
            string date = dpDate.SelectedDate.Value.ToShortDateString();

            MessageBox.Show(
                $"Rendez-vous confirmé !\n\nService : {service}\nCoiffeur : {coiffeur}\nDate : {date}",
                "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void cbService_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void BtnServices_Click(object sender, RoutedEventArgs e) => MessageBox.Show("À venir...");
        private void BtnCoiffeurs_Click(object sender, RoutedEventArgs e) => MessageBox.Show("À venir...");
        private void BtnContact_Click(object sender, RoutedEventArgs e) => MessageBox.Show("À venir...");
    }
}