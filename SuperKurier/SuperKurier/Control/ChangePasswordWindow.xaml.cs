using DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperKurier.Control
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window, INotifyPropertyChanged
    {
        public bool Answer { get; set; } = false;
        #region SetColors
        private string _backgroundOption;
        public string BackgroundOption
        {
            get { return _backgroundOption; }
            set
            {
                _backgroundOption = value;
                OnPropertChanged(nameof(BackgroundOption));
            }
        }
        private string _foregroundOption;
        public string ForegroundOption
        {
            get { return _foregroundOption; }
            set
            {
                _foregroundOption = value;
                OnPropertChanged(nameof(ForegroundOption));
            }
        }
        private string _inputOption;
        public string InputOption
        {
            get { return _inputOption; }
            set
            {
                _inputOption = value;
                OnPropertChanged(nameof(InputOption));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Color _colorBtn;
        public Color ColorBtn
        {
            get { return _colorBtn; }
            set
            {
                _colorBtn = value;
                OnPropertChanged(nameof(ColorBtn));
            }
        }
        public bool IsBlack
        {
            get { return Properties.Settings.Default.IsBlack; }
            set { }
        }
        #endregion
        private CompanyEntities companyEntities;
        public ChangePasswordWindow()
        {
            InitializeComponent();
            companyEntities = new CompanyEntities();
        }

        private void BlackAndWhiteLayout(bool black)
        {
            if (black)
            {
                BackgroundOption = "Black";
                ForegroundOption = "White";
                InputOption = "#FF787878";
                ColorBtn = Color.FromArgb(100, 104, 104, 104);
            }
            else
            {
                BackgroundOption = "White";
                ForegroundOption = "Black";
                InputOption = "#FF6EAAFF";
                ColorBtn = Color.FromArgb(100, 193, 193, 193);
            }
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BlackAndWhiteLayout(IsBlack);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string currentPass = Password.Password;
            string newPass = NewPassword.Password;
            byte[] hashBytes;
            byte[] salt;
            Rfc2898DeriveBytes pbkdf2;
            byte[] hash;

            if (newPass.Length < 6) {
                errorLabel.Content = "Hasło musi zawierać minimum 6 znaków!";
                Answer = false;
                return;
            }
            if (!newPass.Equals(RepeatPassword.Password))
            {
                errorLabel.Content = "Wprowadzone hasła różnią się!";
                Answer = false;
                return;
            }
            Employee employee = companyEntities.Employee.FirstOrDefault(e => e.id == Properties.Settings.Default.IdUser);
            string userPass = employee.password;
            if (!string.IsNullOrWhiteSpace(userPass))
            {
                hashBytes = Convert.FromBase64String(userPass);
                salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
                pbkdf2 = new Rfc2898DeriveBytes(currentPass, salt, 100000);
                hash = pbkdf2.GetBytes(20);
                for (int i = 0; i < 20; i++)
                    if (hashBytes[i + 16] != hash[i])
                    {
                        errorLabel.Content = "Wprowadzone hasło jest nieprawidłowe!";
                        Answer = false;
                        return;
                    }
            }
            

            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            pbkdf2 = new Rfc2898DeriveBytes(newPass, salt, 100000);
            hash = pbkdf2.GetBytes(20);
            hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            newPass = Convert.ToBase64String(hashBytes);
            employee.password = newPass;
            companyEntities.SaveChanges();
            Answer = true;
            Close();
        }
    }
}
