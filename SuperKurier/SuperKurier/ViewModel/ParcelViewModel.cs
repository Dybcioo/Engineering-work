using Caliburn.Micro;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SuperKurier.ViewModel
{
    public class ParcelViewModel : BaseViewModel
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
        private CompanyEntities companyEntities;

        public Visibility VisibilityOption
        {
            get { return _visibilityOption; }
            set
            {
                _visibilityOption = value;
                OnPropertChanged("VisibilityOption");
            }
        }

        public BindableCollection<Status> Status { get; set; }
        public BindableCollection<TypeOfParcel> TypeOfParcel { get; set; }
        public BindableCollection<Warehouse> Warehouse { get; set; }
        private Status _statusSelected;
        public Status StatusSelected
        {
            get { return _statusSelected; }
            set
            {
                if (value != _statusSelected)
                {
                    _statusSelected = value;
                    OnPropertChanged("StatusSelected");
                }
            }
        }

        private TypeOfParcel _typeOfParcelSelected;
        public TypeOfParcel TypeOfParcelSelected
        {
            get { return _typeOfParcelSelected; }
            set
            {
                if (value != _typeOfParcelSelected)
                {
                    _typeOfParcelSelected = value;
                    OnPropertChanged("TypeOfParcelSelected");
                }
            }
        }

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

        public ParcelViewModel()
        {
            companyEntities = new CompanyEntities();
            Status = new BindableCollection<Status>(companyEntities.Status.ToList());
            TypeOfParcel = new BindableCollection<TypeOfParcel>(companyEntities.TypeOfParcel.ToList());
            Warehouse = new BindableCollection<Warehouse>(companyEntities.Warehouse.ToList());
            Status.Insert(0, new Status() { status1 = "Wybierz status" });
            StatusSelected = Status.FirstOrDefault();
            TypeOfParcel.Insert(0, new TypeOfParcel() { type = "Wybierz typ" });
            TypeOfParcelSelected = TypeOfParcel.FirstOrDefault();
            Warehouse.Insert(0, new Warehouse() { code = "Wybierz magazyn" });
            WarehouseSelected = Warehouse.FirstOrDefault(w => w.id == Properties.Settings.Default.Warehouse);
        }
    }
}
