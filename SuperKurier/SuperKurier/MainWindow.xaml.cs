using Caliburn.Micro;
using ConnectionSQL;
using DataModel;
using Microsoft.Maps.MapControl.WPF;
using SuperKurier.ViewModel;
using System;
using System.CodeDom;
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
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SuperKurier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string TestConnection { get; set; }
        public string ColorConnection { get; set; }
        public bool IsBlack = true;
        public Connection Conn { get; set; }
        public BindableCollection<Employee> Employees { get; set; }
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
        
        private MapPolyline polyline = null;
        private Location location = null;
        private bool regionSquare = false;
        private bool activationFunction = true;
        private CompanyEntities companyEntities = new CompanyEntities();
        private bool regionVisibility = false;
        private DataModel.Region region = null;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            /*BlackAndWhiteLayout();
            GetDBSettings();
            LoadWarehouse();*/
        }

        private void LoadWarehouse()
        {
            var temp = companyEntities.Warehouse.FirstOrDefault(w => w.id == Properties.Settings.Default.Warehouse);
            if (temp != null)
                WarehouseSelectedSetting = temp;
            Warehouses = new BindableCollection<Warehouse>(companyEntities.Warehouse.ToList());
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
            GridEmployee.Visibility = Visibility.Hidden;
           // btn.Background = new SolidColorBrush(ColorBtn);
        }

        private void BtnParcel_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnParcel);
        }
        private void BtnEmployee_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnEmployee);
            Employees = new BindableCollection<Employee>(companyEntities.Employee.ToList());
            DataGridEmployees.DataContext = Employees;
            GridEmployee.Visibility = Visibility.Visible;
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
            IsBlack = (bool)BtnToggleTheme.IsChecked;
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

        private void BtnEmployees_Click(object sender, RoutedEventArgs e)
        {
            btnEmployees.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            btnCustomer.Background = Brushes.Black;
            Panel.SetZIndex(btnEmployees, 1);
        }

        private void BtnCustomer_Click(object sender, RoutedEventArgs e)
        {
            btnCustomer.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            btnEmployees.Background = Brushes.Black;
            Panel.SetZIndex(btnEmployees, 0);
        }

        private void DataGridEmployeesRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dgr = (DataGridRow)sender;
            Employee empl = (Employee)dgr.Item;
            if (empl.Address != null && empl.Address.Localization != null)
                EmployeeMap.CheckingPushpin(e, new Location() { Latitude = double.Parse(empl.Address.Localization.latitude), Longitude = double.Parse(empl.Address.Localization.longitude) });
            DataContext = new EmployeeEditViewModel(empl, IsBlack, this);
            TurnOnOffEmployeePanel(false);
            BtnSaveEmployee.Content = "Edytuj";
        }

        private void EmployeeMap_MouseMove(object sender, MouseEventArgs e)
        {
            EmployeeScrollViewer.ScrollToVerticalOffset(300D);
        }

        private void EmployeeMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            EmployeeMap.ClearAllMap();
            EmployeeMap.CheckingPushpin(e);
        }

        private void BtnNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            TurnOnOffEmployeePanel(false);
            BtnSaveEmployee.Content = "Dodaj nowego pracownika";
            DataContext = new EmployeeEditViewModel(new Employee(), IsBlack, this);
        }

        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TurnOnOffEmployeePanel(true);
        }

        private void TurnOnOffEmployeePanel(bool isOff)
        {
            if (isOff)
                EmployeeScrollViewer.Visibility = Visibility.Hidden;
            else
                EmployeeScrollViewer.Visibility = Visibility.Visible;

            btnEmployees.IsEnabled = isOff;
            btnCustomer.IsEnabled = isOff;
            PanelEmployees.IsEnabled = isOff;
        }

        private void BtnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            ((EmployeeEditViewModel)DataContext).ExecuteSaveEmployee();
            Employees = new BindableCollection<Employee>(companyEntities.Employee.ToList());
            DataGridEmployees.DataContext = Employees;
            TurnOnOffEmployeePanel(true);
        }
        private int _noOfErrorsOnScreen = 0;

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void AddCustomer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void BtnSearchEmployees_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Employee> employees = new List<Employee>();
            employees = companyEntities.Employee.ToList();
            string text = BtnSearchEmployees.Text.ToUpper();
            Employees = new BindableCollection<Employee>(employees.Where(em => em.firstName.ToUpper().Contains(text) || em.lastName.ToUpper().Contains(text) || em.code.ToUpper().Contains(text)));
            DataGridEmployees.DataContext = Employees;
        }
    }
}
