using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SmartSoldier.Models;

namespace SmartSoldier.Views
{
    public partial class GesundheitView : UserControl
    {
        public GesundheitView()
        {
            InitializeComponent();
            // Example health status data
            var healthList = new ObservableCollection<HealthStatus>
            {
                new HealthStatus { Vorname = "Max", Nachname = "Mustermann", Puls = 72, Temperatur = 36.6, Location = "Sektor A1" },
                new HealthStatus { Vorname = "Anna", Nachname = "Schmidt", Puls = 80, Temperatur = 37.0, Location = "Sektor B2" }
            };
            dgHealth.ItemsSource = healthList;
        }

        private void BtnErsteHilfe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is HealthStatus hs)
            {
                // Erste Hilfe an die Position des Personals senden
                MessageBox.Show($"Erste Hilfe wird zu {hs.Vorname} {hs.Nachname} in {hs.Location} entsandt.", "Erste Hilfe");
            }
        }
    }
}
