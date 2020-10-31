using Microsoft.Maps.MapControl.WPF;
using SuperKurier.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
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
                    ParcelMap.ClearAllMap();
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

            var from = PinIt(senderBuilder.ToString(), "Nadawca");

            receiverBuilder.Append($"&countryRegion={parcelAddViewModel.ReceiverCountry}");
            receiverBuilder.Append($"&locality={parcelAddViewModel.ReceiverStreet}");
            receiverBuilder.Append($"&postalCode={parcelAddViewModel.ReceiverPostalCode}");
            receiverBuilder.Append($"&addressLine={parcelAddViewModel.ReceiverNumberOfHouse}");
            receiverBuilder.Append($"&key={MAP_KEY}");

            var to = PinIt(receiverBuilder.ToString(), "Odbiorca");

            string uri = $"http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={from.Latitude},{from.Longitude}&wp.1={to.Latitude},{to.Longitude}&rpo=Points&key={MAP_KEY}";
            Route(DriveRoute(uri));
        }

        private Location PinIt(string url, string person)
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
                    return location;
                }
                return null;
            }
        }

        private BingMapsRESTService.Common.JSON.Response DriveRoute(string uri)
        {
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(BingMapsRESTService.Common.JSON.Response));

                    return ser.ReadObject(stream) as BingMapsRESTService.Common.JSON.Response;
                }
            }
        }

        private void Route(BingMapsRESTService.Common.JSON.Response r)
        {
            if (r != null &&
                r.ResourceSets != null &&
                r.ResourceSets.Length > 0 &&
                r.ResourceSets[0].Resources != null &&
                r.ResourceSets[0].Resources.Length > 0)
            {
                BingMapsRESTService.Common.JSON.Route route = r.ResourceSets[0].Resources[0] as BingMapsRESTService.Common.JSON.Route;

                double[][] routePath = route.RoutePath.Line.Coordinates;
                LocationCollection locs = new LocationCollection();

                for (int i = 0; i < routePath.Length; i++)
                {
                    if (routePath[i].Length >= 2)
                    {
                        locs.Add(new Microsoft.Maps.MapControl.WPF.Location(routePath[i][0], routePath[i][1]));
                    }
                }

                MapPolyline routeLine = new MapPolyline()
                {
                    Locations = locs,
                    Stroke = new SolidColorBrush(Colors.Blue),
                    StrokeThickness = 5
                };

                ParcelMap.Children.Add(routeLine);

                ParcelMap.SetView(locs, new Thickness(30), 0);
            }
        }
    }
}
