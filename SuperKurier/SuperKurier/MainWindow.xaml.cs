using DataModel;
using SuperKurier.Control;
using SuperKurier.ViewModel;
using System.Windows;
using System.Linq;
using System.Data.Entity;


namespace SuperKurier
{
    /// <summary>
    /// Interaction logic for employeeView.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CompanyEntities companyEntities;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            companyEntities = new CompanyEntities();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            if (!loginWindow.Answer)
                Application.Current.Shutdown();
            var emp = companyEntities.Employee.Include(e => e.Warehouse).FirstOrDefault(e => e.id == Properties.Settings.Default.IdUser);
            var bvm = (BaseViewModel)DataContext;
            bvm.FooterEmployeeCode = emp.code;
            bvm.FooterWarehouse = emp.Warehouse.code;
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            if (!loginWindow.Answer)
                Application.Current.Shutdown();
            var emp = companyEntities.Employee.Include(e => e.Warehouse).FirstOrDefault(e => e.id == Properties.Settings.Default.IdUser);
            var bvm = (BaseViewModel)DataContext;
            bvm.FooterEmployeeCode = emp.code;
            bvm.FooterWarehouse = emp.Warehouse.code;
        }
    }
}
