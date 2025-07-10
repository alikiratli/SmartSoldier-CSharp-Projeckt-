using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SmartSoldier.Models;

namespace SmartSoldier.Views
{
    public partial class FeuerUnterstuetzungView : UserControl
    {
        private ObservableCollection<FeuerAnfrage> _feuerAnfragen = new();
        private int _nextId = 1;

        public FeuerUnterstuetzungView()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            dgFeuerAnfragen.ItemsSource = _feuerAnfragen;

            // Beispiel-Daten hinzufügen
            _feuerAnfragen.Add(new FeuerAnfrage 
            { 
                Id = _nextId++, 
                Koordinaten = "52.5200°N,13.4050°E", 
                Feuerart = "Indirektes Feuer", 
                Dauer = "5", 
                Zeit = "Sofort", 
                Zeitstempel = DateTime.Now.AddMinutes(-10),
                Status = "Bestätigt"
            });
        }

        private void BtnSenden_Click(object sender, RoutedEventArgs e)
        {
            var koord = txtKoordinaten.Text;
            var feuerArt = (cmbFeuerart.SelectedItem as ComboBoxItem)?.Content.ToString();
            var dauer = txtDauer.Text;
            var zeit = txtZeit.Text;

            // Validierung
            if (string.IsNullOrWhiteSpace(koord) || string.IsNullOrWhiteSpace(feuerArt) || 
                string.IsNullOrWhiteSpace(dauer) || string.IsNullOrWhiteSpace(zeit))
            {
                MessageBox.Show("Bitte alle Felder ausfüllen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Neue Anfrage zur Liste hinzufügen
            var neueAnfrage = new FeuerAnfrage
            {
                Id = _nextId++,
                Koordinaten = koord,
                Feuerart = feuerArt,
                Dauer = dauer,
                Zeit = zeit,
                Zeitstempel = DateTime.Now,
                Status = "Gesendet"
            };

            _feuerAnfragen.Add(neueAnfrage);

            // Formular zurücksetzen
            txtKoordinaten.Clear();
            cmbFeuerart.SelectedIndex = -1;
            txtDauer.Clear();
            txtZeit.Clear();

            MessageBox.Show($"Feuerunterstützungsanfrage gesendet!\nID: {neueAnfrage.Id}\nKoordinaten: {koord}\nFeuerart: {feuerArt}\nDauer: {dauer} Min\nZeit: {zeit}", 
                           "Anfrage gesendet", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
