using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using SmartSoldier.Models;
using System.Text.Json;

namespace SmartSoldier.Views
{
    public partial class KarteView : UserControl
    {
        private double _currentLatitude = 52.5200;
        private double _currentLongitude = 13.4050;
        private double _destinationLatitude = 0;
        private double _destinationLongitude = 0;
        private bool _isMapInitialized = false;

        public KarteView()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            try
            {
                // WebView2 Environment initialisieren
                await mapWebView.EnsureCoreWebView2Async();

                // Navigation events
                mapWebView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
                mapWebView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

                // HTML Datei laden
                var htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map.html");
                if (File.Exists(htmlPath))
                {
                    mapWebView.CoreWebView2.Navigate($"file:///{htmlPath}");
                }
                else
                {
                    // Fallback: HTML inline erstellen
                    await LoadInlineHtml();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Initialisieren der Karte: {ex.Message}", "Fehler", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                _isMapInitialized = true;
                
                // Initiale Position setzen
                await SetCurrentLocationOnMap(_currentLatitude, _currentLongitude);
            }
        }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var message = JsonSerializer.Deserialize<JsonElement>(e.TryGetWebMessageAsString());
                var messageType = message.GetProperty("type").GetString();

                switch (messageType)
                {
                    case "routeCalculated":
                        HandleRouteCalculated(message);
                        break;
                    case "locationUpdated":
                        HandleLocationUpdated(message);
                        break;
                    case "mapClicked":
                        HandleMapClicked(message);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler beim Verarbeiten der WebView-Nachricht: {ex.Message}");
            }
        }

