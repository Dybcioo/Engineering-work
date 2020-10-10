using Caliburn.Micro;
using ConnectionSQL;
using DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace SuperKurier.View
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView : Page
    {

        public string TestConnection { get; set; }
        public string ColorConnection { get; set; }

        public Connection Conn { get; set; }

        public BindableCollection<Warehouse> Warehouses { get; set; }
        private Warehouse warehouseSelectedSetting;
        public Warehouse WarehouseSelectedSetting
        {
            get
            { return warehouseSelectedSetting; }
            set
            {
                warehouseSelectedSetting = value;
                Properties.Settings.Default.Warehouse = WarehouseSelectedSetting.id;
                Properties.Settings.Default.Save();
            }
        }

        private CompanyEntities companyEntities = new CompanyEntities();

        public SettingView()
        {
            InitializeComponent();
            LoadWarehouse();
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
            //ResetContext();
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
                    //ResetContext();
                });
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void LoadWarehouse()
        {
            var temp = companyEntities.Warehouse.FirstOrDefault(w => w.id == Properties.Settings.Default.Warehouse);
            if (temp != null)
                WarehouseSelectedSetting = temp;
            Warehouses = new BindableCollection<Warehouse>(companyEntities.Warehouse.ToList());
            //ResetContext();
        }


    }
}
