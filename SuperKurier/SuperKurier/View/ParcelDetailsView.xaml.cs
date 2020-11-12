using SuperKurier.ViewModel;
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

namespace SuperKurier.View
{
    /// <summary>
    /// Logika interakcji dla klasy ParcelDetailsView.xaml
    /// </summary>
    public partial class ParcelDetailsView : Page
    {
        public ParcelDetailsView()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var parcelDetailsViewModel = (ParcelDetailsViewModel)DataContext;
            MessageBox.Show(parcelDetailsViewModel.Parcel.code);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((ParcelDetailsViewModel)DataContext).VisibilityOption = Visibility.Hidden;
        }
    }
}
