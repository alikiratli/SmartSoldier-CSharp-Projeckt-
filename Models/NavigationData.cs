using System;
using System.ComponentModel;

namespace SmartSoldier.Models
{
    public static class NavigationData
    {
        public static event EventHandler? DestinationChanged;
        
        private static string _currentDestination = "";
        private static double _currentDistance = 1.2;
        private static string _currentRoute = "Kein Ziel gesetzt";
        
        public static string CurrentDestination
        {
            get => _currentDestination;
            set
            {
                _currentDestination = value;
                DestinationChanged?.Invoke(null, EventArgs.Empty);
            }
        }
        
        public static double CurrentDistance
        {
            get => _currentDistance;
            set => _currentDistance = value;
        }
        
        public static string CurrentRoute
        {
            get => _currentRoute;
            set => _currentRoute = value;
        }
        
        public static void SetDestination(double latitude, double longitude, double distanceKm)
        {
            CurrentDestination = $"{latitude:F4}°N, {longitude:F4}°E";
            CurrentDistance = distanceKm;
            CurrentRoute = $"Route zu {CurrentDestination}";
            DestinationChanged?.Invoke(null, EventArgs.Empty);
        }
        
        public static void ClearDestination()
        {
            CurrentDestination = "";
            CurrentDistance = 1.2;
            CurrentRoute = "Kein Ziel gesetzt";
            DestinationChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
