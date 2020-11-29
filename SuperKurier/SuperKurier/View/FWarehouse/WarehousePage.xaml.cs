using Caliburn.Micro;
using DataModel;
using SuperKurier.Enums;
using SuperKurier.ViewModel;
using SuperKurier.ViewModel.FWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private int reload = 0;
        public WarehousePage()
        {
            InitializeComponent();
            companyEntities = new CompanyEntities();
            gridDataInitialize();
        }

        private void gridDataInitialize()
        {
            try
            {
                DataGridDocument.DataContext = null;
                DataGridDocument.DataContext = new BindableCollection<Document>(companyEntities.Document.ToList());
            }
            catch (Exception ex)
            {
            }
        }

        private void SearchDocument_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = (DataGridRow)sender;
            EnumTypeOfDocument type = EnumTypeOfDocument.PZ;
            if (((Document)row.Item).TypeOfDocument.type.Equals("PZ"))
                type = EnumTypeOfDocument.PZ;
            else if (((Document)row.Item).TypeOfDocument.type.Equals("WZ"))
                type = EnumTypeOfDocument.WZ;
            ((WarehouseViewModel)DataContext).AddViewModel = new WarehouseAddViewModel((Document)row.Item, type) { VisibilityOption = Visibility.Visible };
            frame.Visibility = Visibility;
        }

        private void BtnPZAdd_Click(object sender, RoutedEventArgs e)
        {
            ((WarehouseViewModel)DataContext).AddViewModel = new WarehouseAddViewModel() { VisibilityOption = Visibility.Visible };
            frame.Visibility = Visibility;
        }

        private void BtnWZAdd_Click(object sender, RoutedEventArgs e)
        {
            ((WarehouseViewModel)DataContext).AddViewModel = new WarehouseAddViewModel(null, EnumTypeOfDocument.WZ) { VisibilityOption = Visibility.Visible };
            frame.Visibility = Visibility;
        }

        private void Page_LayoutUpdated(object sender, EventArgs e)
        {
            if(DataContext is WarehouseViewModel)
            {
                var wavm = (WarehouseAddViewModel)((WarehouseViewModel)DataContext).AddViewModel;
                if (wavm == null)
                    return;
                if(wavm.VisibilityOption == Visibility.Visible)
                {
                    reload = 0;
                    return;
                }else if(reload == 0)
                {
                    DataGridDocument.DataContext = null;
                    DataGridDocument.DataContext = new BindableCollection<Document>(companyEntities.Document.AsNoTracking().ToList());
                    DataGridDocument.Items.Refresh();
                    reload++;
                }
            }
        }
    }
}
