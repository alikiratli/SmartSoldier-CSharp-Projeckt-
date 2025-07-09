using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using SmartSoldier.Models;

namespace SmartSoldier.Views
{
    public partial class LogistikView : UserControl
    {
        private readonly ObservableCollection<MaterialInfo> _kritischeMaterialien;
        private readonly ObservableCollection<MaterialInfo> _andereMaterialien;

        public LogistikView()
        {
            InitializeComponent();
            // Daten vorbereiten
            _kritischeMaterialien = new ObservableCollection<MaterialInfo>
            {
                new MaterialInfo { MaterialName = "Munition", Anzahl = 100, Bedarf = 150, Fehlend = 50 },
                new MaterialInfo { MaterialName = "Medikamente", Anzahl = 20, Bedarf = 50, Fehlend = 30 }
            };
            _andereMaterialien = new ObservableCollection<MaterialInfo>
            {
                new MaterialInfo { MaterialName = "Wasserflaschen", Anzahl = 200, Bedarf = 200, Fehlend = 0 },
                new MaterialInfo { MaterialName = "Verbandkästen", Anzahl = 10, Bedarf = 15, Fehlend = 5 }
            };
            // Default-Kategorie setzen und ItemsSource
            cmbKategorie.SelectedIndex = 0;
            dgMaterial.ItemsSource = _kritischeMaterialien;
        }

        private void cmbKategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbKategorie.SelectedIndex == 0)
                dgMaterial.ItemsSource = _kritischeMaterialien;
            else
                dgMaterial.ItemsSource = _andereMaterialien;
        }

        private void btnAnfrage_Click(object sender, RoutedEventArgs e)
        {
            var kategorie = (cmbKategorie.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            MessageBox.Show($"Materialanforderung für '{kategorie}' gesendet.", "Anfrage");
        }

        private void BtnAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewMaterialName.Text)
                || !int.TryParse(txtNewAnzahl.Text, out int anz)
                || !int.TryParse(txtNewBedarf.Text, out int bed))
            {
                MessageBox.Show("Bitte gültige Werte eingeben.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var neu = new MaterialInfo
            {
                MaterialName = txtNewMaterialName.Text,
                Anzahl = anz,
                Bedarf = bed,
                Fehlend = bed - anz
            };
            if (cmbKategorie.SelectedIndex == 0)
                _kritischeMaterialien.Add(neu);
            else
                _andereMaterialien.Add(neu);
            txtNewMaterialName.Clear();
            txtNewAnzahl.Clear();
            txtNewBedarf.Clear();
        }
    }
}
