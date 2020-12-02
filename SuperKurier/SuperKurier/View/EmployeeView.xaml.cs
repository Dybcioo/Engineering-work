using Caliburn.Micro;
using DataModel;
using Microsoft.Maps.MapControl.WPF;
using SuperKurier.Enums;
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

        public BindableCollection<Employee> Employees { get; set; }
        private CompanyEntities companyEntities = new CompanyEntities();

        public EmployeeView()
        {
            InitializeComponent();
            Employees = new BindableCollection<Employee>(companyEntities.Employee.ToList());
            DataGridEmployees.DataContext = Employees;
            
        }

        private void DataGridEmployeesRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            DataGridRow dgr = (DataGridRow)sender;
            Employee empl = (Employee)dgr.Item;
            var user = companyEntities.Employee.FirstOrDefault(e => e.id == Properties.Settings.Default.IdUser);
            if (empl.idPosition == (int)EnumPosition.Admin && user.idPosition != (int)EnumPosition.Admin)
                return;
            if (empl.idPosition == (int)EnumPosition.OfficeWorker && user.idPosition != (int)EnumPosition.Admin && empl.id != user.id)
                return;
            EmployeeSalary.IsEnabled = !(Properties.Settings.Default.IdUser == empl.id && empl.idPosition != (int)EnumPosition.Admin);
            EmployeePosition.IsEnabled = !(Properties.Settings.Default.IdUser == empl.id && empl.idPosition != (int)EnumPosition.Admin);
            if (empl.Address != null && empl.Address.Localization != null)
            EmployeeMap.CheckingPushpin(e, new Location() { Latitude = double.Parse(empl.Address.Localization.latitude), Longitude = double.Parse(empl.Address.Localization.longitude) });
            DataContext = new EmployeeEditViewModel(empl, this);
            TurnOnOffEmployeePanel(false);
            BtnSaveEmployee.Content = "Edytuj";
            BtnDeleteEmployee.Visibility = Visibility.Visible;
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
            BtnDeleteEmployee.Visibility = Visibility.Hidden;
            DataContext = new EmployeeEditViewModel(new Employee(), this);
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

        private void AddEmployee_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

            if(EmployeeMap.GetPushpinLocation() == null)
            {
                e.CanExecute = false;
                MapBorder.BorderThickness = new Thickness(1);
            }
            else
            {
                e.CanExecute = _noOfErrorsOnScreen == 0;
                MapBorder.BorderThickness = new Thickness(0);
            }
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

        private void BtnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            ((EmployeeEditViewModel)DataContext).ExecuteDeleteEmployee();
            Employees = new BindableCollection<Employee>(companyEntities.Employee.ToList());
            DataGridEmployees.DataContext = Employees;
            TurnOnOffEmployeePanel(true);
        }
    }
}
