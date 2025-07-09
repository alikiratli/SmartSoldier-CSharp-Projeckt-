using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using SmartSoldier.Models;
using System.Diagnostics;
using System.IO;

namespace SmartSoldier.Views
{
    public partial class PersonellView : UserControl
    {
        private readonly ObservableCollection<Person> _team;

        public PersonellView()
        {
            InitializeComponent();

            // Beispiel-Daten für Anzeige
            _team = new ObservableCollection<Person>
            {
                new Person { Vorname = "Max", Nachname = "Mustermann", Aufgabe = "Späher", Waffe = "Gewehr", IstVerbunden = true },
                new Person { Vorname = "Anna", Nachname = "Schmidt", Aufgabe = "Sanitäterin", Waffe = "Pistole", IstVerbunden = false },
                new Person { Vorname = "Peter", Nachname = "Müller", Aufgabe = "Truppführer", Waffe = "Maschinengewehr", IstVerbunden = true }
            };
            dgPersonell.ItemsSource = _team;
        }

        private void BtnAddPerson_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtVorname.Text) ||
                string.IsNullOrWhiteSpace(txtNachname.Text) ||
                string.IsNullOrWhiteSpace(txtAufgabe.Text) ||
                string.IsNullOrWhiteSpace(txtWaffe.Text))
            {
                MessageBox.Show("Bitte alle Felder ausfüllen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var neu = new Person
            {
                Vorname = txtVorname.Text,
                Nachname = txtNachname.Text,
                Aufgabe = txtAufgabe.Text,
                Waffe = txtWaffe.Text,
                IstVerbunden = false
            };
            _team.Add(neu);
            txtVorname.Clear();
            txtNachname.Clear();
            txtAufgabe.Clear();
            txtWaffe.Clear();
        }

        private void ShowTouchKeyboard(object sender, RoutedEventArgs e)
        {
            try
            {
                var tabTip = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles),
                                          "Microsoft Shared", "ink", "TabTip.exe");
                Process.Start(new ProcessStartInfo(tabTip) { UseShellExecute = true });
            }
            catch { }
        }
    }
}
