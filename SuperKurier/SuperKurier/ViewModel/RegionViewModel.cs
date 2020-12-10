using Caliburn.Micro;
using DataModel;
using Microsoft.Maps.MapControl.WPF;
using SuperKurier.Command;
using SuperKurier.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SuperKurier.ViewModel
{
    class RegionViewModel : BaseViewModel
    {
        public BindableCollection<Warehouse> Warehouses { get; set; }
        private Warehouse _warehouseSelectedRegion;
        public Warehouse WarehouseSelectedRegion
        {
            get
            { return _warehouseSelectedRegion; }
            set
            {
                _warehouseSelectedRegion = value;
                Employees = null;
                Employees = new BindableCollection<Employee>(new CompanyEntities().Employee.Where(e => e.Position.position1.Equals("Kurier") && e.isActive && e.idWarehouse == WarehouseSelectedRegion.id).ToList());
                EmployeeSelectedRegion = Employees.FirstOrDefault();
                OnPropertChanged(nameof(WarehouseSelectedRegion));
                OnPropertChanged(nameof(Employees));
            }
        }

        public BindableCollection<Employee> Employees { get; set; }
        private Employee _employeeSelectedRegion;
        public Employee EmployeeSelectedRegion
        {
            get
            { return _employeeSelectedRegion; }
            set
            {
                _employeeSelectedRegion = value;
                OnPropertChanged(nameof(EmployeeSelectedRegion));
            }
        }

        public RegionViewModel()
        {
            Warehouses = new BindableCollection<Warehouse>(new CompanyEntities().Warehouse.ToList());
            WarehouseSelectedRegion = Warehouses.FirstOrDefault();

            Employees = new BindableCollection<Employee>(new CompanyEntities().Employee.Where(e => e.Position.position1.Equals("Kurier") && e.isActive && e.idWarehouse == WarehouseSelectedRegion.id).ToList());
            EmployeeSelectedRegion = Employees.FirstOrDefault();
        }

    }
}
