using System.Windows.Media;

namespace SmartSoldier.Models
{
    public class DashboardPerson
    {
        public string Vorname { get; set; } = string.Empty;
        public SolidColorBrush StatusColor { get; set; } = Brushes.Red;
    }
}
