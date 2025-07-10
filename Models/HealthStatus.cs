namespace SmartSoldier.Models
{
    public class HealthStatus
    {
        public string Vorname { get; set; } = string.Empty;
        public string Nachname { get; set; } = string.Empty;
        public int Puls { get; set; }
        public double Temperatur { get; set; }
        public string Location { get; set; } = string.Empty;

        public string Status
        {
            get
            {
                // Kritisch, wenn Puls außerhalb 60-100 oder Temperatur außerhalb 36.1-37.2
                bool pulsKritisch = Puls < 60 || Puls > 100;
                bool tempKritisch = Temperatur < 36.1 || Temperatur > 37.2;
                return (pulsKritisch || tempKritisch) ? "kritisch" : "gesund";
            }
        }
    }
}
