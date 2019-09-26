using LaboratoryBook.UserClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace LaboratoryBook.UserWindow
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public static readonly DependencyProperty LaboratoryBookUserProperty =
           DependencyProperty.Register
                             (
                                  "LaboratoryBookUser",
                                   typeof(User),
                                   typeof(UserWindow)
                             );
        public User LaboratoryBookUser
        {
            get { return (User)this.GetValue(LaboratoryBookUserProperty); }
            set { this.SetValue(LaboratoryBookUserProperty, value); }
        }

        public UserWindow(ref User laboratoryBookUser)
        {
            InitializeComponent();
            this.Loaded += UserWindow_Loaded;
            this.LaboratoryBookUser = laboratoryBookUser;
        }

        public bool NameChanged { get; set; } = false;

        private async void UserWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.TbkUserName.Text = LaboratoryBookUser.UserName;
            var userId = LaboratoryBookUser.UserID;

            var loadUserStatusTask = new Task<string>(() =>
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
                    var connection       = new MySqlConnection(connectionString);    
                    
                    var commandStringBuilder = new StringBuilder();
                    commandStringBuilder.Append("SELECT access_status_name FROM access_status ");
                    commandStringBuilder.Append("JOIN users ");
                    commandStringBuilder.Append("ON users.status_id = access_status.access_status_id ");
                    commandStringBuilder.Append($"WHERE users.user_id = '{userId}'; ");
                    var commandString = commandStringBuilder.ToString();

                    var sqlCommand = new MySqlCommand(commandString, connection);

                    connection.Open();
                    string AccessStatus = (string)sqlCommand.ExecuteScalar();
                    connection.Close();

                    sqlCommand?.Dispose();
                    return AccessStatus;
                }
            );
            try
            {
                loadUserStatusTask.Start();
                var result = await loadUserStatusTask;
                TbkStatus.Text = result;
            }
            catch(Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    "User Window error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }            
            
        }
    }
}
