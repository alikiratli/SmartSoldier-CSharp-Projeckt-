namespace SmartSoldier.Models
{
    public class MaterialInfo
    {
        public string MaterialName { get; set; } = string.Empty;
        public int Anzahl { get; set; }
        public int Bedarf { get; set; }
        public int Fehlend { get; set; }
    }
}
