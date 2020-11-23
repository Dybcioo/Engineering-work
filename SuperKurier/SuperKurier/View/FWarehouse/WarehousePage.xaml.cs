using DataModel;
using SuperKurier.ViewModel;
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
    /// Logika interakcji dla klasy WarehousePage.xaml
    /// </summary>
    public partial class WarehousePage : Page
    {
        private CompanyEntities companyEntities;
        public WarehousePage()
        {
            InitializeComponent();
            companyEntities = new CompanyEntities();
            DataGridDocument.DataContext = companyEntities.Document.ToList();
        }

        private void SearchDocument_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = (DataGridRow)sender;
            ((WarehouseViewModel)DataContext).AddViewModel = new WarehouseAddViewModel((Document)row.Item) { VisibilityOption = Visibility.Visible };
            frame.Visibility = Visibility;
        }

        private void BtnPZAdd_Click(object sender, RoutedEventArgs e)
        {
            ((WarehouseViewModel)DataContext).AddViewModel = new WarehouseAddViewModel() { VisibilityOption = Visibility.Visible };
            frame.Visibility = Visibility;
        }

        private void BtnWZAdd_Click(object sender, RoutedEventArgs e)
        {
            ((WarehouseViewModel)DataContext).AddViewModel = new WarehouseAddViewModel() { VisibilityOption = Visibility.Visible };
            frame.Visibility = Visibility;
        }
    }
}
