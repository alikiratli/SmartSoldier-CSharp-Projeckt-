namespace SmartSoldier.Models
{
    public class CriticalTarget
    {
        public int SiraNo { get; set; }
        public required string Zielname { get; set; }
        public required string Gefahrentyp { get; set; }
        public double Distanz { get; set; }
        public double Winkel { get; set; }
        public required string Koordinaten { get; set; }
    }
}
