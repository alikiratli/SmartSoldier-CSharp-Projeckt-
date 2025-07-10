using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SmartSoldier.Models;

namespace SmartSoldier.Views
{
    public partial class UAVView : UserControl
    {
        public UAVView()
        {
            InitializeComponent();
            // Beispiel-Daten für erkannte kritische Ziele
            var targets = new ObservableCollection<CriticalTarget>
            {
                new CriticalTarget { SiraNo = 1, Zielname = "Ziel Alpha", Gefahrentyp = "Infanterie", Distanz = 1200, Winkel = 45, Koordinaten = "52.5200°N,13.4050°E" },
                new CriticalTarget { SiraNo = 2, Zielname = "Ziel Bravo", Gefahrentyp = "Fahrzeug", Distanz = 2500, Winkel = 90, Koordinaten = "52.5200°N,13.4100°E" },
                new CriticalTarget { SiraNo = 3, Zielname = "Ziel Charlie", Gefahrentyp = "Geschütz", Distanz = 1800, Winkel = 135, Koordinaten = "52.5250°N,13.4050°E" }
            };
            dgTargets.ItemsSource = targets;
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UAV bewegt sich nach oben", "Steuerung");
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UAV bewegt sich nach unten", "Steuerung");
        }

        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UAV bewegt sich nach links", "Steuerung");
        }

        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UAV bewegt sich nach rechts", "Steuerung");
        }
    }
}
