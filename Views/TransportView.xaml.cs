using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SmartSoldier.Models;

namespace SmartSoldier.Views
{
    public partial class TransportView : UserControl
    {
        private ObservableCollection<TransportAnfrage> _transportAnfragen = new();
        private int _nextId = 1;

        public TransportView()
        {
            InitializeComponent();
            InitializeVehicleTypes();
            InitializeTransportRequests();
        }

        private void InitializeVehicleTypes()
        {
            // Fahrzeugtypen hinzufügen
            cmbVehicleType.Items.Add(new ComboBoxItem { Content = "Gepanzertes Landfahrzeug" });
            cmbVehicleType.Items.Add(new ComboBoxItem { Content = "Boot" });
            cmbVehicleType.Items.Add(new ComboBoxItem { Content = "Hubschrauber" });
            cmbVehicleType.Items.Add(new ComboBoxItem { Content = "Flugzeug" });
            cmbVehicleType.SelectedIndex = 0;
        }

        private void InitializeTransportRequests()
        {
            // DataGrid mit der ObservableCollection verbinden
            dgTransportAnfragen.ItemsSource = _transportAnfragen;

            // Beispiel-Transportanfragen hinzufügen
            _transportAnfragen.Add(new TransportAnfrage
            {
                Id = _nextId++,
                Von = "Basis Alpha",
                Nach = "Checkpoint Bravo",
                Fahrzeugtyp = "Gepanzertes Landfahrzeug",
                Zeit = "14:30",
                Zeitstempel = DateTime.Now.AddMinutes(-15),
                Status = "Unterwegs"
            });

            _transportAnfragen.Add(new TransportAnfrage
            {
                Id = _nextId++,
                Von = "FOB Charlie",
                Nach = "LZ Delta",
                Fahrzeugtyp = "Hubschrauber",
                Zeit = "16:00",
                Zeitstempel = DateTime.Now.AddMinutes(-5),
                Status = "Angefordert"
            });
        }

        private void BtnSenden_Click(object sender, RoutedEventArgs e)
        {
            // Validierung
            if (string.IsNullOrWhiteSpace(txtVon.Text) ||
                string.IsNullOrWhiteSpace(txtNach.Text) ||
                string.IsNullOrWhiteSpace(txtZeit.Text))
            {
                MessageBox.Show("Bitte alle Felder ausfüllen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var von = txtVon.Text;
            var nach = txtNach.Text;
            var fahrzeug = (cmbVehicleType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            var zeit = txtZeit.Text;

            // Neue Transportanfrage erstellen
            var neueAnfrage = new TransportAnfrage
            {
                Id = _nextId++,
                Von = von,
                Nach = nach,
                Fahrzeugtyp = fahrzeug,
                Zeit = zeit,
                Zeitstempel = DateTime.Now,
                Status = "Angefordert"
            };

            // Zur Liste hinzufügen
            _transportAnfragen.Add(neueAnfrage);

            // Bestätigungsmeldung
            MessageBox.Show($"Transportanfrage #{neueAnfrage.Id} gesendet:\nVon: {von}\nNach: {nach}\nFahrzeugtyp: {fahrzeug}\nZeit: {zeit}", 
                           "Transportanfrage", MessageBoxButton.OK, MessageBoxImage.Information);

            // Felder löschen
            txtVon.Clear();
            txtNach.Clear();
            txtZeit.Clear();
            cmbVehicleType.SelectedIndex = 0;
        }
    }
}
