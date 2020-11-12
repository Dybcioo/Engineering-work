using Caliburn.Micro;
using DataModel;
using SuperKurier.Control;
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
using System.Linq.Expressions;
using System.Data.Entity;

namespace SuperKurier.View
{
    /// <summary>
    /// Interaction logic for ParcelView.xaml
    /// </summary>
    public partial class ParcelView : Page
    {

        CompanyEntities companyEntities;


        public ParcelView()
        {
            InitializeComponent();

            this.companyEntities = new CompanyEntities();
            gridDataInitialize();
        }

        private void gridDataInitialize()
        {
            try
            {
                DataGridParcel.DataContext = new BindableCollection<Parcel>(companyEntities.Parcel.ToList());
            }
            catch (Exception ex)
            {
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnSearchParcel_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = SearchParcel.Text;
            search.ToUpper();
            var parcels = companyEntities.Parcel.Where(p => p.code.ToUpper().Contains(search)).ToList();
            DataGridParcel.DataContext = new BindableCollection<Parcel>(parcels);
        }

        private void BtnParcelAdd_Click(object sender, RoutedEventArgs e)
        {
            ((ParcelViewModel)DataContext).AddViewModel = new ParcelAddViewModel() { VisibilityOption = Visibility.Visible };
            frame.Visibility = Visibility;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var parcels = companyEntities.Parcel.Include(p => p.Region).ToList();
            var parcelModel = (ParcelViewModel)DataContext;
            if (parcelModel.TypeOfParcelSelected.id != 0)
                parcels = parcels.Where(p => p.idTypeOfParcel == parcelModel.TypeOfParcelSelected.id).ToList();
            if (parcelModel.StatusSelected.id != 0)
                parcels = parcels.Where(p => p.idStatus == parcelModel.StatusSelected.id).ToList();
            if (parcelModel.WarehouseSelected.id != 0)
                parcels = parcels.Where(p => p.Region.idWarehouse == parcelModel.WarehouseSelected.id).ToList();
            if (dateFrom.SelectedDate != null)
                parcels = parcels.Where(p => p.dateOfShipment.Date >= dateFrom.SelectedDate).ToList();
            if (dateTo.SelectedDate != null)
                parcels = parcels.Where(p => p.dateOfShipment.Date <= dateTo.SelectedDate).ToList();

            DataGridParcel.DataContext = new BindableCollection<Parcel>(parcels);
        }
    }
}
