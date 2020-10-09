using SuperKurier.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace SuperKurier.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;  

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set 
            {
                _selectedViewModel = value;
                OnPropertChanged(nameof(SelectedViewModel));
            }
        }

        private ICommand _menuViewCommand;

        public ICommand MenuViewCommand
        {
            get { return _menuViewCommand; }
            set
            {
                _menuViewCommand = value;
                OnPropertChanged(nameof(MenuViewCommand));
            }
        }

        public MainViewModel()
        {
            MenuViewCommand = new MenuViewCommand(this);
        }
    }
}
