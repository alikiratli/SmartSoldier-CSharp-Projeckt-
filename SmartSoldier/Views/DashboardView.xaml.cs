using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SmartSoldier.Models;

namespace SmartSoldier.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            // Beispiel-Daten für Personalverbindung
            var personList = new ObservableCollection<DashboardPerson>
            {
                new DashboardPerson { Vorname = "Müller", StatusColor = Brushes.LimeGreen },
                new DashboardPerson { Vorname = "Schmidt", StatusColor = Brushes.Red },
                new DashboardPerson { Vorname = "Meier",   StatusColor = Brushes.LimeGreen }
            };
            dgDashboardPersonal.ItemsSource = personList;
            // Kompass-Pfeil winkelbasiert drehen (z.B. 45° nach NO)
            if (FindName("compassArrow") is Polygon arrow && arrow.RenderTransform is RotateTransform rt)
            {
                rt.Angle = 45; // hier tatsächlichen Kompasswinkel setzen
            }
        }
    }
}
