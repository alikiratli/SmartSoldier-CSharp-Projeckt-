using System.Windows;
using System.Windows.Controls;

namespace SmartSoldier.Views
{
    public partial class TransportView : UserControl
    {
        public TransportView()
        {
            InitializeComponent();
            // Fahrzeugtypen hinzufügen
            cmbVehicleType.Items.Add(new ComboBoxItem { Content = "Gepanzertes Landfahrzeug" });
            cmbVehicleType.Items.Add(new ComboBoxItem { Content = "Boot" });
            cmbVehicleType.Items.Add(new ComboBoxItem { Content = "Hubschrauber" });
            cmbVehicleType.Items.Add(new ComboBoxItem { Content = "Flugzeug" });
            cmbVehicleType.SelectedIndex = 0;
        }

        private void BtnSenden_Click(object sender, RoutedEventArgs e)
        {
            var von = txtVon.Text;
            var nach = txtNach.Text;
            var fahrzeug = (cmbVehicleType.SelectedItem as ComboBoxItem)?.Content.ToString();
            var zeit = txtZeit.Text;
            MessageBox.Show($"Transportanfrage gesendet:\nVon: {von}\nNach: {nach}\nFahrzeugtyp: {fahrzeug}\nZeit: {zeit}", "Anfrage");
        }
    }
}
