using System.Windows;
using System.Windows.Controls;

namespace SmartSoldier.Views
{
    public partial class FeuerUnterstuetzungView : UserControl
    {
        public FeuerUnterstuetzungView()
        {
            InitializeComponent();
        }

        private void BtnSenden_Click(object sender, RoutedEventArgs e)
        {
            var koord = txtKoordinaten.Text;
            var feuerArt = (cmbFeuerart.SelectedItem as ComboBoxItem)?.Content.ToString();
            var dauer = txtDauer.Text;
            var zeit = txtZeit.Text;
            MessageBox.Show($"Feuerunterstützungsanfrage:\nKoordinaten: {koord}\nFeuerart: {feuerArt}\nDauer: {dauer} Min\nZeit: {zeit}", "Anfrage");
        }
    }
}
