using System;
using System.Windows;
using System.Windows.Controls;
using BookMyCut.Views;

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

        private void BtnAccueil_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vous êtes déjà sur la page d'accueil.", "Accueil");
        }

        private void BtnServices_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("La page des services sera ajoutée plus tard.", "Services");
        }

        private void BtnCoiffeurs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("La page des coiffeurs sera ajoutée plus tard.", "Coiffeurs");
        }

        private void BtnContact_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("La page contact sera ajoutée plus tard.", "Contact");
        }

        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            ConnexionView fenetre = new ConnexionView();
            fenetre.Show();
        }

        private void BtnInscription_Click(object sender, RoutedEventArgs e)
        {
            Inscription fenetre = new Inscription();
            fenetre.Show();
        }

        private void cbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbService.SelectedIndex > 0)
            {
                ComboBoxItem selected = (ComboBoxItem)cbService.SelectedItem;
                Console.WriteLine("Service choisi : " + selected.Content.ToString());
            }
        }

        private void BtnReservationForm_Click(object sender, RoutedEventArgs e)
        {
            if (cbService.SelectedIndex <= 0)
            {
                MessageBox.Show("Veuillez choisir un service.", "Erreur");
                return;
            }

            if (cbCoiffeur.SelectedIndex <= 0)
            {
                MessageBox.Show("Veuillez choisir un coiffeur.", "Erreur");
                return;
            }

            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Veuillez choisir une date.", "Erreur");
                return;
            }

            string service = ((ComboBoxItem)cbService.SelectedItem).Content.ToString();
            string coiffeur = ((ComboBoxItem)cbCoiffeur.SelectedItem).Content.ToString();
            string date = dpDate.SelectedDate.Value.ToShortDateString();

            MessageBox.Show(
                "Rendez-vous confirmé !\n\n" +
                "Service : " + service + "\n" +
                "Coiffeur : " + coiffeur + "\n" +
                "Date : " + date,
                "Confirmation",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
}