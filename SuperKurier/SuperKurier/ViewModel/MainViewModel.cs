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

        public MainViewModel()
        {
            MenuViewCommand = new MenuViewCommand(this);
        }
    }
}
