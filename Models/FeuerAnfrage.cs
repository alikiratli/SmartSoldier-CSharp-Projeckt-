using System;

namespace SmartSoldier.Models
{
    public class FeuerAnfrage
    {
        public int Id { get; set; }
        public string Koordinaten { get; set; } = string.Empty;
        public string Feuerart { get; set; } = string.Empty;
        public string Dauer { get; set; } = string.Empty;
        public string Zeit { get; set; } = string.Empty;
        public DateTime Zeitstempel { get; set; }
        public string Status { get; set; } = "Gesendet";
    }
}
