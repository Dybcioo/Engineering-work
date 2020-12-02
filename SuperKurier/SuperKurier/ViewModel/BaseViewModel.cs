using SuperKurier.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SuperKurier.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        private string _backgroundOption;
        public string BackgroundOption
        {
            get { return _backgroundOption; }
            set
            {
                _backgroundOption = value;
                OnPropertChanged(nameof(BackgroundOption));
            }
        }
        private string _foregroundOption;
        public string ForegroundOption
        {
            get { return _foregroundOption; }
            set
            {
                _foregroundOption = value;
                OnPropertChanged(nameof(ForegroundOption));
            }
        }
        private string _inputOption;
        public string InputOption
        {
            get { return _inputOption; }
            set
            {
                _inputOption = value;
                OnPropertChanged(nameof(InputOption));
            }
        }
        private Color _colorBtn;
        public Color ColorBtn
        {
            get { return _colorBtn; }
            set
            {
                _colorBtn = value;
                OnPropertChanged(nameof(ColorBtn));
            }
        }
        public  bool IsBlack 
        {
            get { return Properties.Settings.Default.IsBlack; } 
            set
            {
                Properties.Settings.Default.IsBlack = value;
                BlackAndWhiteLayout(IsBlack);
                OnPropertChanged(nameof(IsBlack));
            }
        }
        private string _footerEmployeeCode;
        public string FooterEmployeeCode
        {
            get { return _footerEmployeeCode; }
            set
            {
                _footerEmployeeCode = value;
                OnPropertChanged(nameof(FooterEmployeeCode));
            }
        }
        private string _footerWarehouse;
        public string FooterWarehouse
        {
            get { return _footerWarehouse; }
            set
            {
                _footerWarehouse = value;
                OnPropertChanged(nameof(FooterWarehouse));
            }
        }
        private Visibility _active;
        public Visibility Active
        {
            get { return _active; }
            set
            {
                _active = value;
                OnPropertChanged(nameof(Active));
            }
        }

        public BaseViewModel()
        {
            BlackAndWhiteLayout(Properties.Settings.Default.IsBlack);
        }


        public void BlackAndWhiteLayout(bool black)
        {
            if (black)
            {
                BackgroundOption = "Black";
                ForegroundOption = "White";
                InputOption = "#FF787878";
                ColorBtn = Color.FromArgb(100, 104, 104, 104);
            }
            else
            {
                BackgroundOption = "White";
                ForegroundOption = "Black";
                InputOption = "#FF6EAAFF";
                ColorBtn = Color.FromArgb(100, 193, 193, 193);
            }
        }

    }
}
