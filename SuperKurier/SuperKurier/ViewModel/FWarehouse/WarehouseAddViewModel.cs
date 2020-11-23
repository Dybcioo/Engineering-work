using Caliburn.Micro;
using DataModel;
using SuperKurier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace SuperKurier.ViewModel.FWarehouse
{
    class WarehouseAddViewModel : WarehouseViewModel
    {
        public CompanyEntities companyEntities { get; set; }
        private BindableCollection<Parcel> _parcels;
        public BindableCollection<Parcel> Parcels 
        {
            get { return _parcels; }
            set
            {
                _parcels = value;
                OnPropertChanged(nameof(Parcels));
            }
        }
        private Parcel _parcelSelected;
        public Parcel ParcelSelected
        {
            get
            { return _parcelSelected; }
            set
            {
                _parcelSelected = value;
                OnPropertChanged(nameof(ParcelSelected));
            }
        }

        private Warehouse _warehouse;

        public WarehouseAddViewModel()
        {
            companyEntities = new CompanyEntities();
            _warehouse = companyEntities.Warehouse.FirstOrDefault(w => w.id == Properties.Settings.Default.Warehouse);
            Parcels = new BindableCollection<Parcel>(companyEntities.Parcel
                .Include(p => p.Region)
                .Include(p => p.Region1)
                .Where(p => p.idStatus <= (int)enumParcelStatus.received
                 && (_warehouse.id == p.Region.idWarehouse || _warehouse.id == p.Region1.idWarehouse))
                .ToList());
            Parcels.Insert(0, new Parcel() { code = "Wybierz przesyłkę" });
            ParcelSelected = Parcels.FirstOrDefault();
        }

        public void UpdateParcelList(List<Parcel> parcels)
        {
            var parcelsId = parcels.Select(p => p.id).ToList();
            Parcels = null;
            Parcels = new BindableCollection<Parcel>(companyEntities.Parcel
                .Include(p => p.Region)
                .Include(p => p.Region1)
                .Where(p => p.idStatus <= (int)enumParcelStatus.received
                 && (_warehouse.id == p.Region.idWarehouse || _warehouse.id == p.Region1.idWarehouse)
                 && !parcelsId.Contains(p.id))
                .ToList());
            Parcels.Insert(0, new Parcel() { code = "Wybierz przesyłkę" });
            ParcelSelected = Parcels.FirstOrDefault( p => p.id != 0);
        }
    }
}
