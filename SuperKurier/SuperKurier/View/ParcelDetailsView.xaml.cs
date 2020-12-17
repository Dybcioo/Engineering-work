using DataModel;
using SuperKurier.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
using Caliburn.Micro;
using System.ComponentModel;
using SuperKurier.Control;
using SuperKurier.Enums;

namespace SuperKurier.View
{
    /// <summary>
    /// Logika interakcji dla klasy ParcelDetailsView.xaml
    /// </summary>
    public partial class ParcelDetailsView : Page
    {
        public Parcel Parcel { get; set; }
        public CompanyEntities CompanyEntities { get; set; }
        private int Counter { get; set; } = 0;

        public ParcelDetailsView()
        {
            InitializeComponent();
            CompanyEntities = new CompanyEntities();
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

        private void Page_LayoutUpdated(object sender, EventArgs e)
        {
            if (DataContext == null || ((ParcelDetailsViewModel)DataContext).VisibilityOption == Visibility.Hidden)
            {
                Counter = 0;
                return;
            }
            if (Counter > 1)
                return;
            Counter++;
            var parcelDetailsViewModel = (ParcelDetailsViewModel)DataContext;
            Parcel = CompanyEntities.Parcel.Include(p => p.Region)
                .Include(p => p.Region1)
                .Include(p => p.Tariff)
                .Include(p => p.Customer)
                .Include(p => p.Customer.Address)
                .Include(p => p.Customer.Company)
                .Include(p => p.Customer1)
                .Include(p => p.Customer1.Address)
                .Include(p => p.Customer1.Company)
                .Include(p => p.Status)
                .FirstOrDefault(p => p.id == parcelDetailsViewModel.Parcel.id);

            SenderName.Text = $"{Parcel?.Customer?.firstName ?? ""} {Parcel?.Customer?.lastName ?? ""}";
            SenderCompany.Text = $"{Parcel?.Customer?.Company?.name ?? ""} NIP:{Parcel?.Customer?.Company?.NIP ?? 0}";
            SenderTel.Text = $"{Parcel?.Customer?.tel ?? 0}";
            SenderCountry.Text = $"{Parcel?.Customer?.Address?.country ?? ""}";
            SenderCity.Text = $"{Parcel?.Customer?.Address?.postalCode ?? ""} {Parcel?.Customer?.Address?.city ?? ""}";
            SenderStreet.Text = $"{Parcel?.Customer?.Address?.street ?? ""} {Parcel?.Customer?.Address?.numberOfHouse ?? ""}";

            ReceiverName.Text = $"{Parcel?.Customer1?.firstName ?? ""} {Parcel?.Customer1?.lastName ?? ""}";
            ReceiverCompany.Text = $"{Parcel?.Customer1?.Company?.name ?? ""} NIP:{Parcel?.Customer1?.Company?.NIP ?? 0}";
            ReceiverTel.Text = $"{Parcel?.Customer1?.tel ?? 0}";
            ReceiverCountry.Text = $"{Parcel?.Customer1?.Address?.country ?? ""}";
            ReceiverCity.Text = $"{Parcel?.Customer1?.Address?.postalCode ?? ""} {Parcel?.Customer1?.Address?.city ?? ""}";
            ReceiverStreet.Text = $"{Parcel?.Customer1?.Address?.street ?? ""} {Parcel?.Customer1?.Address?.numberOfHouse ?? ""}";

            ParcelCode.Text = $"{Parcel?.code ?? ""}";
            ParcelDimensions.Text = $"{Parcel?.width ?? 0} x {Parcel?.height?? 0} x {Parcel?.length?? 0}";
            ParcelWeight.Text = $"{Parcel?.weight ?? 0} kg";
            ParcelWorth.Text = $"{Parcel?.amount ?? 0} zł";
            ParcelType.Text = $"{Parcel?.TypeOfParcel.type ?? ""}";
            ParcelMethod.Text = $"{Parcel?.MethodOfSend.method ?? ""}";

            CurrentStatus.Text = $"{CompanyEntities.Status.FirstOrDefault(s => s.id == Parcel.idStatus).status1}";
            DataGridParcelHistory.DataContext = null;
            DataGridParcelHistory.DataContext = new BindableCollection<HistoryOfParcel>(CompanyEntities.HistoryOfParcel.Where(h => h.idParcel == Parcel.id));
            DataGridParcelHistory.Items.Refresh();
        }

        private void BtnChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            var parcelDetailsViewModel = (ParcelDetailsViewModel)DataContext;
            InfoWindow info = new InfoWindow();
            Parcel = CompanyEntities.Parcel
                .Include(p => p.Status)
                .Include(p => p.Region)
                .Include(p => p.Region1)
                .FirstOrDefault(p => p.id == parcelDetailsViewModel.Parcel.id);
            if(Parcel.idStatus >= parcelDetailsViewModel.StatusSelected.id)
            {
                info.ShowInfo("Wybrany status nie może być niższy niż aktualny", "", "Ok");
                info.Close();
                return;
            }
            HistoryOfParcel history = new HistoryOfParcel()
            {
                idParcel = Parcel.id,
                idStatus = Parcel.idStatus,
                date = DateTime.Now
            };
            if (parcelDetailsViewModel.StatusSelected.id > (int)EnumParcelStatus.beetwen)
                history.idWarehouse = Parcel.Region.idWarehouse;
            else if (Parcel.Region1 != null)
                history.idWarehouse = Parcel.Region1.idWarehouse;
            else
                history.idWarehouse = Properties.Settings.Default.Warehouse;
            CompanyEntities.HistoryOfParcel.Add(history);
            Parcel.idStatus = parcelDetailsViewModel.StatusSelected.id;
            if (Parcel.idStatus == (int)EnumParcelStatus.delivered)
                Parcel.dateOfDelivery = DateTime.Now;
            CompanyEntities.SaveChanges();
            Counter = 0;
            AddDoc(Parcel);
            info.ShowInfo("Status został zmieniony pomyślnie", "", "Ok");
            info.Close();
        }

