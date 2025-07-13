using System;

namespace SmartSoldier.Models
{
    public class TransportAnfrage
    {
        public int Id { get; set; }
        public string Von { get; set; } = "";
        public string Nach { get; set; } = "";
        public string Fahrzeugtyp { get; set; } = "";
        public string Zeit { get; set; } = "";
        public DateTime Zeitstempel { get; set; }
        public string Status { get; set; } = "Angefordert";
    }
}
