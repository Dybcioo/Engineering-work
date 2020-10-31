using Microsoft.Maps.MapControl.WPF;
using SuperKurier.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using System.Xml;
using System.Xml.Linq;

namespace SuperKurier.View
{
    /// <summary>
    /// Interaction logic for ParceleditView.xaml
    /// </summary>
    public partial class ParceleditView : Page
    {
        public ParceleditView()
        {
            InitializeComponent();
        }

        private int _noOfErrorsOnScreen = 0;

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void ParcelMap_MouseMove(object sender, MouseEventArgs e)
        {
            ParcelScrollViewer.ScrollToVerticalOffset(0D);
        }

        private void ParcelMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ParcelMap.CheckingPushpin(e);
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void SendParcel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void ParcelMap_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu context = new ContextMenu();
            context.IsOpen = true;
            if (_noOfErrorsOnScreen == 0)
            {
                var setPushpins = new MenuItem() { Header = "Wyznacz pinezki na podstawie adresów" };
                setPushpins.Click += (se, e) =>
                {
                    SetPushpins();
                };
                context.Items.Add(setPushpins);
            }
        }

        private void SetPushpins()
        {
            const string MAP_KEY = "4zVzomIhx3FPdd6MwCo5~vFNFUzU_KFebfFMVQu-DXw~AmbZ9wc13wUEvQOdKvmxl-2lFEPUKMFDdttvVqxsnSVH2tnrEyWxsTo2IngDUbXA";        
            StringBuilder senderBuilder = new StringBuilder("http://dev.virtualearth.net/REST/v1/Locations?o=xml");
            StringBuilder receiverBuilder = new StringBuilder("http://dev.virtualearth.net/REST/v1/Locations?o=xml");

            var parcelAddViewModel = (ParcelAddViewModel)DataContext;

            senderBuilder.Append($"&countryRegion={parcelAddViewModel.SenderCountry}");
            senderBuilder.Append($"&locality={parcelAddViewModel.SenderStreet}");
            senderBuilder.Append($"&postalCode={parcelAddViewModel.SenderPostalCode}");
            senderBuilder.Append($"&addressLine={parcelAddViewModel.SenderNumberOfHouse}");
            senderBuilder.Append($"&key={MAP_KEY}");

            PinIt(senderBuilder.ToString(), "Nadawca");

            receiverBuilder.Append($"&countryRegion={parcelAddViewModel.ReceiverCountry}");
            receiverBuilder.Append($"&locality={parcelAddViewModel.ReceiverStreet}");
            receiverBuilder.Append($"&postalCode={parcelAddViewModel.ReceiverPostalCode}");
            receiverBuilder.Append($"&addressLine={parcelAddViewModel.ReceiverNumberOfHouse}");
            receiverBuilder.Append($"&key={MAP_KEY}");

            PinIt(receiverBuilder.ToString(), "Odbiorca");
        }

        private void PinIt(string url, string person)
        {
            var request = WebRequest.Create(url.ToString()) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(response.GetResponseStream());
                    var node = xmlDoc.DocumentElement.LastChild.LastChild.LastChild.FirstChild.LastChild;
                    string latitude = node["Latitude"].InnerText;
                    double latitudeD = Double.Parse(latitude.Replace('.', ','));
                    string longitude = node["Longitude"].InnerText;
                    double longitudeD = Double.Parse(longitude.Replace('.', ','));
                    var location = new Location() { Latitude = latitudeD, Longitude = longitudeD };
                    ParcelMap.PinPushpinWithName(location, person);
                    ParcelMap.Center = location;
                    ParcelMap.ZoomLevel = 14;
                }
            }
        }
    }
}
