using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SmartSoldier.Models;

namespace SmartSoldier.Views
{
    public partial class UAVView : UserControl
    {
        private bool _isTargetLocked = false;
        private double _currentZoom = 1.0;
        private Random _random = new Random();

        public UAVView()
        {
            InitializeComponent();
            InitializeTargetingSystem();
            InitializeTargetData();
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
            crosshairH.X1 = centerX - 20;
            crosshairH.Y1 = centerY;
            crosshairH.X2 = centerX + 20;
            crosshairH.Y2 = centerY;

            crosshairV.X1 = centerX;
            crosshairV.Y1 = centerY - 20;
            crosshairV.X2 = centerX;
            crosshairV.Y2 = centerY + 20;

            // Ecken-Visierlinien
            double cornerSize = 30;
            double margin = 50;

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
            rangeCircle1.Width = 100;
            rangeCircle1.Height = 100;
            Canvas.SetLeft(rangeCircle1, centerX - 50);
            Canvas.SetTop(rangeCircle1, centerY - 50);

            rangeCircle2.Width = 200;
            rangeCircle2.Height = 200;
            Canvas.SetLeft(rangeCircle2, centerX - 100);
            Canvas.SetTop(rangeCircle2, centerY - 100);

            // Ziel-Erfassungsrahmen
            Canvas.SetLeft(targetLockBox, centerX - 40);
            Canvas.SetTop(targetLockBox, centerY - 30);
        }

        private void InitializeTargetData()
        {
            // Beispiel-Daten für erkannte kritische Ziele
            var targets = new ObservableCollection<CriticalTarget>
            {
                new CriticalTarget { SiraNo = 1, Zielname = "Ziel Alpha", Gefahrentyp = "Infanterie", Distanz = 1200, Winkel = 45, Koordinaten = "52.5200°N,13.4050°E" },
                new CriticalTarget { SiraNo = 2, Zielname = "Ziel Bravo", Gefahrentyp = "Fahrzeug", Distanz = 2500, Winkel = 90, Koordinaten = "52.5200°N,13.4100°E" },
                new CriticalTarget { SiraNo = 3, Zielname = "Ziel Charlie", Gefahrentyp = "Geschütz", Distanz = 1800, Winkel = 135, Koordinaten = "52.5250°N,13.4050°E" }
            };
            dgTargets.ItemsSource = targets;
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UAV bewegt sich nach oben", "Steuerung");
            UpdateZoom(0.1);
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UAV bewegt sich nach unten", "Steuerung");
            UpdateZoom(-0.1);
        }

        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UAV bewegt sich nach links", "Steuerung");
            ToggleTargetLock();
        }

        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UAV bewegt sich nach rechts", "Steuerung");
            ToggleTargetLock();
        }

        private void BtnFire_Click(object sender, RoutedEventArgs e)
        {
            if (_isTargetLocked)
            {
                MessageBox.Show("🎯 UAV feuert! Ziel getroffen!\n\n" +
                                "Treffer bestätigt - Ziel neutralisiert!", 
                                "Feuersystem", MessageBoxButton.OK, MessageBoxImage.Warning);
                _isTargetLocked = false;
                targetLockBox.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("⚠️ Kein Ziel erfasst!\n\nBitte zuerst ein Ziel anvisieren.", 
                                "Feuersystem", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdateZoom(double delta)
        {
            _currentZoom = Math.Max(0.5, Math.Min(5.0, _currentZoom + delta));
            tbZoomLevel.Text = $"ZOOM: {_currentZoom:F1}x";
            
            // Zoom-Stufe entsprechend der Zielsystem anpassen
            double zoomFactor = _currentZoom / 2.0;
            crosshairH.StrokeThickness = 2 * zoomFactor;
            crosshairV.StrokeThickness = 2 * zoomFactor;
        }

        private void ToggleTargetLock()
        {
            _isTargetLocked = !_isTargetLocked;
            targetLockBox.Visibility = _isTargetLocked ? Visibility.Visible : Visibility.Hidden;
            
            if (_isTargetLocked)
            {
                // Ziel erfasst - Koordinaten aktualisieren
                double lat = 41.123 + (_random.NextDouble() - 0.5) * 0.01;
                double lon = 29.456 + (_random.NextDouble() - 0.5) * 0.01;
                tbCoordinates.Text = $"N {lat:F3} E {lon:F3}";
                
                double elevation = 1250 + (_random.Next(-50, 50));
                tbElevation.Text = $"HÖHE: {elevation}m";
            }
        }
    }
}