        private void AddDoc(Parcel parcel)
        {
            var parcelDetailsViewModel = (ParcelDetailsViewModel)DataContext;
            Document doc = new Document();
            doc.exposure = true;
            doc.quantity = 1;
            doc.idWarehouse = Properties.Settings.Default.Warehouse;
            doc.idEmployee = Properties.Settings.Default.IdUser;
            doc.summary = parcel.amount;
            doc.code = $"/{DateTime.Now.Month}/{DateTime.Now.Year}";
            int idType = 0;
            switch (parcelDetailsViewModel.StatusSelected.id)
            {
                case (int)EnumParcelStatus.acceptedSender:
                    idType = CompanyEntities.TypeOfDocument.FirstOrDefault(p => p.type.Equals("PZ")).id;
                    break;
                case (int)EnumParcelStatus.beetwen:
                    idType = CompanyEntities.TypeOfDocument.FirstOrDefault(p => p.type.Equals("WZ")).id;
                    break;
                case (int)EnumParcelStatus.acceptedReciver:
                    idType = CompanyEntities.TypeOfDocument.FirstOrDefault(p => p.type.Equals("PZ")).id;
                    break;
                case (int)EnumParcelStatus.handed:
                    idType = CompanyEntities.TypeOfDocument.FirstOrDefault(p => p.type.Equals("WZ")).id;
                    break;
            }
            if (idType == 0)
                return;
            doc.idTypeOfDocument = idType;
            CompanyEntities.Document.Add(doc);
            CompanyEntities.SaveChanges();
            ParcelMoving parcelMoving = new ParcelMoving();
            if (idType == (int)EnumTypeOfDocument.PZ)
            {
                parcelMoving.idDocPZ = doc.id;
                parcelMoving.readingPZ = true;
                parcelMoving.readingWZ = false;
            } 
            else
            {
                parcelMoving.idDocWZ = doc.id;
                parcelMoving.readingWZ = true;
                parcelMoving.readingPZ = false;
            }
            parcelMoving.idParcel = parcel.id;
            doc.code = doc.id + doc.code;
            CompanyEntities.ParcelMoving.Add(parcelMoving);
            CompanyEntities.SaveChanges();
        }

        private void BtnGenerateLabel_Click(object sender, RoutedEventArgs e)
        {
            var parcelDetailsViewModel = (ParcelDetailsViewModel)DataContext;
            parcelDetailsViewModel.GenerateLabel(parcelDetailsViewModel.Parcel.id);
        }
    }
}
