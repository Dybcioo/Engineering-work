﻿using Caliburn.Micro;
using DataModel;
using Microsoft.Maps.MapControl.WPF;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperKurier.View
{
    /// <summary>
    /// Interaction logic for RegionView.xaml
    /// </summary>
    public partial class RegionView : Page
    {
        
        private MapPolyline polyline = null;
        private Location location = null;
        private bool regionSquare = false;
        private bool activationFunction = true;
        private CompanyEntities companyEntities = new CompanyEntities();
        private bool regionVisibility = false;
        private DataModel.Region region = null;

        public RegionView()
        {
            InitializeComponent();
        }

        private void MyMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (activationFunction)
            {
                e.Handled = true;
                MyMap.CheckingPushpin(e);
            }
        }

        private void ContextMenu_RightClick(object sender, MouseButtonEventArgs e)
        {
            if (activationFunction)
            {
                Location location = MyMap.ViewportPointToLocation(Mouse.GetPosition(MyMap));
                var temp = MyMap.GetCurrentRegion(location, companyEntities);
                if (temp != null && regionVisibility)
                {
                    ContextMenu context = new ContextMenu();
                    context.IsOpen = true;
                    var editRegion = new MenuItem() { Header = "Edytuj region" };
                    var removeRegion = new MenuItem() { Header = "Usuń region" };
                    var infoRegion = new MenuItem() { Header = "Info" };

                    infoRegion.Click += (s, es) => MessageBox.Show($"Kod: {temp.code}");
                    editRegion.Click += async (s, es) =>
                    {
                        MyMap.Children.Remove(MyMap.GetPolyline(temp));
                        while (e.LeftButton == MouseButtonState.Released) { await Task.Delay(25); }
                        var viewModel = (RegionViewModel)this.DataContext;
                        viewModel.WarehouseSelectedRegion = viewModel.Warehouses.FirstOrDefault(w => w.id == temp.idWarehouse);
                        viewModel.EmployeeSelectedRegion = viewModel.Employees.FirstOrDefault(e => e.idRegion == temp.id);
                        CreateRegions_ClickAsync(s, es);
                        region = temp;
                    };
                    removeRegion.Click += (s, es) =>
                    {
                        System.Windows.Forms.DialogResult result = (System.Windows.Forms.DialogResult)MessageBox.Show($"Na pewno chcesz usunąć region {temp.code}?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            var emplTemp = companyEntities.Employee.Where(e => e.idRegion == temp.id);
                            foreach (var employee in emplTemp)
                            {
                                employee.idRegion = null;
                            }
                            MyMap.Children.Remove(MyMap.GetPolyline(temp));
                            companyEntities.Localization.Remove(temp.Localization);
                            companyEntities.Localization.Remove(temp.Localization1);
                            temp.idWarehouse = null;
                            companyEntities.Region.Remove(temp);
                            companyEntities.SaveChanges();
                        }
                    };
                    context.Items.Add(editRegion);
                    context.Items.Add(removeRegion);
                    context.Items.Add(infoRegion);
                }
                else
                {
                    ContextMenu context = new ContextMenu();
                    context.IsOpen = true;
                    var createRegions = new MenuItem() { Header = "Dodaj nowy region" };
                    var connectPushPins = new MenuItem() { Header = "Połącz pinezki" };
                    var clear = new MenuItem() { Header = "Wyczyść trase" };
                    clear.Click += ClearPolyline_Click;
                    connectPushPins.Click += ConnectPushPins_Click;
                    createRegions.Click += CreateRegions_ClickAsync;
                    context.Items.Add(createRegions);
                    context.Items.Add(connectPushPins);
                    context.Items.Add(clear);
                }
            }

        }

        private void BtnClearRegion_Click(object sender, RoutedEventArgs e)
        {
            activationFunction = true;
            MyMap.Children.Remove(polyline);
            MyMap.ClearTextInMap();
            RegionOption.Visibility = Visibility.Hidden;
        }

        private void BtnAddRegion_Click(object sender, RoutedEventArgs e)
        {
            var info = new InfoWindow();
            activationFunction = true;
            MyMap.Children.Remove(polyline);
            MyMap.ClearTextInMap();
            RegionOption.Visibility = Visibility.Hidden;
            DataModel.Localization startLocal = new DataModel.Localization() { latitude = location.Latitude.ToString(), longitude = location.Longitude.ToString() };
            DataModel.Localization endLocal = new DataModel.Localization() { latitude = polyline.Locations[2].Latitude.ToString(), longitude = polyline.Locations[2].Longitude.ToString() };
            Warehouse warehouse = (Warehouse)RegionWarehouse.SelectedItem;
            Employee empl = (Employee)RegionEmployee.SelectedItem;
            var employee = companyEntities.Employee.FirstOrDefault(e => e.id == empl.id);
            if (region != null)
            {
                if (MyMap.IsAllowRegion(location, polyline.Locations[2], companyEntities, region.id))
                {
                    var start = companyEntities.Localization.First(l => l.id == region.idStartLocalization);
                    var end = companyEntities.Localization.First(l => l.id == region.idEndLocalization);
                    start.latitude = startLocal.latitude;
                    start.longitude = startLocal.longitude;
                    end.latitude = endLocal.latitude;
                    end.longitude = endLocal.longitude;
                    var reg = companyEntities.Region.Find(region.id);
                    reg.code = $"{warehouse.code}/{reg.id}";
                    reg.idWarehouse = warehouse.id;
                    employee.idRegion = reg.id;
                    employee.Region = reg;
                    companyEntities.SaveChanges();
                    info.ShowInfo("Region edytowano pomyślnie", "Region", "OK");
                }
                else
                {
                    info.ShowInfo("Nowy region nie może pokrywać regionów już istniejących!", "Region", "OK");
                }

            }
            else if (MyMap.IsAllowRegion(location, polyline.Locations[2], companyEntities))
            {
                DataModel.Region newRegion = new DataModel.Region();
                companyEntities.Localization.Add(startLocal);
                companyEntities.Localization.Add(endLocal);
                companyEntities.SaveChanges();
                newRegion.idWarehouse = warehouse.id;
                newRegion.idStartLocalization = startLocal.id;
                newRegion.idEndLocalization = endLocal.id;
                companyEntities.Region.Add(newRegion);
                companyEntities.SaveChanges();
                var temp = companyEntities.Region.OrderByDescending(r => r.id).First();
                temp.code =  $"{warehouse.code}/{temp.id}";
                employee.idRegion = temp.id;
                employee.Region = temp;
                companyEntities.SaveChanges();
                info.ShowInfo("Region zapisano pomyślnie", "Region", "OK");
            }
            else
            {
                info.ShowInfo("Nowy region nie może pokrywać regionów już istniejących!", "Region", "OK");
            }
            region = null;
            BtnShowRegions_Click(sender, e);
        }

        private void BtnShowRegions_Click(object sender, RoutedEventArgs e)
        {
            var regions = companyEntities.Region.ToList();
            var localizations = companyEntities.Localization.ToList();
            foreach (var region in regions)
            {
                var startLocal = localizations.Find(l => l.id == region.idStartLocalization);
                var endLocal = localizations.Find(l => l.id == region.idEndLocalization);
                if (MyMap.IsPolylineInLocalization(startLocal))
                    continue;
                MyMap.DrawSquare(startLocal, endLocal);
                regionVisibility = true;
            }
        }

        private async void CreateRegions_ClickAsync(object sender, RoutedEventArgs e)
        {
            while (Mouse.LeftButton == MouseButtonState.Released) { await Task.Delay(25); }
            polyline = new MapPolyline();
            polyline.Stroke = new SolidColorBrush(Colors.Blue);
            polyline.StrokeThickness = 2;
            polyline.Opacity = 0.7;
            location = MyMap.ViewportPointToLocation(Mouse.GetPosition(MyMap));
            LocationCollection locations = new LocationCollection();
            locations.Add(location);
            locations.Add(new Location());
            locations.Add(new Location());
            locations.Add(new Location());
            locations.Add(new Location());
            polyline.Locations = locations;
            MyMap.Children.Add(polyline);
            regionSquare = true;
            activationFunction = false;
        }

        private async void MyMap_MouseMove(object sender, MouseEventArgs e)
        {
            Location location2 = new Location();
            if (polyline != null && regionSquare)
            {
                var position1 = Mouse.GetPosition(MyMap);
                Location location1 = MyMap.ViewportPointToLocation(position1);
                polyline.Locations[1].Latitude = location1.Latitude;
                polyline.Locations[1].Longitude = location.Longitude;
                polyline.Locations[2] = location1;
                polyline.Locations[3].Latitude = location.Latitude;
                polyline.Locations[3].Longitude = location1.Longitude;
                polyline.Locations[4] = location;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    location2 = MyMap.Center;
                    while (e.LeftButton == MouseButtonState.Pressed) { await Task.Delay(25); }
                }
                location1 = MyMap.Center;
                if (location2 == location1)
                {
                    regionSquare = false;
                    polyline.Stroke = new SolidColorBrush(Colors.Green);
                    MyMap.ShowDistance(polyline);
                    RegionOption.Visibility = Visibility.Visible;
                    DoubleAnimation widthAnimation = new DoubleAnimation() { From = 0, To = 300, Duration = TimeSpan.FromSeconds(1), RepeatBehavior = new RepeatBehavior(1) };
                    DoubleAnimation opacityAnimation = new DoubleAnimation() { From = 0.0, To = 1.0, Duration = TimeSpan.FromSeconds(1), RepeatBehavior = new RepeatBehavior(1) };
                    RegionOption.BeginAnimation(StackPanel.WidthProperty, widthAnimation);
                    RegionOption.BeginAnimation(StackPanel.OpacityProperty, opacityAnimation);
                }
            }
        }

        private void ClearPolyline_Click(object sender, RoutedEventArgs e)
        {
            MyMap.ClearAllMap();
            regionVisibility = false;
            region = null;
        }

        private void ConnectPushPins_Click(object sender, RoutedEventArgs e)
        {
            MyMap.ConnectPushpins();
        }
    }
}
