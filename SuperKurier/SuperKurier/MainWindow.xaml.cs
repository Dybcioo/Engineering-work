using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperKurier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string BackgroundOption { get; set; }
        public string ForegroundOption { get; set; }
        public string InputOption { get; set; }
        public Color ColorBtn { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            BlackAndWhiteLayout();
            GetDBSettings();
        }
        private void BlackAndWhiteLayout(bool black = true)
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
            DataContext = null;
            DataContext = this;
        }
        private void BtnBackgroundColor(Button btn)
        {
            BtnParcel.Background = Brushes.Transparent;
            BtnEmployee.Background = Brushes.Transparent;
            BtnRegion.Background = Brushes.Transparent;
            BtnWarehouse.Background = Brushes.Transparent;
            BtnTransport.Background = Brushes.Transparent;
            BtnSettings.Background = Brushes.Transparent;

            GridSettings.Visibility = Visibility.Hidden;
            btn.Background = new SolidColorBrush(ColorBtn);
        }

        private void BtnParcel_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnParcel);
        }
        private void BtnEmployee_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnEmployee);
        }
        private void BtnRegion_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnRegion);
        }
        private void BtnWarehouse_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnWarehouse);
        }
        private void BtnTransport_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnTransport);
        }
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            BtnBackgroundColor(BtnSettings);
            GridSettings.Visibility = Visibility.Visible;
        }
        private void BtnToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            BlackAndWhiteLayout((bool)BtnToggleTheme.IsChecked);
        }

        private void SaveDBSettings()
        {
            Properties.Settings.Default.DBServer = DBServer.Text;
            Properties.Settings.Default.DBData = DBData.Text;
            Properties.Settings.Default.DBUser = DBUser.Text;
            Properties.Settings.Default.DBPassword = DBPassword.Text;
            Properties.Settings.Default.Save();
        }

        private void GetDBSettings()
        {
            DBServer.Text = Properties.Settings.Default.DBServer;
            DBData.Text = Properties.Settings.Default.DBData;
            DBUser.Text = Properties.Settings.Default.DBUser;
            DBPassword.Text = Properties.Settings.Default.DBPassword;
        }

        private void BtnSaveDBSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveDBSettings();
        }


    }
}
