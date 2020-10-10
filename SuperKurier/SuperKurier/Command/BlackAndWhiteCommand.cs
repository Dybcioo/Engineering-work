using SuperKurier.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SuperKurier.Command
{
    class BlackAndWhiteCommand : ICommand
    {
        MainViewModel viewModel;

        public BlackAndWhiteCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.BlackAndWhiteLayout(viewModel.IsBlack);
        }
    }
}
