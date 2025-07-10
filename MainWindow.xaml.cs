using System.Text;
using System.Windows;
using System.Windows.Controls;
using SmartSoldier.Views;

namespace SmartSoldier;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // Standardansicht: Dashboard
        MainContent.Content = new DashboardView();
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            switch (menuItem.Name)
            {
                case "mnuDashboard":
                    MainContent.Content = new DashboardView();
                    break;
                case "mnuKarte":
                    MainContent.Content = new KarteView();
                    break;
                case "mnuPersonal":
                    MainContent.Content = new PersonellView();
                    break;
                case "mnuFeuerUnterstuetzung":
                    MainContent.Content = new FeuerUnterstuetzungView();
                    break;
                case "mnuLogistik":
                    MainContent.Content = new LogistikView();
                    break;
                case "mnuTransport":
                    MainContent.Content = new TransportView();
                    break;
                case "mnuIHA":
                    MainContent.Content = new UAVView();
                    break;
                case "mnuGesundheit":
                    MainContent.Content = new GesundheitView();
                    break;
                case "mnuKommunikation":
                    MainContent.Content = new KommunikationView();
                    break;
                case "mnuWaffenkamera":
                    MainContent.Content = new WaffenkameraView();
                    break;
            }
        }
    }
}