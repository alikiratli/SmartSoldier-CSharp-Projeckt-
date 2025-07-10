using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartSoldier.Views
{
    public partial class WaffenkameraView : UserControl
    {
        private bool _laserOn = false;
        private Random _random = new Random();

        public WaffenkameraView()
        {
            InitializeComponent();
        }

        private void BtnLaserToggle_Click(object sender, RoutedEventArgs e)
        {
            _laserOn = !_laserOn;
            if (FindName("ellipseLaserStatus") is System.Windows.Shapes.Ellipse ellipse)
            {
                ellipse.Fill = _laserOn ? Brushes.LimeGreen : Brushes.Red;
            }
            if (FindName("btnLaserToggle") is Button btn)
            {
                btn.Content = _laserOn ? "Laser ausschalten" : "Laser einschalten";
            }
        }

        private void BtnMeasureDistance_Click(object sender, RoutedEventArgs e)
        {
            // Simuliere Entfernungsmessung
            int distance = _random.Next(50, 500);
            if (FindName("tbTargetDistance") is TextBlock tb)
            {
                tb.Text = $"{distance}m";
                tb.Foreground = distance < 100 ? Brushes.Red : 
                               distance < 200 ? Brushes.Orange : Brushes.Green;
            }
            MessageBox.Show($"Zielentfernung gemessen: {distance}m", "Entfernungsmessung");
        }
    }
}
