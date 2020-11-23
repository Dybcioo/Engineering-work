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
        public CompanyEntities companyEntities { get; set; }
        public List<Parcel> Parcel { get; set; }
        public WarehouseAddView()
        {
            InitializeComponent();
            Parcel = new List<Parcel>();
            WarehouseGrid.DataContext = Parcel;
            companyEntities = new CompanyEntities();
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

        private void Btn_Buffer_Click(object sender, RoutedEventArgs e)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            var document = addViewModel.Document;
            if (document == null)
            {
                document = new Document();
                document.idTypeOfDocument = companyEntities.TypeOfDocument.FirstOrDefault(p => p.type.Equals("PZ")).id;
                document.idWarehouse = Properties.Settings.Default.Warehouse;
                document.code = $"/{DateTime.Now.Month}/{DateTime.Now.Year}";
                document.quantity = Parcel.Count;
                document.exposure = false;
                companyEntities.Document.Add(document);
                companyEntities.SaveChanges();
                document.code = document.id + document.code;

                foreach (var p in Parcel)
                {
                    var temp = new ParcelMoving()
                    {
                        idDoc = document.id,
                        idParcel = p.id,
                        reading = false
                    };
                    companyEntities.ParcelMoving.Add(temp);
                }
                companyEntities.SaveChanges();
            }
            else
            {
                
            }
        }

        private void Btn_PutOut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            if (addViewModel.actuallyParcelList != null)
                Parcel = addViewModel.actuallyParcelList;
            WarehouseGrid.DataContext = null;
            WarehouseGrid.DataContext = Parcel;
        }
    }
}
