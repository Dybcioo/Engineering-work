using Caliburn.Micro;
using DataModel;
using Microsoft.Maps.MapControl.WPF;
using SuperKurier.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : Page
    {

        public bool IsBlack = true;
        public BindableCollection<Employee> Employees { get; set; }
        private CompanyEntities companyEntities = new CompanyEntities();

        public EmployeeView()
        {
            InitializeComponent();
            Employees = new BindableCollection<Employee>(companyEntities.Employee.ToList());
            DataGridEmployees.DataContext = Employees;
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
