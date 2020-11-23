using DataModel;
using SuperKurier.ViewModel.FWarehouse;
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

namespace SuperKurier.View.FWarehouse
{
    /// <summary>
    /// Logika interakcji dla klasy WarehouseAddView.xaml
    /// </summary>
    public partial class WarehouseAddView : Page
    {
        public List<Parcel> Parcel { get; set; }
        public WarehouseAddView()
        {
            InitializeComponent();
            Parcel = new List<Parcel>();
            WarehouseGrid.DataContext = Parcel;
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((WarehouseAddViewModel)DataContext).VisibilityOption = Visibility.Hidden;
        }

        private void AddParcelToGrid_Click(object sender, RoutedEventArgs e)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            var parcel = addViewModel.ParcelSelected;
            if (parcel != null && parcel.id != 0)
                Parcel.Add(parcel);
            addViewModel.UpdateParcelList(Parcel);
            WarehouseGrid.DataContext = null;
            WarehouseGrid.DataContext = Parcel;
        }
    }
}
