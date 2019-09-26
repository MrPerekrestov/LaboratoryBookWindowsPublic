using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
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

namespace LaboratoryBook.ModifyDatabaseWindow.AddUserWindow
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<sbyte> PermissionIDs { get; set; }
        public int LaboratoryBookID { get; set; }
        private ModifyDatabaseViewModel UsedViewModel { get; set; }

        public  AddUserWindow(ref ModifyDatabaseViewModel usedViewModel)
        {
            InitializeComponent();
            this.UsedViewModel = usedViewModel;

            GetUsersComboboxValues();
            GetPermissionIDsComoboxValues();
            
            this.CbxUsers.ItemsSource = Users;
            this.CbxPermissionIDs.ItemsSource = PermissionIDs;
        }

        private void GetPermissionIDsComoboxValues()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = "SELECT permission_id FROM permission; ";
            var sqlCommand = new MySqlCommand(commandString, connection);

            var userTable = new DataTable();

            try
            {
                connection.Open();

                var reader = sqlCommand.ExecuteReader();
                userTable.Load(reader);
                var permissionIDs = new ObservableCollection<sbyte>();

                foreach (DataRow row in userTable.Rows)
                {
                    var item = (sbyte)row[0];
                    permissionIDs.Add(item);                    
                }
                PermissionIDs = permissionIDs;

            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Add permissionID error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error
                            );
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }
        }

        private  void GetUsersComboboxValues()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = "SELECT user_id, user_name FROM users;";
            var sqlCommand = new MySqlCommand(commandString, connection);

            var userTable = new DataTable();

            try
            {
                connection.Open();

                var reader = sqlCommand.ExecuteReader();
                userTable.Load(reader);
                var userCollection = new ObservableCollection<UserModel>();
                foreach(DataRow row in userTable.Rows)
                {                   
                    var user = new UserModel()
                    {
                        UserID       = (int)row[0],
                        UserName     = (string)row[1],
                        PermissionID = 1 
                    };
                   
                    if (UsedViewModel.LaboratoryBookUsers
                                                 .Where(_user=>_user.UserID == user.UserID)
                                                 .ToArray()
                                                 .Count()==0)
                    {
                        if (UsedViewModel.LaboratoryBookUser.UserID!=user.UserID) userCollection.Add(user);
                    }                   
                }
                Users = userCollection;
            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Add users error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error
                            );
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button.Content.ToString() == "Close")
            {
                this.Close();
                return;
            }

            if (button.Content.ToString() == "Add")
            {
                var user = CbxUsers.SelectedItem as UserModel;

                if (user == null) return;

                user.PermissionID = (sbyte)CbxPermissionIDs.SelectedItem;

                var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
                var connection = new MySqlConnection(connectionString);

                var commandString = "INSERT INTO`db_users` (`user_id`, `db_id`, `permission_id`)" +
                                    $" VALUES ('{user.UserID}', '{UsedViewModel.LaboratoryBookID}', '{user.PermissionID}');";

                var sqlCommand = new MySqlCommand(commandString, connection);               

                try
                {
                    await connection.OpenAsync();
                    var result = await sqlCommand.ExecuteNonQueryAsync();

                    UsedViewModel.LaboratoryBookUsers.Add(user);
                    user.PropertyChanged += UsedViewModel.User_PropertyChanged;
                }
                catch (Exception exception)
                {
                    MessageBox.Show
                               (
                                  exception.Message,
                                  "Add user error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error
                                );
                }
                finally
                {
                    connection.Close();
                    sqlCommand?.Dispose();
                }

                

                this.Close();
                return;
            }
        }
    }
}
