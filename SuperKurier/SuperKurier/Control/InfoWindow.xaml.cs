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
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window, INotifyPropertyChanged
    {
        public bool Answer { get; set; } = false;
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
            set {}
        }

        public InfoWindow()
        {
            InitializeComponent();
            this.Title = "";
        }

        public void ShowInfo(string info)
        {
            informationLabel.Content = info;
            this.ShowDialog();
        }

        public void ShowInfo(string info, string title)
        {
            this.Title = title;
            informationLabel.Content = info;
            this.ShowDialog();
        }

        public void ShowInfo(string info, string title, string btnLeft , string btnRight)
        {
            this.btnLeft.Content = btnLeft;
            this.btnRight.Content = btnRight;
            this.Title = title;
            informationLabel.Content = info;
            this.ShowDialog();
        }

        public void ShowInfo(string info,string title, string btnRight)
        {
            this.btnLeft.Visibility = Visibility.Hidden;
            this.btnRight.Content = btnRight;
            informationLabel.Content = info;
            this.ShowDialog();
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            Answer = false;
            this.Close();
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            Answer = true;
            this.Close();
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
    }
}
