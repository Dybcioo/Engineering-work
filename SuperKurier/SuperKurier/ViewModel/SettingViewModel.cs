using Caliburn.Micro;
using DataModel;
using SuperKurier.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SuperKurier.ViewModel
{
    class SettingViewModel : BaseViewModel
    {
        private ICommand _blackAndWhiteCommand;

        public ICommand BlackAndWhiteCommand
        {
            get { return _blackAndWhiteCommand; }
            set
            {
                _blackAndWhiteCommand = value;
                OnPropertChanged(nameof(BlackAndWhiteCommand));
            }
        }

        private CompanyEntities companyEntities = new CompanyEntities();
        public BindableCollection<Warehouse> Warehouses { get; set; }
        private Warehouse warehouseSelectedSetting;
        public Warehouse WarehouseSelectedSetting
        {
            get
            { return warehouseSelectedSetting; }
            set
            {
                warehouseSelectedSetting = value;
                Properties.Settings.Default.Warehouse = WarehouseSelectedSetting.id;
                Properties.Settings.Default.Save();
                OnPropertChanged(nameof(WarehouseSelectedSetting));
            }
        }

        public SettingViewModel(MainViewModel mainViewModel)
        {
            try
            {
                BlackAndWhiteCommand = new BlackAndWhiteCommand(mainViewModel);
                var temp = companyEntities.Warehouse.FirstOrDefault(w => w.id == Properties.Settings.Default.Warehouse);
                if (temp != null)
                    WarehouseSelectedSetting = temp;
                Warehouses = new BindableCollection<Warehouse>(companyEntities.Warehouse.ToList());
            }
            catch (Exception ex) { }
        }
    }
}
