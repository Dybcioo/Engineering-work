using ConnectionSQL;
using DataModel;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;


namespace SuperKurier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string BackgroundOption { get; set; }
        public string ForegroundOption { get; set; }
        public string InputOption { get; set; }
        public Color ColorBtn { get; set; }
        public string TestConnection { get; set; }
        public string ColorConnection { get; set; }
        public Connection Conn { get; set; }
        private MapPolyline polyline = null;
        private Location location = null;
        private bool regionSquare = false;
        private bool activationFunction = true;
        private CompanyEntities companyEntities = new CompanyEntities();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            BlackAndWhiteLayout();
            GetDBSettings();
        }

        private void MyMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (activationFunction)
            {
                e.Handled = true;
                MyMap.CheckingPushpin(e);
            } 
        }
        private void ContextMenu_RightClick(object sender, MouseButtonEventArgs e)
        {
            if(activationFunction)
            {
                ContextMenu context = new ContextMenu();
                context.IsOpen = true;
                var createRegions = new MenuItem() { Header = "Dodaj nowy region" };
                var connectPushPins = new MenuItem() { Header = "Połącz pinezki" };
                var clear = new MenuItem() { Header = "Wyczyść trase" };
                clear.Click += ClearPolyline_Click;
                connectPushPins.Click += ConnectPushPins_Click;
                createRegions.Click += CreateRegions_Click;
                context.Items.Add(createRegions);
                context.Items.Add(connectPushPins);
                context.Items.Add(clear);
            }
        }

        private void BtnClearRegion_Click(object sender, RoutedEventArgs e)
        {
            activationFunction = true;
            MyMap.ClearAllMap();
            btnClearRegion.Visibility = Visibility.Hidden;
            btnAddRegion.Visibility = Visibility.Hidden;
        }

        private void CreateRegions_Click(object sender, RoutedEventArgs e)
        {
            polyline = new MapPolyline();
            polyline.Stroke = new SolidColorBrush(Colors.Blue);
            polyline.StrokeThickness = 3;
            polyline.Opacity = 0.7; 
            location = MyMap.ViewportPointToLocation(Mouse.GetPosition(MyMap));
            LocationCollection locations = new LocationCollection();
            locations.Add(location);
            locations.Add(new Location());
            locations.Add(new Location());
            locations.Add(new Location());
            locations.Add(new Location());
            polyline.Locations = locations;
            MyMap.Children.Add(polyline);
            regionSquare = true;
            activationFunction = false;
        }
        private async void MyMap_MouseMove(object sender, MouseEventArgs e)
        {
            Location location2 = new Location();
            if (polyline != null && regionSquare)
            {
                var position1 = Mouse.GetPosition(MyMap);
                Location location1 = MyMap.ViewportPointToLocation(position1);
                polyline.Locations[1].Latitude = location1.Latitude;
                polyline.Locations[1].Longitude = location.Longitude;
                polyline.Locations[2] = location1;
                polyline.Locations[3].Latitude = location.Latitude;
                polyline.Locations[3].Longitude = location1.Longitude;
                polyline.Locations[4] = location;
                if(e.LeftButton == MouseButtonState.Pressed)
                {
                    location2 = MyMap.Center;
                    while(e.LeftButton == MouseButtonState.Pressed) { await Task.Delay(25); }
                }
                location1 = MyMap.Center;
                if (location2 == location1)
                {
                    regionSquare = false;
                    polyline.Stroke = new SolidColorBrush(Colors.Green);
                    MyMap.ShowDistance();
                    btnAddRegion.Visibility = Visibility.Visible;
                    btnClearRegion.Visibility = Visibility.Visible;
                    /*DataModel.Localization startLocal = new DataModel.Localization() { latitude = location.Latitude.ToString(), longitude = location.Longitude.ToString() };
                    DataModel.Localization endLocal = new DataModel.Localization() { latitude = location1.Latitude.ToString(), longitude = location1.Longitude.ToString() };
                    companyEntities.Localization.Add(startLocal);
                    companyEntities.Localization.Add(endLocal);
                    companyEntities.SaveChanges();
                    DataModel.Region newRegion = new DataModel.Region();
                    newRegion.Warehouse = companyEntities.Warehouse.First(w => w.code.Equals("MAZ"));
                    newRegion.idStartLocalization = startLocal.id;
                    newRegion.idEndLocalization = endLocal.id;
                    newRegion.code = "MAZ/SDL/01";
                    companyEntities.Region.Add(newRegion);
                    companyEntities.SaveChanges();*/
                    
                }     
            }    
        }

        private void ClearPolyline_Click(object sender, RoutedEventArgs e)
        {
            MyMap.ClearAllMap();
        }

        private void ConnectPushPins_Click(object sender, RoutedEventArgs e)
        {
            MyMap.ConnectPushpins();
        }

        private void BlackAndWhiteLayout(bool black = true)
        {
            if (black)
            {
                BackgroundOption = "Black";
                ForegroundOption = "White";
                InputOption = "#FF787878";
                ColorBtn = Color.FromArgb(100, 104, 104, 104);
            }
            else
            {
                BackgroundOption = "White";
                ForegroundOption = "Black";
                InputOption = "#FF6EAAFF";
                ColorBtn = Color.FromArgb(100, 193, 193, 193);
            }
            ResetContext();
        }
        private void ResetContext()
        {
            DataContext = null;
            DataContext = this;
        }
        private void BtnBackgroundColor(Button btn)
        {
            BtnParcel.Background = Brushes.Transparent;
            BtnEmployee.Background = Brushes.Transparent;
            BtnRegion.Background = Brushes.Transparent;
            BtnWarehouse.Background = Brushes.Transparent;
            BtnTransport.Background = Brushes.Transparent;
            BtnSettings.Background = Brushes.Transparent;

            GridSettings.Visibility = Visibility.Hidden;
            btn.Background = new SolidColorBrush(ColorBtn);
        }

        private void BtnParcel_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnParcel);
        }
        private void BtnEmployee_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnEmployee);
        }
        private void BtnRegion_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnRegion);
        }
        private void BtnWarehouse_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnWarehouse);
        }
        private void BtnTransport_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnTransport);
        }
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnSettings);
            GridSettings.Visibility = Visibility.Visible;
        }
        private void BtnToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            BlackAndWhiteLayout((bool)BtnToggleTheme.IsChecked);
        }

        private void SaveDBSettings()
        {
            Properties.Settings.Default.DBServer = DBServer.Text;
            Properties.Settings.Default.DBData = DBData.Text;
            Properties.Settings.Default.DBUser = DBUser.Text;
            Properties.Settings.Default.DBPassword = DBPassword.Password;
            Properties.Settings.Default.Save();
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionString = (ConnectionStringsSection)config.GetSection("connectionStrings");
            string cs = $"metadata=res://*/Model.csdl|res://*/Model.ssdl|res://*/Model.msl;provider=System.Data.SqlClient;provider connection string={"\""}data source={DBServer.Text};initial catalog={DBData.Text};integrated security=True;MultipleActiveResultSets=True;App=EntityFramework{"\""}";
            connectionString.ConnectionStrings["CompanyEntities"].ConnectionString = cs;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");

        }

        private void GetDBSettings()
        {
            DBServer.Text = Properties.Settings.Default.DBServer;
            DBData.Text = Properties.Settings.Default.DBData;
            DBUser.Text = Properties.Settings.Default.DBUser;
            DBPassword.Password = Properties.Settings.Default.DBPassword;
        }

        private void BtnSaveDBSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveDBSettings();
        }

        private void BtnTestConn_Click(object sender, RoutedEventArgs e)
        {
            BtnTestConn.IsEnabled = false;
            TestConnection = "";
            ResetContext();
            var t = new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Conn = new Connection(DBServer.Text, DBData.Text, DBUser.Text, DBPassword.Password);
                });
                if (Conn.ConnectionSql())
                {
                    TestConnection = "Udane połączenie z bazą danych";
                    ColorConnection = "Green";
                }
                else
                {
                    TestConnection = "Nieudane połącznie z bazą danych";
                    ColorConnection = "Red";
                }
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    BtnTestConn.IsEnabled = true;
                    ResetContext();
                });
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        
    }
}
