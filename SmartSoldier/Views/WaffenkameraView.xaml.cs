using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartSoldier.Views
{
    public partial class WaffenkameraView : UserControl
    {
        private bool _laserOn = false;

        public WaffenkameraView()
        {
            InitializeComponent();
        }

        private void BtnLaserToggle_Click(object sender, RoutedEventArgs e)
        {
            _laserOn = !_laserOn;
            ellipseLaserStatus.Fill = _laserOn ? Brushes.LimeGreen : Brushes.Red;
            btnLaserToggle.Content = _laserOn ? "Laser ausschalten" : "Laser einschalten";
        }
    }
}
