namespace SmartSoldier.Models
{
    public class Person
    {
        public string Vorname { get; set; } = string.Empty;
        public string Nachname { get; set; } = string.Empty;
        public string Aufgabe { get; set; } = string.Empty;
        public string Waffe { get; set; } = string.Empty;
        public bool IstVerbunden { get; set; } = false;
    }
}
