using SuperKurier.Control;
using SuperKurier.ViewModel;
using System.Windows;


namespace SuperKurier
{
    /// <summary>
    /// Interaction logic for employeeView.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            if (!loginWindow.Answer)
                Application.Current.Shutdown();
        }
    }
}
