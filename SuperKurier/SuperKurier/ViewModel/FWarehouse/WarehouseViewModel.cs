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
    }
}
