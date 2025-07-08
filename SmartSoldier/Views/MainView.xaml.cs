using System.Windows;
using System.Windows.Controls;

namespace SmartSoldier.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                switch (menuItem.Name)
                {
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
                }
            }
        }
    }

}