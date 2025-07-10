using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace SmartSoldier.Views
{
    public partial class KarteView : UserControl
    {
        private Random random = new Random();

        public KarteView()
        {
            InitializeComponent();
            // Simuliere GPS-Update alle 3 Sekunden
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += UpdateNavigation;
            timer.Start();
        }

        private void BtnCurrentLocation_Click(object sender, RoutedEventArgs e)
        {
            // Simuliere GPS-Lokalisierung
            MessageBox.Show("GPS-Position aktualisiert: 52.5200° N, 13.4050° E", "GPS Lokalisierung");
            
            // Animation für Position-Update
            if (FindName("currentPos") is Ellipse currentPos)
            {
                var storyboard = new Storyboard();
                var moveX = new DoubleAnimation();
                moveX.From = Canvas.GetLeft(currentPos);
                moveX.To = random.Next(50, 350);
                moveX.Duration = TimeSpan.FromSeconds(1);
                Storyboard.SetTarget(moveX, currentPos);
                Storyboard.SetTargetProperty(moveX, new PropertyPath(Canvas.LeftProperty));
                
                var moveY = new DoubleAnimation();
                moveY.From = Canvas.GetTop(currentPos);
                moveY.To = random.Next(50, 250);
                moveY.Duration = TimeSpan.FromSeconds(1);
                Storyboard.SetTarget(moveY, currentPos);
                Storyboard.SetTargetProperty(moveY, new PropertyPath(Canvas.TopProperty));
                
                storyboard.Children.Add(moveX);
                storyboard.Children.Add(moveY);
                storyboard.Begin();
            }
        }

        private void BtnSetDestination_Click(object sender, RoutedEventArgs e)
        {
            // Einfache Eingabe-Dialog
            var dialog = new Window()
            {
                Title = "Ziel eingeben",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            
            var panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock { Text = "Ziel eingeben:", Margin = new Thickness(0, 0, 0, 10) });
            
            var textBox = new TextBox { Text = "Hauptquartier" };
            panel.Children.Add(textBox);
            
            var btnOK = new Button { Content = "OK", Margin = new Thickness(0, 10, 0, 0), Width = 60 };
            btnOK.Click += (s, args) => dialog.DialogResult = true;
            panel.Children.Add(btnOK);
            
            dialog.Content = panel;
            
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(textBox.Text))
            {
                MessageBox.Show($"Route zu '{textBox.Text}' wird berechnet...", "Navigation");
                
                // Animiere Ziel-Position
                if (FindName("destination") is Ellipse dest)
                {
                    var destX = random.Next(300, 380);
                    var destY = random.Next(180, 220);
                    Canvas.SetLeft(dest, destX);
                    Canvas.SetTop(dest, destY);
                }
                
                // Update Navigation Info
                if (FindName("tbNavInfo") is TextBlock tbNavInfo)
                    tbNavInfo.Text = $"Route zu {textBox.Text} - Folgen Sie der grünen Linie";
                if (FindName("tbDistance") is TextBlock tbDistance)
                    tbDistance.Text = $"{random.Next(200, 800)}m";
                if (FindName("tbETA") is TextBlock tbETA)
                    tbETA.Text = $"{random.Next(3, 15)} Minuten";
            }
        }

        private void UpdateNavigation(object? sender, EventArgs e)
        {
            // Simuliere Navigations-Updates
            var directions = new[]
            {
                "In 200m rechts abbiegen",
                "Geradeaus für 300m",
                "In 150m links abbiegen auf Hauptstraße",
                "Ziel in 100m erreicht",
                "Folgen Sie der Route"
            };
            
            if (FindName("tbNavInfo") is TextBlock tbNavInfo)
                tbNavInfo.Text = $"Navigation: {directions[random.Next(directions.Length)]}";
            
            if (FindName("tbDistance") is TextBlock tbDistance)
            {
                var currentDist = int.Parse(tbDistance.Text.Replace("m", ""));
                tbDistance.Text = $"{Math.Max(50, currentDist - random.Next(10, 30))}m";
            }
        }
    }
}
