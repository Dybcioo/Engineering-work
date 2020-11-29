using DataModel;
using SuperKurier.Control;
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
using System.Data.Entity;
using SuperKurier.ViewModel;

namespace SuperKurier.View.FWarehouse
{
    /// <summary>
    /// Logika interakcji dla klasy WarehouseAddView.xaml
    /// </summary>
    public partial class WarehouseAddView : Page
    {
        public CompanyEntities companyEntities { get; set; }
        public List<Parcel> Parcel { get; set; }
        private int reload = 0;
        public WarehouseAddView()
        {
            InitializeComponent();
            Parcel = new List<Parcel>();
            WarehouseGrid.DataContext = Parcel;
            companyEntities = new CompanyEntities();
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
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
            var doc = UpdateBuffer();
            var info = new InfoWindow();
            info.ShowInfo($"Dokument {doc.code} pozostawiony w buforze!", "PZ", "Ok");
            info.Close();
            Close();
        }

        private void Btn_PutOut_Click(object sender, RoutedEventArgs e)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            var doc = UpdateBuffer();
            doc = companyEntities.Document.FirstOrDefault(d => d.id == doc.id);
           double summary = 0.0;
           foreach(var d in companyEntities.ParcelMoving.Include(p => p.Parcel).Where(p => p.idDoc == doc.id))
            {
                d.readingPZ = true;
                summary += (double)d.Parcel.amount;
            }
            doc.summary = (decimal)summary;
            doc.exposure = true;
            companyEntities.SaveChanges();
            var info = new InfoWindow();
            info.ShowInfo($"Dokument {doc.code} został wystawiony pomyślnie!", "PZ", "Ok");
            info.Close();
            Close();
        }

        private void BtnTrashBin_Click(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var temp = (Parcel)row.Item;
                    Parcel.Remove(temp);
                    break;
                }
            var addViewModel = (WarehouseAddViewModel)DataContext;
            WarehouseGrid.DataContext = null;
            WarehouseGrid.DataContext = Parcel;
            addViewModel.UpdateParcelList(Parcel);
        }

        private Document UpdateBuffer()
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
                        readingPZ = false,
                        readingWZ = false
                    };
                    companyEntities.ParcelMoving.Add(temp);
                }
                companyEntities.SaveChanges();
            }
            else
            {
                var actuallyList = addViewModel.actuallyParcelList;
                foreach (var rem in actuallyList)
                {
                    if (Parcel.Any(p => p.id == rem.id))
                        continue;
                    var temp = companyEntities.ParcelMoving.FirstOrDefault(p => p.idParcel == rem.id);
                    companyEntities.ParcelMoving.Remove(temp);
                }
                foreach (var p in Parcel)
                {
                    if (actuallyList.Any(parcel => parcel.id == p.id))
                        continue;
                    var temp = new ParcelMoving()
                    {
                        idDoc = document.id,
                        idParcel = p.id,
                        readingPZ = false,
                        readingWZ = false
                    };
                    companyEntities.ParcelMoving.Add(temp);
                }
                companyEntities.SaveChanges();
                document = companyEntities.Document.FirstOrDefault(d => d.id == document.id);
                document.quantity = companyEntities.ParcelMoving.Where(p => p.idDoc == document.id).ToList().Count;
                companyEntities.SaveChanges();
            }
            return document;
        }

        private void freezePage(bool freeze)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            BtnAddParcel.IsEnabled = freeze;
            BtnBuffer.IsEnabled = freeze;
            BtnPutOut.IsEnabled = freeze;
            ParcelCombo.IsEnabled = freeze;
            BtnDelete.IsEnabled = freeze;
            WarehouseGrid.Columns.FirstOrDefault(c => c.Header.Equals("Opcje")).Visibility = freeze ? Visibility.Visible : Visibility.Hidden;
        }

        private void Close()
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            Parcel = new List<Parcel>();
            WarehouseGrid.DataContext = null;
            addViewModel.VisibilityOption = Visibility.Hidden;
        }

        private void Page_LayoutUpdated(object sender, EventArgs e)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            if (addViewModel.VisibilityOption == Visibility.Hidden)
            {
                reload = 0;
                return;
            }
            if (reload > 0)
                return;
            if (addViewModel.actuallyParcelList != null)
            {
                Parcel = new List<Parcel>();
                foreach (var act in addViewModel.actuallyParcelList)
                    Parcel.Add(act);
            }
            WarehouseGrid.DataContext = null;
            WarehouseGrid.DataContext = Parcel;
            addViewModel.UpdateParcelList(Parcel);
            var doc = addViewModel.Document;
            if (doc != null && doc.exposure)
                freezePage(false);
            else
                freezePage(true);
            reload++;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var addViewModel = (WarehouseAddViewModel)DataContext;
            var document = addViewModel.Document;
            if (document == null)
                return;
            foreach (var parcel in companyEntities.ParcelMoving.Where(p => p.idDoc == document.id))
                companyEntities.ParcelMoving.Remove(parcel);
            companyEntities.Document.Remove(companyEntities.Document.FirstOrDefault(d => d.id == document.id));
            companyEntities.SaveChanges();
            var info = new InfoWindow();
            info.ShowInfo($"Dokument {document.code} został usunięty pomyślnie!", "PZ", "Ok");
            info.Close();
            Close();
        }
    }
}
