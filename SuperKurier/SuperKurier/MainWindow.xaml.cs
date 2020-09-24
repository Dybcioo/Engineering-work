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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            BlackAndWhiteLayout();
            GetDBSettings();
        }

        private void MyMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            MyMap.CheckingPushpin();
        }
        private void ContextMenu_RightClick(object sender, MouseButtonEventArgs e)
        {
            ContextMenu context = new ContextMenu();
            context.IsOpen = true;
            var connectPushPins = new MenuItem() { Header = "Połącz pinezki" };
            var clear = new MenuItem() { Header = "Wyczyść trase" };
            clear.Click += ClearPolyline_Click;
            connectPushPins.Click += ConnectPushPins_Click;
            context.Items.Add(connectPushPins);
            context.Items.Add(clear);
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
            /*var comp = new CompanyEntities();
            comp.TypeOfDocument.ToList().ForEach(a => MessageBox.Show(a.type));*/
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
