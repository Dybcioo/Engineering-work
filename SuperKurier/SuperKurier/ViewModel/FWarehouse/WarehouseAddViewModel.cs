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
        public Document Document;
        public EnumTypeOfDocument DocumentType;
        public List<Parcel> actuallyParcelList;

        public WarehouseAddViewModel(Document document = null, EnumTypeOfDocument enumDocumentType = EnumTypeOfDocument.PZ)
        {
            companyEntities = new CompanyEntities();
            Document = document;
            DocumentType = enumDocumentType;
            _warehouse = companyEntities.Warehouse.FirstOrDefault(w => w.id == Properties.Settings.Default.Warehouse);
                if (document != null)
                    actuallyParcelList = companyEntities.ParcelMoving
                        .Include(p => p.Parcel)
                        .Where(p => DocumentType == EnumTypeOfDocument.PZ ? p.idDocPZ == document.id : p.idDocWZ == document.id)
                        .Select(p => p.Parcel)
                        .ToList();

        }

        public void UpdateParcelList(List<Parcel> parcels, EnumTypeOfDocument enumDocumentType)
        {
            try
            {
                var parcelsId = parcels.Select(p => p.id).ToList();
                Parcels = null;
                if (enumDocumentType == EnumTypeOfDocument.PZ)
                {
                    Parcels = new BindableCollection<Parcel>(companyEntities.Parcel
                    .Include(p => p.Region)
                    .Include(p => p.Region1)
                    .Include(p => p.ParcelMoving)
                    .Include(p => p.Status)
                    .Include(p => p.Status.HistoryOfParcel)
                    .Where(p => ((p.ParcelMoving.Count == 0 && companyEntities.HistoryOfParcel.FirstOrDefault(h => h.idParcel == p.id).idWarehouse == _warehouse.id && p.idStatus <= (int)EnumParcelStatus.received)
                        || (p.ParcelMoving.Count > 0 && p.idStatus == (int)EnumParcelStatus.beetwen && _warehouse.id == p.Region.idWarehouse))
                     && !parcelsId.Contains(p.id))
                    .ToList());
                }
                else if (enumDocumentType == EnumTypeOfDocument.WZ)
                {
                    var _parcels = new List<Parcel>();
                    foreach (var pm in companyEntities.ParcelMoving.Include(p => p.Document))
                    {
                        if (pm.Document == null || !pm.Document.exposure || pm.Document.idWarehouse != _warehouse.id || pm.readingWZ || parcelsId.Contains(pm.idParcel))
                            continue;
                        _parcels.Add(companyEntities.Parcel.FirstOrDefault(p => p.id == pm.idParcel));
                    }
                    Parcels = new BindableCollection<Parcel>(_parcels);
                }

                Parcels.Insert(0, new Parcel() { code = "Wybierz przesyłkę" });
                ParcelSelected = Parcels.FirstOrDefault(p => p.id != 0);
            }
            catch (Exception ex) { }
        }
    }
}