        private void HandleRouteCalculated(JsonElement message)
        {
            try
            {
                var distance = message.GetProperty("distance").GetString();
                var timeMinutes = message.GetProperty("timeMinutes").GetInt32();
                var instruction = message.GetProperty("instruction").GetString();

                // UI Updates im UI Thread
                Dispatcher.Invoke(() =>
                {
                    tbDistance.Text = $"{distance} km";
                    tbETA.Text = $"{timeMinutes} Minuten";
                    tbNavInfo.Text = $"Navigation: {instruction}";
                });

                // NavigationData für Dashboard aktualisieren
                if (double.TryParse(distance, out double dist))
                {
                    NavigationData.SetDestination(_destinationLatitude, _destinationLongitude, dist);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler beim Verarbeiten der Route: {ex.Message}");
            }
        }

        private void HandleLocationUpdated(JsonElement message)
        {
            try
            {
                _currentLatitude = message.GetProperty("latitude").GetDouble();
                _currentLongitude = message.GetProperty("longitude").GetDouble();
                
                System.Diagnostics.Debug.WriteLine($"Position aktualisiert: {_currentLatitude}, {_currentLongitude}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler beim Aktualisieren der Position: {ex.Message}");
            }
        }

        private void HandleMapClicked(JsonElement message)
        {
            try
            {
                var latitude = message.GetProperty("latitude").GetDouble();
                var longitude = message.GetProperty("longitude").GetDouble();
                
                // UI Thread - Dialog anzeigen
                Dispatcher.Invoke(() =>
                {
                    ShowLocationOptionsDialog(latitude, longitude);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler beim Verarbeiten des Karten-Klicks: {ex.Message}");
            }
        }

        private void ShowLocationOptionsDialog(double latitude, double longitude)
        {
            var dialog = new Window()
            {
                Title = "Standort-Optionen",
                Width = 350,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            var panel = new StackPanel { Margin = new Thickness(15) };

            // Header
            panel.Children.Add(new TextBlock 
            { 
                Text = "Gewählte Position:", 
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10) 
            });

            // Koordinaten anzeigen
            panel.Children.Add(new TextBlock 
            { 
                Text = $"Breitengrad: {latitude:F6}°",
                Margin = new Thickness(0, 0, 0, 5) 
            });

            panel.Children.Add(new TextBlock 
            { 
                Text = $"Längengrad: {longitude:F6}°",
                Margin = new Thickness(0, 0, 0, 15) 
            });

            // Buttons
            var buttonPanel = new StackPanel 
            { 
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };

            var btnCopyCoordinates = new Button 
            { 
                Content = "Koordinaten kopieren", 
                Width = 130,
                Height = 30,
                Margin = new Thickness(0, 0, 10, 0),
                Background = System.Windows.Media.Brushes.LightBlue
            };
            btnCopyCoordinates.Click += (s, args) => {
                var coordinates = $"{latitude:F6}, {longitude:F6}";
                System.Windows.Clipboard.SetText(coordinates);
                MessageBox.Show($"Koordinaten kopiert:\n{coordinates}", "Kopiert", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
                dialog.Close();
            };

            var btnSetNavigation = new Button 
            { 
                Content = "Navigation setzen", 
                Width = 130,
                Height = 30,
                Background = System.Windows.Media.Brushes.LightGreen
            };
            btnSetNavigation.Click += async (s, args) => {
                _destinationLatitude = latitude;
                _destinationLongitude = longitude;
                
                await mapWebView.CoreWebView2.ExecuteScriptAsync($"setClickedLocationAsDestination({latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}, {longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
                
                MessageBox.Show($"Navigation gesetzt zu:\n{latitude:F4}°N, {longitude:F4}°E\n\nRoute wird berechnet...", 
                               "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                dialog.Close();
            };

            var btnCancel = new Button 
            { 
                Content = "Abbrechen", 
                Width = 80,
                Height = 30,
                Margin = new Thickness(10, 0, 0, 0)
            };
            btnCancel.Click += (s, args) => {
                // Clicked marker entfernen
                _ = mapWebView.CoreWebView2.ExecuteScriptAsync("removeClickedMarker();");
                dialog.Close();
            };

            buttonPanel.Children.Add(btnCopyCoordinates);
            buttonPanel.Children.Add(btnSetNavigation);
            buttonPanel.Children.Add(btnCancel);
            panel.Children.Add(buttonPanel);

            dialog.Content = panel;
            dialog.ShowDialog();
        }

        private async void BtnCurrentLocation_Click(object sender, RoutedEventArgs e)
        {
            if (!_isMapInitialized) return;

            try
            {
                // GPS Position abrufen (JavaScript Funktion aufrufen)
                await mapWebView.CoreWebView2.ExecuteScriptAsync("getCurrentGPSLocation();");
                
                MessageBox.Show($"GPS-Position wird aktualisiert...\nAktuelle Position: {_currentLatitude:F4}°N, {_currentLongitude:F4}°E", 
                               "GPS Lokalisierung", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Abrufen der GPS-Position: {ex.Message}", "Fehler", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnSetDestination_Click(object sender, RoutedEventArgs e)
        {
            // Koordinat eingabe-Dialog (gleich wie vorher)
            var dialog = new Window()
            {
                Title = "Zielkoordinaten eingeben",
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };
            
            var panel = new StackPanel { Margin = new Thickness(15) };
            
            // Header
            panel.Children.Add(new TextBlock 
            { 
                Text = "Zielkoordinaten eingeben:", 
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 15) 
            });
            
            // Koordinaten Input Grid
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            
            // Latitude
            var latLabel = new TextBlock { Text = "Breitengrad:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(latLabel, 0);
            Grid.SetColumn(latLabel, 0);
            grid.Children.Add(latLabel);
            
            var latTextBox = new TextBox 
            { 
                Text = "52.5170",
                Margin = new Thickness(5, 2, 0, 2),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(latTextBox, 0);
            Grid.SetColumn(latTextBox, 1);
            grid.Children.Add(latTextBox);
            
            // Longitude
            var lonLabel = new TextBlock 
            { 
                Text = "Längengrad:", 
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };
            Grid.SetRow(lonLabel, 1);
            Grid.SetColumn(lonLabel, 0);
            grid.Children.Add(lonLabel);
            
            var lonTextBox = new TextBox 
            { 
                Text = "13.3888",
                Margin = new Thickness(5, 7, 0, 2),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(lonTextBox, 1);
            Grid.SetColumn(lonTextBox, 1);
            grid.Children.Add(lonTextBox);
            
            panel.Children.Add(grid);
            
            // Beispiel Text
            panel.Children.Add(new TextBlock 
            { 
                Text = "Beispiel: Brandenburger Tor = 52.5170, 13.3888", 
                FontStyle = FontStyles.Italic,
                Foreground = System.Windows.Media.Brushes.Gray,
                Margin = new Thickness(0, 10, 0, 0),
                FontSize = 11
            });
            
            // Buttons
            var buttonPanel = new StackPanel 
            { 
                Orientation = Orientation.Horizontal, 
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 15, 0, 0) 
            };
            
            var btnCancel = new Button 
            { 
                Content = "Abbrechen", 
                Width = 80, 
                Margin = new Thickness(0, 0, 10, 0) 
            };
            btnCancel.Click += (s, args) => dialog.DialogResult = false;
            
            var btnOK = new Button 
            { 
                Content = "Ziel setzen", 
                Width = 80 
            };
            btnOK.Click += (s, args) => dialog.DialogResult = true;
            
            buttonPanel.Children.Add(btnCancel);
            buttonPanel.Children.Add(btnOK);
            panel.Children.Add(buttonPanel);
            
            dialog.Content = panel;
            
            if (dialog.ShowDialog() == true && _isMapInitialized)
            {
                try
                {
                    var lat = double.Parse(latTextBox.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                    var lon = double.Parse(lonTextBox.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                    
                    _destinationLatitude = lat;
                    _destinationLongitude = lon;
                    
                    var coordinates = $"{lat:F4}°N, {lon:F4}°E";
                    
                    // Ziel auf der Karte setzen
                    await SetDestinationOnMap(lat, lon);
                    
                    MessageBox.Show($"Zielkoordinaten gesetzt:\n{coordinates}\n\nRoute wird berechnet...", 
                                   "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Ungültige Koordinaten eingegeben!\nBitte verwenden Sie das Format: 52.5200", 
                                   "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void BtnGetRoute_Click(object sender, RoutedEventArgs e)
        {
            if (!_isMapInitialized) return;

            try
            {
                await mapWebView.CoreWebView2.ExecuteScriptAsync("calculateRoute();");
                MessageBox.Show("Route wird neu berechnet...", "Navigation", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Berechnen der Route: {ex.Message}", "Fehler", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SetCurrentLocationOnMap(double lat, double lng)
        {
            if (!_isMapInitialized) return;
            
            var script = $"setCurrentLocation({lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}, {lng.ToString(System.Globalization.CultureInfo.InvariantCulture)});";
            await mapWebView.CoreWebView2.ExecuteScriptAsync(script);
        }

        private async Task SetDestinationOnMap(double lat, double lng)
        {
            if (!_isMapInitialized) return;
            
            var script = $"setDestination({lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}, {lng.ToString(System.Globalization.CultureInfo.InvariantCulture)});";
            await mapWebView.CoreWebView2.ExecuteScriptAsync(script);
        }

        private async Task LoadInlineHtml()
        {
            // Fallback HTML falls Datei nicht gefunden wird
            var htmlContent = @"
            <!DOCTYPE html>
            <html>
            <head>
                <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' />
                <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
            </head>
            <body>
                <div id='map' style='height: 100vh;'></div>
                <script>
                    var map = L.map('map').setView([52.5200, 13.4050], 13);
                    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(map);
                </script>
            </body>
            </html>";

            await Task.Run(() => mapWebView.CoreWebView2.NavigateToString(htmlContent));
        }
    }
}
