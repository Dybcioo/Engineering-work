using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using DataModel;

namespace SuperKurier
{
    static class Region
    {
        private static int counter = 1;
        private static Map map;

        public static void ClearPolyline(this Map MyMap)
        {
            foreach (var child in MyMap.Children)
            {
                if (child is MapPolyline)
                {
                    MyMap.Children.Remove((MapPolyline)child);
                    break;
                }
            }
            MyMap.ClearTextInMap();
        }

        public static void ClearTextInMap(this Map MyMap)
        {
            List<MapLayer> mapLayers = new List<MapLayer>();
            foreach (var child in MyMap.Children)
            {
                if (child is MapLayer)
                {
                    mapLayers.Add((MapLayer)child);
                }
            }
            foreach (var ml in mapLayers)
            {
                MyMap.Children.Remove(ml);
            }
        }

        public static void ClearAllMap(this Map MyMap)
        {
            List<UIElement> mapLayers = new List<UIElement>();
            foreach (var child in MyMap.Children)
            {
                mapLayers.Add((UIElement)child);
            }
            foreach (var ml in mapLayers)
            {
                MyMap.Children.Remove(ml);
            }
            counter = 1;
        }

        public static void ConnectPushpins(this Map MyMap)
        {
            MyMap.ClearPolyline();
            List<Pushpin> pins = MyMap.GetPushpins();
            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new SolidColorBrush(Colors.Orange);
            polyline.StrokeThickness = 2;
            polyline.Opacity = 0.7;
            LocationCollection locations = new LocationCollection();
            foreach (var pin in pins)
            {
                locations.Add(pin.Location);
            }
            polyline.Locations = locations;
            MyMap.Children.Add(polyline);
            MyMap.ShowDistance(polyline);
        }

        public static void DrawSquare(this Map MyMap, DataModel.Localization startLocal, DataModel.Localization endLocal)
        {
            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new SolidColorBrush(Colors.Blue);
            polyline.StrokeThickness = 2;
            polyline.Opacity = 0.7;
            LocationCollection locations = new LocationCollection();
            locations.Add(new Location() { Latitude = double.Parse(startLocal.latitude),   Longitude = double.Parse(startLocal.longitude)});
            locations.Add(new Location() { Latitude = double.Parse(endLocal.latitude),     Longitude = double.Parse(startLocal.longitude)});
            locations.Add(new Location() { Latitude = double.Parse(endLocal.latitude),     Longitude = double.Parse(endLocal.longitude)});
            locations.Add(new Location() { Latitude = double.Parse(startLocal.latitude),   Longitude = double.Parse(endLocal.longitude)});
            locations.Add(new Location() { Latitude = double.Parse(startLocal.latitude),   Longitude = double.Parse(startLocal.longitude) });
            polyline.Locations = locations;
            MyMap.Children.Add(polyline);
        }

        public static bool IsAllowRegion(this Map MyMap, Location startLocal, Location endLocal, CompanyEntities companyEntities, int regionId = 0)
        {
            var regions = companyEntities.Region.ToList();
            var localizations = companyEntities.Localization.ToList();
            foreach (var region in regions)
            {
                if (region.id == regionId)
                    continue;
                var startTmp = localizations.Find(l => l.id == region.idStartLocalization);
                var endTmp = localizations.Find(l => l.id == region.idEndLocalization);
                var startLocalTmp = new Location() { Latitude = double.Parse(startTmp.latitude), Longitude = double.Parse(startTmp.longitude) };
                var endLocalTmp = new Location() { Latitude = double.Parse(endTmp.latitude), Longitude = double.Parse(endTmp.longitude) };
                if (startLocalTmp.Latitude < endLocalTmp.Latitude)
                {
                    if (startLocal.Latitude < startLocalTmp.Latitude && endLocal.Latitude > startLocalTmp.Latitude)
                        return CheckLonglitude(startLocal, endLocal, startLocalTmp, endLocalTmp);
                    if (startLocal.Latitude > startLocalTmp.Latitude && startLocal.Latitude < endLocalTmp.Latitude)
                        return CheckLonglitude(startLocal, endLocal, startLocalTmp, endLocalTmp);
                    if(startLocal.Latitude > endLocalTmp.Latitude && endLocal.Latitude < endLocalTmp.Latitude)
                        return CheckLonglitude(startLocal, endLocal, startLocalTmp, endLocalTmp);
                }
                else
                {
                    if (startLocal.Latitude > startLocalTmp.Latitude && endLocal.Latitude < startLocalTmp.Latitude)
                        return CheckLonglitude(startLocal, endLocal, startLocalTmp, endLocalTmp);
                    if (startLocal.Latitude < startLocalTmp.Latitude && startLocal.Latitude > endLocalTmp.Latitude)
                        return CheckLonglitude(startLocal, endLocal, startLocalTmp, endLocalTmp);
                    if (startLocal.Latitude < endLocalTmp.Latitude && endLocal.Latitude > endLocalTmp.Latitude)
                        return CheckLonglitude(startLocal, endLocal, startLocalTmp, endLocalTmp);
                }
            }
            return true;
        }     

