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
                }
            }
        }

        public string CustomerLastName
        {
            get { return Customer.lastName; }
            set
            {
                if (value != Customer.firstName)
                {
                    Customer.lastName = value;
                    OnPropertyChange("CustomerLastName");
                }
            }
        }

        public string CustomerCompanyName
        {
            get { return Customer.Company.name; }
            set
            {
                if (Customer.Company == null || value != Customer.Company.name)
                {
                    Customer.Company.name = value;
                    OnPropertyChange("CustomerCompanyName");
                }
            }
        }

        public int CustomerCompanyNIP
        {
            get { return Customer.Company.NIP; }
            set
            {
                if (Customer.Company == null || value != Customer.Company.NIP)
                {
                    Customer.Company.NIP = value;
                    OnPropertyChange("CustomerCompanyName");
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
                        if (string.IsNullOrWhiteSpace(CustomerFirstName))
                            result = "Pole nie może być puste";
                        else if (CustomerFirstName.Length > 20)
                            result = "Długość imienia nie może przekraczać 20 znaków";
                        break;
                    case "CustomerLastName":
                        if (string.IsNullOrWhiteSpace(CustomerLastName))
                            result = "Pole nie może być puste";
                        else if (CustomerLastName.Length > 30)
                            result = "Długość nazwiska nie może przekraczać 30 znaków";
                        break;
                }
                return result;
            }
        }

        public CustomerEditViewModel(Customer customer)
        {
            Customer = customer;
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
