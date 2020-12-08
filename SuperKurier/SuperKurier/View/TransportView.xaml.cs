using DataModel;
using SuperKurier.Enums;
using SuperKurier.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;

namespace SuperKurier.View
{
    /// <summary>
    /// Interaction logic for TransportView.xaml
    /// </summary>
    public partial class TransportView : Page
    {
        private List<Transport> transports;
        private CompanyEntities companyEntities;

        public TransportView()
        {
            InitializeComponent();
            companyEntities = new CompanyEntities();
            transports = new List<Transport>();
            loadData();
        }

        private void loadData()
        {
            transports = companyEntities.Parcel.Include(p => p.Region).Include(p => p.Region.Warehouse).Where(p => p.Status.id == (int)EnumParcelStatus.beetwen).Select(p =>
                 new Transport()
                 {
                     ParcelId = p.id,
                     ParcelCode = p.code,
                     WarehouseToCode = p.Region.Warehouse.code
                 }).ToList();
            transports.ForEach(p => p.WarehouseFromCode = companyEntities.HistoryOfParcel.Include(x => x.Warehouse).FirstOrDefault(x => x.idStatus < (int)EnumParcelStatus.beetwen && x.idParcel == p.ParcelId).Warehouse.code);
            TransportGrid.DataContext = null;
            TransportGrid.DataContext = new BindableCollection<Transport>(transports);
        }

        private void SearchTransport_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = SearchTransport.Text;
            search.ToUpper();
            var tran = transports.Where(p => p.ParcelCode.ToUpper().Contains(search) || p.WarehouseFromCode.ToUpper().Contains(search) || p.WarehouseToCode.ToUpper().Contains(search)).ToList();
            TransportGrid.DataContext = new BindableCollection<Transport>(tran);
        }
    }
}
