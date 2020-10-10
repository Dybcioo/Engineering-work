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

        public SettingViewModel(MainViewModel mainViewModel)
        {
            BlackAndWhiteCommand = new BlackAndWhiteCommand(mainViewModel);
        }
    }
}
