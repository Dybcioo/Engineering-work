using DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKurier.ViewModel
{
    class CustomerEditViewModel : BaseViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        private Customer Customer;
        private CompanyEntities CompanyEntities = new CompanyEntities();

        public string CustomerFirstName
        {
            get { return Customer.firstName; }
            set
            {
                if (value != Customer.firstName)
                {
                    Customer.firstName = value;
                    OnPropertyChange("CustomerFirstName");
                    OnPropertyChange("CustomerCompanyNIP");
                    OnPropertyChange("CustomerCompanyName");
                }
            }
        }

        public string CustomerLastName
        {
            get { return Customer.lastName; }
            set
            {
                if (value != Customer.lastName)
                {
                    Customer.lastName = value;
                    OnPropertyChange("CustomerLastName");
                    OnPropertyChange("CustomerCompanyNIP");
                    OnPropertyChange("CustomerCompanyName");
                }
            }
        }

        private string _customerCompanyName;

        public string CustomerCompanyName
        {
            get { return _customerCompanyName; }
            set
            {
                if (Customer.Company == null || value != _customerCompanyName)
                {
                    _customerCompanyName = value;
                    OnPropertyChange("CustomerCompanyName");
                    OnPropertyChange("CustomerFirstName");
                    OnPropertyChange("CustomerLastName");
                }
            }
        }

        private string _customerCompanyNIP;

        public string CustomerCompanyNIP
        {
            get { return _customerCompanyNIP; }
            set
            {
                if (Customer.Company == null || value != _customerCompanyNIP)
                {
                    _customerCompanyNIP = value;
                    OnPropertyChange("CustomerCompanyNIP");
                    OnPropertyChange("CustomerFirstName");
                    OnPropertyChange("CustomerLastName");
                }
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case "CustomerFirstName":
                        if (!string.IsNullOrEmpty(CustomerCompanyName) || !string.IsNullOrEmpty(CustomerCompanyNIP))
                            break;
                        if (string.IsNullOrWhiteSpace(CustomerFirstName))
                            result = "Pole nie może być puste";
                        else if (CustomerFirstName.Length > 25)
                            result = "Długość imienia nie może przekraczać 25 znaków";
                        break;
                    case "CustomerLastName":
                        if (!string.IsNullOrEmpty(CustomerCompanyName) || !string.IsNullOrEmpty(CustomerCompanyNIP))
                            break;
                        if (string.IsNullOrWhiteSpace(CustomerLastName))
                            result = "Pole nie może być puste";
                        else if (CustomerLastName.Length > 30)
                            result = "Długość nazwiska nie może przekraczać 30 znaków";
                        break;
                    case "CustomerCompanyName":
                        if (!string.IsNullOrEmpty(CustomerFirstName) || !string.IsNullOrEmpty(CustomerLastName))
                            break;
                        if (string.IsNullOrWhiteSpace(CustomerCompanyName))
                            result = "Pole nie może być puste";
                        else if (CustomerCompanyName.Length > 25)
                            result = "Nazwa firmy nie może przekraczać 25 znaków";
                        break;
                    case "CustomerCompanyNIP":
                        int NIP;
                        bool condition = int.TryParse(CustomerCompanyNIP, out NIP);
                        if (!string.IsNullOrEmpty(CustomerFirstName) || !string.IsNullOrEmpty(CustomerLastName))
                            break;
                        if (string.IsNullOrEmpty(CustomerCompanyNIP))
                            result = "Pole nie może być puste";
                        else if (!condition)
                            result = "NIP musi zawierać wyłącznie cyfry";
                        else if (CustomerCompanyNIP.Length != 10)
                            result = "NIP musi zawierać 10 cyfr";
                        break;
                }
                return result;
            }
        }

        public CustomerEditViewModel(Customer customer)
        {
            Customer = customer;
            _customerCompanyNIP = Customer.Company?.NIP.ToString();
            _customerCompanyName = Customer.Company?.name.ToString();
        }

        public void ExecuteSaveEmployee()
        {
            //to Do
        }

        public void ExecuteDeleteEmployee()
        {
            //to Do
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
