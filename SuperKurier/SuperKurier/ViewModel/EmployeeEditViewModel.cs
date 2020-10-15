﻿using Caliburn.Micro;
using DataModel;
using GalaSoft.MvvmLight.Command;
using Microsoft.Maps.MapControl.WPF;
using SuperKurier.View;
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
        private EmployeeView employeeView;

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

        private string _employeeSalary;

        public string EmployeeSalary
        {
            get 
            {
                if (_employeeSalary != null)
                    return _employeeSalary;
                else if(Employee.salary != null)
                    return ((decimal)Employee.salary).ToString("0.##");
                else
                    return "0";
            }
            set
            {
                if (value != _employeeSalary)
                {
                    _employeeSalary = value;
                    OnPropertyChange("EmployeeSalary");   
                }
            }
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
                switch (columnName)
                {
                    case "EmployeeSalary":
                        decimal value;
                        if (decimal.TryParse(EmployeeSalary, out value))
                        {
                            if(value < 0)
                            {
                                result = "Wypłata musi być większa od zera";
                            }
                            else
                            {
                                Employee.salary = value;
                            }
                        }
                        else
                        {
                            result = "Podana wartość nie jest liczbą";
                        }
                        break;
                    case "EmployeeCountry":
                        if (string.IsNullOrWhiteSpace(EmployeeCountry))
                            result = "Pole nie może być puste";
                        else if (EmployeeCountry.Length > 20)
                            result = "Długość nazwy kraju nie może przekraczać 20 znaków";
                        break;
                    case "EmployeeCity":
                        if (string.IsNullOrWhiteSpace(EmployeeCity))
                            result = "Pole nie może być puste";
                        else if (EmployeeCity.Length > 30)
                            result = "Długość nazwy miasta nie może przekraczać 30 znaków";
                        break;
                    case "EmployeePostCode":
                        if (string.IsNullOrWhiteSpace(EmployeePostCode))
                            result = "Pole nie może być puste";
                        else if (EmployeePostCode.Length > 10)
                            result = "Długość kodu pocztowego nie może przekraczać 10 znaków";
                        break;
                    case "EmployeeStreet":
                        if (EmployeeStreet.Length > 30)
                            result = "Długość nazwy ulicy nie może przekraczać 30 znaków";
                        break;
                    case "EmployeeNumberOfHouse":
                        if (string.IsNullOrWhiteSpace(EmployeeNumberOfHouse))
                            result = "Pole nie może być puste";
                        else if (EmployeeNumberOfHouse.Length > 10)
                            result = "Długość numeru domu nie może przekraczać 10 znaków";
                        break;
                    case "EmployeeFirstName":
                        if (string.IsNullOrWhiteSpace(EmployeeFirstName))
                            result = "Pole nie może być puste";
                        else if (EmployeeFirstName.Length > 20)
                            result = "Długość imienia nie może przekraczać 20 znaków";
                        break;
                    case "EmployeeLastName":
                        if (string.IsNullOrWhiteSpace(EmployeeLastName))
                            result = "Pole nie może być puste";
                        else if (EmployeeLastName.Length > 30)
                            result = "Długość nazwiska nie może przekraczać 30 znaków";
                        break;
                    case "PositionSelected":
                        if (PositionSelected == null)
                            result = "Pole nie może być puste";
                        break;
                    case "WarehouseSelected":
                        if (WarehouseSelected == null)
                            result = "Pole nie może być puste";
                        break;
                    case "RegionSelected":
                        if (RegionSelected == null)
                            result = "Pole nie może być puste";
                        break;
                }
                return result;
            }
        }

        public EmployeeEditViewModel(Employee employee, bool black, EmployeeView window)
        {
            BlackAndWhiteLayout(black);
            Employee = employee;
            if (Employee.Address == null)
                Employee.Address = new Address();
            employeeView = window;
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
            location = employeeView.EmployeeMap.GetPushpinLocation();
            employeeView.EmployeeMap.ReserCounter();

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
                Employee.code = $"/{Employee.firstName}/{Employee.Position.position1}";
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
                Employee.code = $"{Employee.id}/{Employee.firstName}/{Employee.Position.position1}";
                var empl = CompanyEntities.Employee.Find(Employee.id);
                var address = CompanyEntities.Address.Find(Employee.Address.id);
                var localization = CompanyEntities.Localization.Find(address.Localization.id);
                CompanyEntities.Entry(localization).CurrentValues.SetValues(Employee.Address.Localization);
                CompanyEntities.Entry(address).CurrentValues.SetValues(Employee.Address);
                CompanyEntities.Entry(empl).CurrentValues.SetValues(Employee);
            }
                CompanyEntities.SaveChanges();
        }

        public void ExecuteDeleteEmployee()
        {
            CompanyEntities.Employee.Remove(CompanyEntities.Employee.Find(Employee.id));
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
