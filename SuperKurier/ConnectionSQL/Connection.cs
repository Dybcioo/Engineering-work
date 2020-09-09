using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace ConnectionSQL
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ConnectionSQL"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ConnectionSQL;assembly=ConnectionSQL"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class Connection : Control
    {
        public string ConnectionString { get; set; }
        static Connection()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Connection), new FrameworkPropertyMetadata(typeof(Connection)));
        }

        public Connection(string serverDb, string db, string user, string password)
        {
           ConnectionString = GetConnectionString(serverDb, db, user, password);
        }

        public bool ConnectionSql()
        {
            try
            {
                SqlConnection cnn = new SqlConnection(ConnectionString);
                cnn.Open();
                cnn.Close();
            }catch(Exception e)
            {
                return false;
            }
            return true;
        }

        private string GetConnectionString(string serverDb, string db, string user, string password)
        {
            StringBuilder sb = new StringBuilder("Data Source=");
            sb.Append(serverDb);
            sb.Append(";Initial Catalog=");
            sb.Append(db);
            sb.Append(";User ID=");
            sb.Append(user);
            sb.Append(";Password=");
            sb.Append(password);
            return sb.ToString();
        }
    }
}
