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
        public MainViewModel MainViewModel;
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
                companyEntities.Employee.FirstOrDefault(e => e.id == Properties.Settings.Default.IdUser).idWarehouse = warehouseSelectedSetting.id;
                companyEntities.SaveChanges();
                MainViewModel.FooterWarehouse = warehouseSelectedSetting.code;
                OnPropertChanged(nameof(WarehouseSelectedSetting));
            }
        }

        public SettingViewModel(MainViewModel mainViewModel)
        {
            try
            {
                MainViewModel = mainViewModel;
                BlackAndWhiteCommand = new BlackAndWhiteCommand(mainViewModel);
                var temp = companyEntities.Employee.FirstOrDefault(e => e.id == Properties.Settings.Default.IdUser).Warehouse;
                if (temp != null)
                    WarehouseSelectedSetting = temp;
                Warehouses = new BindableCollection<Warehouse>(companyEntities.Warehouse.ToList());
            }
            catch (Exception ex) { }
        }
    }
}
