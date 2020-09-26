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

        public static void ConnectPushpins(this Map MyMap)
        {
            MyMap.ClearPolyline();
            List<Pushpin> pins = MyMap.GetPushpins();
            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new SolidColorBrush(Colors.Blue);
            polyline.StrokeThickness = 5;
            polyline.Opacity = 0.7;
            LocationCollection locations = new LocationCollection();
            foreach (var pin in pins)
            {
                locations.Add(pin.Location);
            }
            polyline.Locations = locations;
            MyMap.Children.Add(polyline);
            MyMap.ShowDistance();
        }

        public static void ShowDistance(this Map MyMap)
        {
            var geo1 = new GeoCoordinate();
            var geo2 = new GeoCoordinate();
            int i = 0;
            double km = 0;
            foreach (var location in MyMap.GetPolyline().Locations)
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

        public static void CheckingPushpin(this Map MyMap)
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

        private static void Pin_MouseDown(object sender, MouseButtonEventArgs e)
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
            if(map.IsPolyline())
                map.ConnectPushpins();
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

        private static MapPolyline GetPolyline(this Map MyMap)
        {
            foreach (var child in MyMap.Children)
            {
                if (child is MapPolyline)
                    return (MapPolyline) child;
            }
            return null;
        }

        public static void ClearAllMap(this Map MyMap)
        {
            MyMap.ClearPolyline();
            List<Pushpin> pins = MyMap.GetPushpins();
            foreach (var pin in pins)
            {
                MyMap.Children.Remove(pin);
            }
            counter = 0;
        }

    }
}
