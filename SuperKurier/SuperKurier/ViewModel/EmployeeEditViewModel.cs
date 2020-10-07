﻿using Caliburn.Micro;
using DataModel;
using GalaSoft.MvvmLight.Command;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SuperKurier.ViewModel
{
    class EmployeeEditViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private Employee Employee;
        private CompanyEntities CompanyEntities = new CompanyEntities();
        public string BackgroundOption { get; set; }
        public string ForegroundOption { get; set; }
        public string InputOption { get; set; }
        public Color ColorBtn { get; set; }
        private MainWindow MainWindow;

        public string EmployeeFirstName
        {
            get { return Employee.firstName; }
            set 
            {
                if(value != Employee.firstName)
                {
                    Employee.firstName = value;
                    OnPropertyChange("EmployeeFirstName");
                }
            }
        }

        public string EmployeeLastName
        {
            get { return Employee.lastName; }
            set
            {
                if (value != Employee.firstName)
                {
                    Employee.lastName = value;
                    OnPropertyChange("EmployeeLastName");
                }
            }
        }

        public string EmployeePassword
        {
            get { return Employee.password; }
            set
            {
                if (value != Employee.password)
                {
                    Employee.password = value;
                    OnPropertyChange("EmployeePassword");
                }
            }
        }

        public string EmployeeRepeatPassword
        {
            get { return Employee.password; }
            set
            {
                if (value != Employee.password)
                {
                    Employee.password = value;
                    OnPropertyChange("EmployeeRepeatPassword");
                }
            }
        }

        public decimal EmployeeSalary
        {
            get 
            {
                if (Employee.salary != null)
                    return (decimal) Employee.salary;
                else
                    return 0M;
            }
            set
            {
                if (value != Employee.salary)
                {
                    Employee.salary = value;
                    OnPropertyChange("EmployeeSalary");
                    //ValidateProperty(value, "EmployeeSalary");
                }
            }
        }

        private void ValidateProperty<T>(T value, string name)
        {
            Validator.ValidateProperty(value, new ValidationContext(this, null, null)
            {
                MemberName = name
            });
        }

        public BindableCollection<DataModel.Region> Regions { get; set; }
        public BindableCollection<Position> Positions { get; set; }
        public BindableCollection<Warehouse> Warehouses { get; set; }
        public DataModel.Region RegionSelected { get; set; }
        public Position PositionSelected { get; set; }
        public Warehouse WarehouseSelected { get; set; }

        public string EmployeeCountry
        {
            get { return Employee.Address.country; }
            set
            {
                if (value != Employee.Address.country)
                {
                    Employee.Address.country = value;
                    OnPropertyChange("EmployeeCountry");
                }
            }
        }

        public string EmployeeCity
        {
            get { return Employee.Address.city; }
            set
            {
                if (value != Employee.Address.city)
                {
                    Employee.Address.city = value;
                    OnPropertyChange("EmployeeCity");
                }
            }
        }

        public string EmployeePostCode
        {
            get { return Employee.Address.postalCode; }
            set
            {
                if (value != Employee.Address.postalCode)
                {
                    Employee.Address.postalCode = value;
                    OnPropertyChange("EmployeePostCode");
                }
            }
        }

        public string EmployeeStreet
        {
            get { return Employee.Address.street; }
            set
            {
                if (value != Employee.Address.street)
                {
                    Employee.Address.street = value;
                    OnPropertyChange("EmployeeStreet");
                }
            }
        }

        public string EmployeeNumberOfHouse
        {
            get { return Employee.Address.numberOfHouse; }
            set
            {
                if (value != Employee.Address.numberOfHouse)
                {
                    Employee.Address.numberOfHouse = value;
                    OnPropertyChange("EmployeeNumberOfHouse");
                }
            }
        }

        public ICommand SaveEmployee { get; private set; }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "EmployeeSalary")
                {
                    if (string.IsNullOrEmpty(EmployeeSalary.ToString()))
                        result = "Please enter a First Name";
                }
                return result;
            }
        }

        public EmployeeEditViewModel(Employee employee, bool black, MainWindow window)
        {
            BlackAndWhiteLayout(black);
            Employee = employee;
            if (Employee.Address == null)
                Employee.Address = new Address();
            MainWindow = window;
            Positions = new BindableCollection<Position>(CompanyEntities.Position.ToList());
            Regions = new BindableCollection<DataModel.Region>(CompanyEntities.Region.ToList());
            Warehouses = new BindableCollection<Warehouse>(CompanyEntities.Warehouse.ToList());

            if (employee.Position != null)
                PositionSelected = employee.Position;
            if (employee.Warehouse != null)
                WarehouseSelected = employee.Warehouse;
            if (employee.Region != null)
                RegionSelected = employee.Region;

            SaveEmployee = new RelayCommand(
                () => ExecuteSaveEmployee(),
                () => true);
        }

        public EmployeeEditViewModel() { }

        public void ExecuteSaveEmployee()
        {
            Location location = new Location();
            location = MainWindow.EmployeeMap.GetPushpinLocation();
            MainWindow.EmployeeMap.ReserCounter();

            if (Employee.dateOfEmployment == null)
                Employee.dateOfEmployment = DateTime.Now;

            Employee.Position = PositionSelected;
            Employee.idPosition = PositionSelected.id;
            Employee.idWarehouse = WarehouseSelected.id;
            Employee.Warehouse = WarehouseSelected;
            Employee.idRegion = RegionSelected.id;
            Employee.Region = RegionSelected;

            if (Employee.code == null)
            {
                Employee.Address.Localization = new DataModel.Localization() { latitude = location.Latitude.ToString(), longitude = location.Longitude.ToString() };
                Employee.code = $"/{Employee.firstName}/{Employee.Warehouse.code}/{Employee.Position.position1}";
                CompanyEntities.Address.Add(Employee.Address);
                CompanyEntities.Employee.Add(Employee);
                CompanyEntities.SaveChanges();
                Employee = CompanyEntities.Employee.OrderByDescending(em => em.id).First();
                Employee.code = $"{Employee.id}{Employee.code}";
            }
            else
            {
                Employee.Address.Localization.latitude = location.Latitude.ToString();
                Employee.Address.Localization.longitude = location.Longitude.ToString();
                var empl = CompanyEntities.Employee.Find(Employee.id);
                var address = CompanyEntities.Address.Find(Employee.Address.id);
                var localization = CompanyEntities.Localization.Find(address.Localization.id);
                CompanyEntities.Entry(localization).CurrentValues.SetValues(Employee.Address.Localization);
                CompanyEntities.Entry(address).CurrentValues.SetValues(Employee.Address);
                CompanyEntities.Entry(empl).CurrentValues.SetValues(Employee);
            }
                CompanyEntities.SaveChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void BlackAndWhiteLayout(bool black)
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
        }
    }
}