        public static DataModel.Region GetCurrentRegion(this Map MyMap, Location location, CompanyEntities companyEntities)
        {
            var regions = companyEntities.Region.ToList();
            var localizations = companyEntities.Localization.ToList();
            foreach (var region in regions)
            {
                var startTmp = localizations.Find(l => l.id == region.idStartLocalization);
                var endTmp = localizations.Find(l => l.id == region.idEndLocalization);
                var startLocalTmp = new Location() { Latitude = double.Parse(startTmp.latitude), Longitude = double.Parse(startTmp.longitude) };
                var endLocalTmp = new Location() { Latitude = double.Parse(endTmp.latitude), Longitude = double.Parse(endTmp.longitude) };
                if (startLocalTmp.Latitude < endLocalTmp.Latitude)
                {
                    if (location.Latitude > startLocalTmp.Latitude && location.Latitude < endLocalTmp.Latitude && CheckCurrentRegion(location, startLocalTmp, endLocalTmp))
                        return region;
                }
                else
                {
                    if (location.Latitude < startLocalTmp.Latitude && location.Latitude > endLocalTmp.Latitude && CheckCurrentRegion(location, startLocalTmp, endLocalTmp))
                        return region;
                }
            }
            return null;
        }

        public static void ShowDistance(this Map MyMap, MapPolyline mapPolyline)
        {
            var geo1 = new GeoCoordinate();
            var geo2 = new GeoCoordinate();
            int i = 0;
            double km = 0;
            foreach (var location in mapPolyline.Locations)
            {
                if (i < 1)
                {
                    geo2.Latitude = location.Latitude;
                    geo2.Longitude = location.Longitude;
                }
                else
                {
                    geo1.Latitude = geo2.Latitude;
                    geo1.Longitude = geo2.Longitude;
                    geo2.Latitude = location.Latitude;
                    geo2.Longitude = location.Longitude;
                    km += geo1.GetDistanceTo(geo2);
                    var child = new MapLayer();
                    child.AddChild(new TextBlock() { Text = String.Format("{0:0.##}", km / 1000) + "km", Foreground = Brushes.Red }, location);
                    MyMap.Children.Add(child);
                }
                i++;
            }
        }

        public static List<Pushpin> GetPushpins(this Map MyMap)
        {
            List<Pushpin> pins = new List<Pushpin>();
            foreach (var child in MyMap.Children)
            {
                if (child is Pushpin)
                {
                    pins.Add((Pushpin)child);
                }
            }
            return pins;
        }

        public static void CheckingPushpin(this Map MyMap, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Point pt = Mouse.GetPosition(MyMap);
                Location lt = MyMap.ViewportPointToLocation(pt);
                Pushpin pin = new Pushpin();
                pin.Location = lt;
                pin.Content = counter;
                map = MyMap;
                pin.MouseDown += new MouseButtonEventHandler(Pin_MouseDown);
                counter++;
                MyMap.Children.Add(pin);
            }
        }

        public static MapPolyline GetPolyline(this Map MyMap, DataModel.Region region)
        {
            foreach (var child in MyMap.Children)
            {
                if (child is MapPolyline)
                {
                    var polyline = (MapPolyline)child;
                    if (polyline.Locations.Any(l => l.Latitude == double.Parse(region.Localization.latitude) && l.Longitude == double.Parse(region.Localization.longitude)))
                        return polyline;
                }
                    
            }
            return null;
        }

        private static bool CheckLonglitude(Location startLocal, Location endLocal, Location startLocalTmp, Location endLocalTmp)
        {
            if (startLocalTmp.Longitude < endLocalTmp.Longitude)
            {
                if (startLocal.Longitude < startLocalTmp.Longitude && endLocal.Longitude > startLocalTmp.Longitude)
                    return false;
                if (startLocal.Longitude > startLocalTmp.Longitude && startLocal.Longitude < endLocalTmp.Longitude)
                    return false;
                if (startLocal.Longitude > endLocalTmp.Longitude && endLocal.Longitude < endLocalTmp.Longitude)
                    return false;
            }
            else
            {
                if (startLocal.Longitude > startLocalTmp.Longitude && endLocal.Longitude < startLocalTmp.Longitude)
                    return false;
                if (startLocal.Longitude < startLocalTmp.Longitude && startLocal.Longitude > endLocalTmp.Longitude)
                    return false;
                if (startLocal.Longitude < endLocalTmp.Longitude && endLocal.Longitude > endLocalTmp.Longitude)
                    return false;
            }
            return true;
        }

        private static bool CheckCurrentRegion(Location location, Location startLocalTmp, Location endLocalTmp)
        {
            if (startLocalTmp.Longitude < endLocalTmp.Longitude)
            {
                if (location.Longitude > startLocalTmp.Longitude && location.Longitude < endLocalTmp.Longitude)
                    return true;
            }
            else
            {
                if (location.Longitude < startLocalTmp.Longitude && location.Longitude > endLocalTmp.Longitude)
                    return true;
            }
            return false;
        }

        private static void Pin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Pushpin pin = (Pushpin)sender;
                List<Pushpin> pins = map.GetPushpins();
                for (int i = (int)pin.Content; i < counter; i++)
                {
                    foreach (var pinn in pins)
                    {
                        if (pinn.Content.Equals(i))
                        {
                            pinn.Content = i - 1;
                            break;
                        }
                    }
                }
                counter--;
                map.Children.Remove(pin);
                if (map.IsPolyline())
                    map.ConnectPushpins();
            }
        }

        private static bool IsPolyline(this Map MyMap)
        {
            foreach(var child in MyMap.Children)
            {
                if (child is MapPolyline)
                    return true;
            }
            return false;
        }

    }
}
