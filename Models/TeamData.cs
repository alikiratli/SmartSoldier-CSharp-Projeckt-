using System.Collections.ObjectModel;

namespace SmartSoldier.Models
{
    public static class TeamData
    {
        private static ObservableCollection<Person>? _team;

        public static ObservableCollection<Person> Team
        {
            get
            {
                if (_team == null)
                {
                    _team = new ObservableCollection<Person>
                    {
                        new Person { Vorname = "Max", Nachname = "Mustermann", Aufgabe = "Späher", Waffe = "Gewehr", IstVerbunden = true },
                        new Person { Vorname = "Anna", Nachname = "Schmidt", Aufgabe = "Sanitäterin", Waffe = "Pistole", IstVerbunden = false },
                        new Person { Vorname = "Peter", Nachname = "Müller", Aufgabe = "Truppführer", Waffe = "Maschinengewehr", IstVerbunden = true }
                    };
                }
                return _team;
            }
        }
    }
}
