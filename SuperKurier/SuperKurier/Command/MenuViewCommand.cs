using SuperKurier.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SuperKurier.Command
{
    class MenuViewCommand : ICommand
    {
        MainViewModel viewModel;

        public MenuViewCommand(MainViewModel viewModel)
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
            var temp = (Button)parameter;

            switch (temp?.Name)
            {
                case "BtnRegion":
                    viewModel.SelectedViewModel = new RegionViewModel();
                    break;
                case "BtnEmployee":
                    viewModel.SelectedViewModel = new EmployeeViewModel();
                    break;
                case "BtnSettings":
                    viewModel.SelectedViewModel = new SettingViewModel(viewModel);
                    break;
                case "BtnParcel":
                    viewModel.SelectedViewModel = new ParcelViewModel();
                    break;
            }
            viewModel.BlackAndWhiteLayout(viewModel.IsBlack);
            
        }
    }
}
