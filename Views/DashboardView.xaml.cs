using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmartSoldier.Models;
using System.Linq;
using System.Collections.Specialized;

namespace SmartSoldier.Views
{
    public partial class DashboardView : UserControl
    {
        private System.Timers.Timer? _dataUpdateTimer;
        private System.Timers.Timer? _annaConnectionTimer;
        private Random _random = new Random();
        private ObservableCollection<DashboardPerson> _dashboardPersons = new();
        
        public DashboardView()
        {
            InitializeComponent();
            InitializeDashboardData();
            StartDataUpdateTimer();
            StartAnnaConnectionTimer();
            
            // Team verilerindeki değişiklikleri dinle
            TeamData.Team.CollectionChanged += Team_CollectionChanged;
            
            // Navigation değişikliklerini dinle
            NavigationData.DestinationChanged += NavigationData_DestinationChanged;
        }

        private void InitializeDashboardData()
        {
            // PersonellView ile aynı veri kaynağını kullan
            _dashboardPersons.Clear();
            
            // TeamData'dan verileri al ve DashboardPerson formatına çevir
            UpdatePersonList();

            if (FindName("dgDashboardPersonal") is DataGrid dg)
            {
                dg.ItemsSource = _dashboardPersons;
            }
            
            // Kompass-Pfeil winkelbasiert drehen (z.B. 45° nach NO)
            if (FindName("compassArrow") is Polygon arrow && arrow.RenderTransform is RotateTransform rt)
            {
                rt.Angle = 45; // hier tatsächlichen Kompasswinkel setzen
            }
            
            // Wetter-Symbol setzen (Beispiel: Klar, Regen oder Bewölkt)
            string wetter = "Klar"; // TODO: dynamisch aus Datenquelle
            string icon = wetter.Contains("Regen") ? "rain" : wetter.Contains("Klar") ? "sun" : "cloud";
            // Pack URI als relativer Pfad
            var weatherUri = new Uri($"/SmartSoldier;component/Images/{icon}.png", UriKind.Relative);
            if (FindName("imgWeatherIcon") is System.Windows.Controls.Image weatherImg)
            {
                try
                {
                    weatherImg.Source = new BitmapImage(weatherUri);
                }
                catch (Exception ex)
                {
                    // Fallback: kein Icon verfügbar
                    System.Diagnostics.Debug.WriteLine($"Fehler beim Laden des Wetter-Icons: {ex.Message}");
                    weatherImg.Source = null;
                }
            }
            if (FindName("tbWeather") is System.Windows.Controls.TextBlock weatherTxt)
            {
                weatherTxt.Text = $"Wetter: {wetter}, 25°C";
            }
            
            // Gesundheitsstatus initial setzen
            UpdateHealthStatus();
        }

        private void StartDataUpdateTimer()
        {
            _dataUpdateTimer = new System.Timers.Timer(5000); // Update alle 5 Sekunden
            _dataUpdateTimer.Elapsed += OnDataUpdateTimer;
            _dataUpdateTimer.Start();
        }

        private void StartAnnaConnectionTimer()
        {
            // Anna verbindet sich nach 1 Minute (60000 ms) - Timer
            _annaConnectionTimer = new System.Timers.Timer(60000); // 1 Minute
            _annaConnectionTimer.Elapsed += OnAnnaConnectionTimer;
            _annaConnectionTimer.AutoReset = false; // Nur einmal ausführen
            _annaConnectionTimer.Start();
        }

        private void OnAnnaConnectionTimer(object? sender, ElapsedEventArgs e)
        {
            // Anna in verbundenen Zustand setzen
            Dispatcher.Invoke(() =>
            {
                var anna = TeamData.Team.FirstOrDefault(p => p.Vorname == "Anna");
                if (anna != null)
                {
                    anna.IstVerbunden = true;
                    UpdatePersonList(); // Dashboard aktualisieren
                }
            });
        }

