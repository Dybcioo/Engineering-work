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
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : Page
    {
        public bool IsBlack = true;
        public BindableCollection<Customer> Customers { get; set; }
        private CompanyEntities companyEntities = new CompanyEntities();

        public CustomerView()
        {
            InitializeComponent();
            Customers = new BindableCollection<Customer>(companyEntities.Customer.ToList());
            DataGridCustomers.DataContext = Customers;
        }

        private void btnEmployees_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnNewCustomers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var customer = (Customer)((DataGridRow)sender).Item;
            DataContext = new CustomerEditViewModel(customer);
            TurnOnOffCustomerPanel(false);
            BtnSaveCustomer.Content = "Edytuj";
            BtnDeleteCustomer.Visibility = Visibility.Visible;
        }

        private void TurnOnOffCustomerPanel(bool isOff)
        {
            if (isOff)
                CustomerScrollViewer.Visibility = Visibility.Hidden;
            else
                CustomerScrollViewer.Visibility = Visibility.Visible;

            btnEmployees.IsEnabled = isOff;
            btnCustomer.IsEnabled = isOff;
            PanelCustomers.IsEnabled = isOff;
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

            if (CustomerMap.GetPushpinLocation() == null)
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

        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TurnOnOffCustomerPanel(true);
        }

        private void BtnSearchCustomers_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Customer> customers = new List<Customer>();
            customers = companyEntities.Customer.ToList();
            string text = BtnSearchCustomers.Text.ToUpper();
            Customers = new BindableCollection<Customer>(customers.Where(em => 
                (em.firstName != null && em.firstName.ToUpper().Contains(text))     ||
                (em.lastName != null && em.lastName.ToUpper().Contains(text))       || 
                (em.Company != null &&  em.Company.name.ToUpper().Contains(text)    ||
                (em.Company != null && em.Company.NIP.ToString().Contains(text)))));
            DataGridCustomers.DataContext = Customers;
        }

        private void CustomerMap_MouseMove(object sender, MouseEventArgs e)
        {
            CustomerScrollViewer.ScrollToVerticalOffset(300D);
        }
    }
}
