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
        public Color colorBtn;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            BlackAndWhiteLayout();
        }
        private void BlackAndWhiteLayout(bool black = true)
        {
            if (black)
            {
                BackgroundOption = "Black";
                ForegroundOption = "White";
                colorBtn = Color.FromArgb(100, 104, 104, 104);
            }
            else
            {
                BackgroundOption = "White";
                ForegroundOption = "Black";
                colorBtn = Color.FromArgb(100, 193, 193, 193);
            }
        }
        private void BtnBackgroundColor(Button btn)
        {
            BtnParcel.Background = Brushes.Transparent;
            BtnEmployee.Background = Brushes.Transparent;
            BtnRegion.Background = Brushes.Transparent;
            BtnWarehouse.Background = Brushes.Transparent;
            BtnTransport.Background = Brushes.Transparent;
            BtnSettings.Background = Brushes.Transparent;
            btn.Background = new SolidColorBrush(colorBtn);
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
        }
    }
}
