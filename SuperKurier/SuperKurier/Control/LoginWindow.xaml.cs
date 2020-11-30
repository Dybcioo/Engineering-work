using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperKurier.Control
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        public bool Answer { get; set; } = false;
        #region SetColors
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        public bool IsBlack
        {
            get { return Properties.Settings.Default.IsBlack; }
            set { }
        }
        #endregion
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BlackAndWhiteLayout(bool black)
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
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BlackAndWhiteLayout(IsBlack);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btnSingIn_Click(object sender, RoutedEventArgs e)
        {
            Answer = true;
            Close();
        }
    }
}
