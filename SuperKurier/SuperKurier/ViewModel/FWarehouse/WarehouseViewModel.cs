using Caliburn.Micro;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SuperKurier.ViewModel.FWarehouse
{
    class WarehouseViewModel : BaseViewModel
    {
        private BaseViewModel _addViewModel;

        public BaseViewModel AddViewModel
        {
            get { return _addViewModel; }
            set
            {
                _addViewModel = value;
                OnPropertChanged(nameof(AddViewModel));
            }
        }

        private Visibility _visibilityOption;

        public Visibility VisibilityOption
        {
            get { return _visibilityOption; }
            set
            {
                _visibilityOption = value;
                OnPropertChanged("VisibilityOption");
            }
        }

        private CompanyEntities companyEntities;
        public BindableCollection<Warehouse> Warehouse { get; set; }

        private Warehouse _warehouseSelected;
        public Warehouse WarehouseSelected
        {
            get { return _warehouseSelected; }
            set
            {
                if (value != _warehouseSelected)
                {
                    _warehouseSelected = value;
                    OnPropertChanged("WarehouseSelected");
                }
            }
        }

        public WarehouseViewModel()
        {
            companyEntities = new CompanyEntities();
            Warehouse = new BindableCollection<Warehouse>(companyEntities.Warehouse.ToList());
            Warehouse.Insert(0, new Warehouse() { code = "Wybierz magazyn" });
            WarehouseSelected = Warehouse.FirstOrDefault(w => w.id == Properties.Settings.Default.Warehouse);
        }
    }
}
