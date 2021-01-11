using DataModel;
using Microsoft.Maps.MapControl.WPF;
using SuperKurier.Control;
using SuperKurier.Enums;
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
        private const string MAP_KEY = "4zVzomIhx3FPdd6MwCo5~vFNFUzU_KFebfFMVQu-DXw~AmbZ9wc13wUEvQOdKvmxl-2lFEPUKMFDdttvVqxsnSVH2tnrEyWxsTo2IngDUbXA";
        public Location From { get; set; }
        public Location To { get; set; }
        private bool Lock { get; set; }
        private CompanyEntities companyEntities = new CompanyEntities();

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

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((ParcelAddViewModel)DataContext).VisibilityOption = Visibility.Hidden;
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
            if (_noOfErrorsOnScreen == 0 && !Lock)
            {
                var setPushpins = new MenuItem() { Header = "Wyznacz pinezki na podstawie adresów" };
                setPushpins.Click += (se, e) =>
                {
                    ParcelMap.ClearAllMap();
                    SetPushpins();
                };
                context.Items.Add(setPushpins);

                var setManualPushpins = new MenuItem() { Header = "Dodaj pinezki samodzielnie" };
                setManualPushpins.Click += (se, e) =>
                {
                    ParcelMap.ClearAllMap();
                    if (From != null)
                        ParcelMap.PinPushpinWithName(From, "Nadawca");
                    if (To != null)
                        ParcelMap.PinPushpinWithName(To, "Odbiorca");
                    SetManualPushpinsAsync();
                };
                context.Items.Add(setManualPushpins);
            }
        }

        private async void SetManualPushpinsAsync()
        {
            Lock = true;
            InfoWindow info = new InfoWindow();
            string sender = "Nadawca";
            string receiver = "Odbiorca";
            if (From != null)
            {
                info.ShowInfo("Czy chcesz zmienić lokalizacje nadawcy?", "Zmiana lokalizacji", "Nie", "Tak");
                if (info.Answer)
                {
                    info.ShowInfo("Dodaj lokalizację nadawcy poprzez dwukrotne kliknięcie na mapie!", "Dodaj pinezkę", "Ok");
                    await ParcelMap.PinPushpinWhenClicked(true, sender);
                }
            }
            else
            {
                info.ShowInfo("Dodaj lokalizację nadawcy poprzez dwukrotne kliknięcie na mapie!", "Dodaj pinezkę", "Ok");
                await ParcelMap.PinPushpinWhenClicked(false, sender);
            }

            if (To != null)
            {
                info.ShowInfo("Czy chcesz zmienić lokalizacje odbiorcy?", "Zmiana lokalizacji", "Nie", "Tak");
                if (info.Answer)
                {
                    info.ShowInfo("Dodaj lokalizację odbiorcy poprzez dwukrotne kliknięcie na mapie!", "Dodaj pinezkę", "Ok");
                    await ParcelMap.PinPushpinWhenClicked(true, receiver);
                }
            }
            else
            {
                info.ShowInfo("Dodaj lokalizację odbiorcy poprzez dwukrotne kliknięcie na mapie!", "Dodaj pinezkę", "Ok");
                await ParcelMap.PinPushpinWhenClicked(false, receiver);
            }

            var pushpins = ParcelMap.GetPushpins();

            From = pushpins.FirstOrDefault(p => p.Content.Equals(sender)).Location;
            To = pushpins.FirstOrDefault(p => p.Content.Equals(receiver)).Location;

            if(From == null || To == null)
            {
                info.ShowInfo("Ze względu na brak jednej z lokalizacji nie można wyznaczyć trasy.\nProszę powtórz procedurę dodawania pinezek.", "Błąd wyznaczania trasy", "Ok");
                info.Close();
                Lock = false;
                return;
            }
            string uri = $"http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={From.Latitude},{From.Longitude}&wp.1={To.Latitude},{To.Longitude}&rpo=Points&key={MAP_KEY}";

            var response = DriveRoute(uri);
            if (response != null)
            {
                Route(response);
                SetDistanceAndDuration();
            }
            else
                info.ShowInfo("Nie można wyznaczyć trasy. Proszę spróbować jeszcze raz.", "Błąd wyznaczania trasy", "Ok");
            
            info.Close();
            Lock = false;
        }

        private void SetPushpins()
        {
            StringBuilder senderBuilder = new StringBuilder("http://dev.virtualearth.net/REST/v1/Locations?o=xml");
            StringBuilder receiverBuilder = new StringBuilder("http://dev.virtualearth.net/REST/v1/Locations?o=xml");
            var parcelAddViewModel = (ParcelAddViewModel)DataContext;
            senderBuilder.Append($"&countryRegion={parcelAddViewModel.SenderCountry}");
            senderBuilder.Append($"&locality={parcelAddViewModel.SenderCity}");
            senderBuilder.Append($"&postalCode={parcelAddViewModel.SenderPostalCode}");
            senderBuilder.Append($"&addressLine={parcelAddViewModel.SenderStreet} {parcelAddViewModel.SenderNumberOfHouse}");
            senderBuilder.Append($"&key={MAP_KEY}");
            From = PinIt(senderBuilder.ToString(), "Nadawca");
            receiverBuilder.Append($"&countryRegion={parcelAddViewModel.ReceiverCountry}");
            receiverBuilder.Append($"&locality={parcelAddViewModel.ReceiverCity}");
            receiverBuilder.Append($"&postalCode={parcelAddViewModel.ReceiverPostalCode}");
            receiverBuilder.Append($"&addressLine={parcelAddViewModel.ReceiverStreet} {parcelAddViewModel.ReceiverNumberOfHouse}");
            receiverBuilder.Append($"&key={MAP_KEY}");
            To = PinIt(receiverBuilder.ToString(), "Odbiorca");
            string uri = $"http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0=" +
                $"{From.Latitude},{From.Longitude}&wp.1={To.Latitude},{To.Longitude}&rpo=Points&key={MAP_KEY}";
            var response = DriveRoute(uri);
            if (response != null)
            {
                Route(response);
                SetDistanceAndDuration();
            }else
            {
                InfoWindow info = new InfoWindow();
                info.ShowInfo("Nie można wyznaczyć trasy. Proszę spróbować jeszcze raz.", "Błąd wyznaczania trasy", "Ok");
                info.Close();
            }
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
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(BingMapsRESTService.Common.JSON.Response));

                        return ser.ReadObject(stream) as BingMapsRESTService.Common.JSON.Response;
                    }
                }
            }catch(Exception e) { }

            return null;
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
                var parcelAddViewModel = (ParcelAddViewModel)DataContext;
                parcelAddViewModel.ParcelDistance = route.TravelDistance;
                parcelAddViewModel.ParcelDuration = route.TravelDuration;
            }
        }

        private void SetDistanceAndDuration()
        {
            var parcelAddViewModel = (ParcelAddViewModel)DataContext;

            double distance = parcelAddViewModel.ParcelDistance;

            if (distance != 0)
                ParcelDistance.Content = $"{distance} km";

            double duration = parcelAddViewModel.ParcelDuration;

            if (duration == 0)
                return;
            int hours = (int)duration / 3600;
            int minutes = (int)(duration - (hours * 3600)) / 60;

            string hoursString = "";
            string minutesString = "";

            hoursString = hours < 10 ? $"0{hours}" : hours.ToString();
            minutesString = minutes < 10 ? $"0{minutes}" : minutes.ToString();

            ParcelDuration.Content = $"{hoursString}:{minutesString} h";
        }

        private void calculateCost_Click(object sender, RoutedEventArgs e)
        {
            CalculateCost();
        }

        private bool CalculateCost()
        {
            var parcelAddViewModel = (ParcelAddViewModel)DataContext;
            InfoWindow info = new InfoWindow();
            if (Lock || parcelAddViewModel.ParcelDistance == 0)
            {
                
                if (_noOfErrorsOnScreen == 0)
                {
                    info.ShowInfo("Nie można obliczyć kosztu dopóki nie zostanie wyznaczona trasa.\nCzy wyznaczyć trasę automatycznie" +
                        " na podstawie adresów?", "Błąd wyliczenia kosztów", "Nie", "Tak");
                }
                else
                {
                    info.ShowInfo("Nie można obliczyć kosztu dopóki nie zostanie wyznaczona trasa." +
                        "\nNie można wyznaczyć trasy dopóki nie zostaną wprowadzone wymagane dane", "Błąd wyliczenia kosztów", "Ok");
                    info.Close();
                    return false;
                }
                if (info.Answer)
                    SetPushpins();
                else
                {
                    info.Close();
                    return false;
                }
            }
            double dimensions = double.Parse(parcelAddViewModel.ParcelHeight) + double.Parse(parcelAddViewModel.ParcelLength) + double.Parse(parcelAddViewModel.ParcelWidth);
            dimensions *= 0.1;
            double distance = parcelAddViewModel.ParcelDistance * 0.01;
            double weight = double.Parse(parcelAddViewModel.ParcelWeight) * 0.1;
            double worth = double.Parse(parcelAddViewModel.ParcelWorth) * 0.001;

            int result = (int)(dimensions + distance + weight + worth);

            switch(parcelAddViewModel.ParcelTypeSelected.id)
            {
                case (int)EnumTypeOfParcel.StandardParcel:
                    break;
                case (int)EnumTypeOfParcel.UnstandardParcel:
                    result += (int)(result * 0.1);
                    break;
            }

            var myTariff = companyEntities.Tariff.FirstOrDefault(t => result >= t.valueFrom && result <= t.valueTo);

            if (myTariff == null)
            {
                info.ShowInfo("Nie można obliczyć kosztu!", "Błąd wyliczenia kosztów", "Ok");
                info.Close();
                return false;
            }
            parcelAddViewModel.MyTariff = myTariff;
            if(parcelAddViewModel.ParcelTypeSelected.id == (int)EnumTypeOfParcel.CashOnDelivery)
                Worth.Text = (myTariff.cost + 10).ToString();
            else
                Worth.Text = myTariff.cost.ToString();
            return true;
        }

        private void SendParcel_Click(object sender, RoutedEventArgs e)
        {
            var parcelAddViewModel = (ParcelAddViewModel)DataContext;
            InfoWindow info = new InfoWindow();
            if (!CalculateCost())
                return;

            DataModel.Region senderRegion = null;
            if(parcelAddViewModel.ParcelSendMethodSelected.id == 2)
            {
                senderRegion = ParcelMap.GetCurrentRegion(From, companyEntities);
                if(senderRegion == null)
                {
                    info.ShowInfo("Lokalizacja nadania przesyłki znajduje się w nieobsługiwanym regionie!", "Nadanie przesyłki", "Ok");
                    return;
                }
            }
            DataModel.Region receiverRegion = null;
            receiverRegion = ParcelMap.GetCurrentRegion(To, companyEntities);
            if(receiverRegion == null)
            {
                info.ShowInfo("Lokalizacja odbioru przesyłki znajduje się w nieobsługiwanym regionie!", "Nadanie przesyłki", "Ok");
                return;
            }

            if (parcelAddViewModel.ParcelTypeSelected.id == (int)EnumTypeOfParcel.CashOnDelivery)
                info.ShowInfo($"Przesyłka została wyceniona na kwotę {parcelAddViewModel.MyTariff.cost + 10} zł. Chcesz nadać przesyłkę?", "Nadanie przeyłki", "Nie", "Tak");
            else
                info.ShowInfo($"Przesyłka została wyceniona na kwotę {parcelAddViewModel.MyTariff.cost} zł. Chcesz nadać przesyłkę?", "Nadanie przeyłki", "Nie", "Tak");

            if (!info.Answer)
                return;

            if (parcelAddViewModel.SendParcel(senderRegion, receiverRegion, From, To, (bool)generateLabel.IsChecked, (bool)generateConfirmate.IsChecked))
            {
                info.ShowInfo("Przesyłka została nadana!", "Nadanie przesyłki", "Ok");
                ParcelMap.ClearAllMap();
                Worth.Text = "0,00 zł";
                generateLabel.IsChecked = false;
                generateConfirmate.IsChecked = false;
                parcelAddViewModel.VisibilityOption = Visibility.Hidden;
            }
            else
                info.ShowInfo("Nie udało się nadać przesyłki!", "Nadanie przesyłki", "Ok");
        }
    }
}
