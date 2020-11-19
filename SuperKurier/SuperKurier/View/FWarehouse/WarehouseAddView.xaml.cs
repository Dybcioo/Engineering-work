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
    /// Logika interakcji dla klasy WarehouseAddView.xaml
    /// </summary>
    public partial class WarehouseAddView : Page
    {
        public WarehouseAddView()
        {
            InitializeComponent();
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((WarehouseAddViewModel)DataContext).VisibilityOption = Visibility.Hidden;
        }
    }
}
