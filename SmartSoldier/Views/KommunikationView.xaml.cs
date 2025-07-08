using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SmartSoldier.Models;

namespace SmartSoldier.Views
{
    public partial class KommunikationView : UserControl
    {
        public KommunikationView()
        {
            InitializeComponent();
            // Beispielkontakte
            var contacts = new ObservableCollection<Person>
            {
                new Person { Vorname = "Max", Nachname = "Mustermann" },
                new Person { Vorname = "Anna", Nachname = "Schmidt" }
            };
            dgKommunikation.ItemsSource = contacts;
        }

        private void BtnVideoCall_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Person p)
                MessageBox.Show($"Starte Videoanruf mit {p.Vorname} {p.Nachname}.", "Videoanruf");
        }

        private void BtnAudioCall_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Person p)
                MessageBox.Show($"Starte Sprachanruf mit {p.Vorname} {p.Nachname}.", "Sprachanruf");
        }

        private void BtnMessage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Person p)
                MessageBox.Show($"Sende Nachricht an {p.Vorname} {p.Nachname}.", "Nachricht");
        }
    }
}