        private void OnDataUpdateTimer(object? sender, ElapsedEventArgs e)
        {
            // Dispatcher für UI-Updates
            Dispatcher.Invoke(() =>
            {
                UpdateHealthStatus();
                UpdateNavigationData();
            });
        }

        private void UpdateHealthStatus()
        {
            // Simuliere schwankende Körpertemperatur
            double bodyTemp = 36.5 + (_random.NextDouble() * 0.8); // 36.5 - 37.3°C
            if (FindName("tbBodyTemp") is TextBlock tbTemp)
            {
                tbTemp.Text = $"Körpertemperatur: {bodyTemp:F1}°C";
            }

            // Gesundheitsstatus basierend auf Temperatur
            string healthStatus;
            Brush statusColor;
            
            if (bodyTemp < 36.0)
            {
                healthStatus = "Unterkühlung";
                statusColor = Brushes.Blue;
            }
            else if (bodyTemp > 37.5)
            {
                healthStatus = "Fieber";
                statusColor = Brushes.Red;
            }
            else if (bodyTemp > 37.0)
            {
                healthStatus = "Erhöht";
                statusColor = Brushes.Orange;
            }
            else
            {
                healthStatus = "Optimal";
                statusColor = Brushes.Green;
            }

            if (FindName("tbHealthStatus") is TextBlock tbHealth)
            {
                tbHealth.Text = $"Gesundheitsstatus: {healthStatus}";
                tbHealth.Foreground = statusColor;
            }
        }

        private void UpdateNavigationData()
        {
            // NavigationData - aktuelle Informationen abrufen
            double distanceWalked = 2.8 + (_random.NextDouble() * 0.5); // 2.8 - 3.3 km
            double distanceToTarget = NavigationData.CurrentDistance;

            if (FindName("tbDistanceWalked") is TextBlock tbWalked)
            {
                tbWalked.Text = $"Gelaufene Strecke: {distanceWalked:F1} km";
            }

            if (FindName("tbDistanceToTarget") is TextBlock tbTarget)
            {
                tbTarget.Text = $"Distanz zum Ziel: {distanceToTarget:F1} km";
            }

            // Navigationstext NavigationData'dan al
            string navText;
            if (string.IsNullOrEmpty(NavigationData.CurrentDestination))
            {
                navText = "Kein Ziel gesetzt";
            }
            else if (distanceToTarget < 0.2)
            {
                navText = "Ziel erreicht!";
            }
            else if (distanceToTarget < 0.5)
            {
                navText = "Ziel in Sichtweite - geradeaus";
            }
            else
            {
                navText = NavigationData.CurrentRoute;
            }

            if (FindName("tbNavigationText") is TextBlock tbNav)
            {
                tbNav.Text = $"Navigation: {navText}";
            }
        }

        // Cleanup
        public void Dispose()
        {
            TeamData.Team.CollectionChanged -= Team_CollectionChanged;
            NavigationData.DestinationChanged -= NavigationData_DestinationChanged;
            _dataUpdateTimer?.Stop();
            _dataUpdateTimer?.Dispose();
            _annaConnectionTimer?.Stop();
            _annaConnectionTimer?.Dispose();
        }

        private void NavigationData_DestinationChanged(object? sender, EventArgs e)
        {
            // UI thread - Navigation-Informationen aktualisieren
            Dispatcher.Invoke(() =>
            {
                UpdateNavigationData();
            });
        }

        private void UpdatePersonList()
        {
            _dashboardPersons.Clear();
            foreach (var person in TeamData.Team)
            {
                _dashboardPersons.Add(new DashboardPerson 
                { 
                    Vorname = person.Vorname,
                    StatusColor = person.IstVerbunden ? Brushes.LimeGreen : Brushes.Red
                });
            }
        }

        private void Team_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // UI thread - aktualisieren
            Dispatcher.Invoke(() =>
            {
                UpdatePersonList();
            });
        }
    }
}
