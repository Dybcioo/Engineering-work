using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKurier.ViewModel
{
    class ParcelViewModel : BaseViewModel
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
    }
}
