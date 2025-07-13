using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SmartSoldier.Views
{
    public partial class WaffenkameraView : UserControl
    {
        private bool _laserOn = false;
        private bool _isTargetLocked = false;
        private bool _isFireReady = false;
        private double _currentZoom = 2.5;
        private Random _random = new Random();

        public WaffenkameraView()
        {
            InitializeComponent();
            InitializeTargetingSystem();
        }

        private void InitializeTargetingSystem()
        {
            // Canvas-Größe geändert - Zielsystem aktualisieren
            canvasTargeting.SizeChanged += (s, e) => UpdateTargetingSystem();
            
            // Beim ersten Laden kurz warten
            Loaded += (s, e) => 
            {
                Dispatcher.BeginInvoke(new Action(() => UpdateTargetingSystem()));
            };
        }

        private void UpdateTargetingSystem()
        {
            if (canvasTargeting.ActualWidth == 0 || canvasTargeting.ActualHeight == 0)
                return;

            double centerX = canvasTargeting.ActualWidth / 2;
            double centerY = canvasTargeting.ActualHeight / 2;

            // Zentral-Kreuzvisier
            crosshairH.X1 = centerX - 25;
            crosshairH.Y1 = centerY;
            crosshairH.X2 = centerX + 25;
            crosshairH.Y2 = centerY;

            crosshairV.X1 = centerX;
            crosshairV.Y1 = centerY - 25;
            crosshairV.X2 = centerX;
            crosshairV.Y2 = centerY + 25;

            // Ecken-Visierlinien
            double cornerSize = 40;
            double margin = 60;

            // Links oben
            cornerTL1.X1 = margin;
            cornerTL1.Y1 = margin;
            cornerTL1.X2 = margin + cornerSize;
            cornerTL1.Y2 = margin;

            cornerTL2.X1 = margin;
            cornerTL2.Y1 = margin;
            cornerTL2.X2 = margin;
            cornerTL2.Y2 = margin + cornerSize;

            // Rechts oben
            cornerTR1.X1 = canvasTargeting.ActualWidth - margin - cornerSize;
            cornerTR1.Y1 = margin;
            cornerTR1.X2 = canvasTargeting.ActualWidth - margin;
            cornerTR1.Y2 = margin;

            cornerTR2.X1 = canvasTargeting.ActualWidth - margin;
            cornerTR2.Y1 = margin;
            cornerTR2.X2 = canvasTargeting.ActualWidth - margin;
            cornerTR2.Y2 = margin + cornerSize;

            // Links unten
            cornerBL1.X1 = margin;
            cornerBL1.Y1 = canvasTargeting.ActualHeight - margin;
            cornerBL1.X2 = margin + cornerSize;
            cornerBL1.Y2 = canvasTargeting.ActualHeight - margin;

            cornerBL2.X1 = margin;
            cornerBL2.Y1 = canvasTargeting.ActualHeight - margin - cornerSize;
            cornerBL2.X2 = margin;
            cornerBL2.Y2 = canvasTargeting.ActualHeight - margin;

            // Rechts unten
            cornerBR1.X1 = canvasTargeting.ActualWidth - margin - cornerSize;
            cornerBR1.Y1 = canvasTargeting.ActualHeight - margin;
            cornerBR1.X2 = canvasTargeting.ActualWidth - margin;
            cornerBR1.Y2 = canvasTargeting.ActualHeight - margin;

            cornerBR2.X1 = canvasTargeting.ActualWidth - margin;
            cornerBR2.Y1 = canvasTargeting.ActualHeight - margin - cornerSize;
            cornerBR2.X2 = canvasTargeting.ActualWidth - margin;
            cornerBR2.Y2 = canvasTargeting.ActualHeight - margin;

            // Entfernungskreise
            rangeCircle1.Width = 120;
            rangeCircle1.Height = 120;
            Canvas.SetLeft(rangeCircle1, centerX - 60);
            Canvas.SetTop(rangeCircle1, centerY - 60);

            rangeCircle2.Width = 240;
            rangeCircle2.Height = 240;
            Canvas.SetLeft(rangeCircle2, centerX - 120);
            Canvas.SetTop(rangeCircle2, centerY - 120);

            // Laser-Visierpunkt
            Canvas.SetLeft(laserDot, centerX - 4);
            Canvas.SetTop(laserDot, centerY - 4);

            // Ziel-Erfassungsrahmen
            Canvas.SetLeft(targetLockBox, centerX - 50);
            Canvas.SetTop(targetLockBox, centerY - 40);
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
            
            // Laser-Visierpunkt anzeigen/verbergen
            laserDot.Visibility = _laserOn ? Visibility.Visible : Visibility.Hidden;
            
            // Laser-Status aktualisieren
            tbLaserStatus.Text = _laserOn ? "LASER: EIN" : "LASER: AUS";
            tbLaserStatus.Foreground = _laserOn ? Brushes.Red : Brushes.Gray;
            
            // Ziel-Erfassung
            if (_laserOn)
            {
                _isTargetLocked = true;
                targetLockBox.Visibility = Visibility.Visible;
                _isFireReady = true;
                tbFireReady.Visibility = Visibility.Visible;
                
                // Windinformationen aktualisieren
                double windSpeed = 0.5 + (_random.NextDouble() * 3.0);
                tbWindage.Text = $"WIND: {windSpeed:F1} m/s";
            }
            else
            {
                _isTargetLocked = false;
                targetLockBox.Visibility = Visibility.Hidden;
                _isFireReady = false;
                tbFireReady.Visibility = Visibility.Hidden;
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
            
            // Entfernungsinformation im Zielsystem anzeigen
            tbTargetRange.Text = $"ENTFERNUNG: {distance}m";
            tbTargetRange.Foreground = distance < 100 ? Brushes.Red : 
                                     distance < 200 ? Brushes.Orange : Brushes.Lime;
            
            // Zoom-Stufe entsprechend der Entfernung anpassen
            if (distance < 100)
            {
                _currentZoom = 4.0;
            }
            else if (distance < 200)
            {
                _currentZoom = 3.0;
            }
            else
            {
                _currentZoom = 2.5;
            }
            
            tbZoomLevel.Text = $"ZOOM: {_currentZoom:F1}x";
            
            MessageBox.Show($"🎯 Zielentfernung gemessen: {distance}m\n\n" +
                           $"Zoom automatisch angepasst: {_currentZoom:F1}x\n" +
                           $"Laser: {(_laserOn ? "AKTIV" : "INAKTIV")}\n" +
                           $"Feuerbereit: {(_isFireReady ? "JA" : "NEIN")}", 
                           "Entfernungsmessung");
        }
    }
}
