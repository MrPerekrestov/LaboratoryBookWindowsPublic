using LaboratoryBook.CreateLaboratoryBookWindow;
using LaboratoryBook.UserClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LaboratoryBook.LoginWindow
{
    public partial class LoginWindow : Window
    {
        //Close command event handlers

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {            
            WriteLoginToFile();            
            this.Close();
        }

        //Login command event handlers

        private void LoginCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var loginString = TbxLogin?.Text;

            if ((String.IsNullOrEmpty(loginString)) ||
                    (String.IsNullOrWhiteSpace(loginString)))
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
            if (IsLogged) e.CanExecute = false;
           
        }

        private async void LoginCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = $"SELECT count(*) FROM laboratory_books.users WHERE user_name = '{TbxLogin.Text}';";
            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {
                await connection.OpenAsync();
                TbkConnectionStatus.Text = "trying to log in...";

                //Check if user exist
                var userCheckResult = (long)(await sqlCommand.ExecuteScalarAsync());
                if (userCheckResult > 0)
                {
                    commandString = $"SELECT salt FROM laboratory_books.users WHERE user_name = '{TbxLogin.Text}'";
                    sqlCommand.CommandText = commandString;
                    var salt = (string)(await sqlCommand.ExecuteScalarAsync());

                    var generatedHash = LoginHelper.GenerateHash(salt, TbxPassword.Password);

                    commandString = $"SELECT count(*) FROM laboratory_books.users WHERE user_name = '{TbxLogin.Text}' AND password_hash ='{generatedHash}';";
                    sqlCommand.CommandText = commandString;
                    var passwordAndUserCheckResult = (long)(await sqlCommand.ExecuteScalarAsync());

                    //Check if user and pasword match 
                    if (passwordAndUserCheckResult > 0)
                    {
                        TbkConnectionStatus.Text = "Successfully connected";
                        var foregraundBrush = new SolidColorBrush(Colors.Green);
                        TbkConnectionStatus.Foreground = foregraundBrush;

                        //create user
                        var user = await LoginHelper.GetUserByNameAsync(TbxLogin.Text);
                        this.BookUser = user;
                       
                        //get list of databases and fill combobox by their values
                        var dbList = await LoginHelper.GetAvailableLaboratoryBooksAsync(TbxLogin.Text);

                        CbxDataBases.DataContext = dbList;
                        CbxDataBases.SelectedIndex = 0;
                        CbxDataBases.IsEnabled = true;

                        //Disable login and password controls
                        TbxLogin.IsEnabled = false;
                        TbxPassword.IsEnabled = false;

                        IsLogged = true;
                    }
                    else
                    {
                        TbkConnectionStatus.Text = $"Wrong password";
                        var foregraundBrush = new SolidColorBrush(Colors.Red);
                        TbkConnectionStatus.Foreground = foregraundBrush;
                    }
                }
                else
                {
                    var foregraundBrush = new SolidColorBrush(Colors.Red);
                    TbkConnectionStatus.Text = "User does not exist";
                    TbkConnectionStatus.Foreground = foregraundBrush;
                }
            }
            finally
            {
                await connection.CloseAsync();
                sqlCommand?.Dispose();
            }
        }

        //Connect command event handlers
        private void ConnectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (IsLogged)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private async void ConnectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);

            var dbName        = CbxDataBases?.SelectedItem?.ToString();
            var userName      = BookUser?.GetUserName();
            MessageBox((BookUser == null).ToString());
            MessageBox.Show(userName);
            MessageBox.Show(dbName);
            var commandString = "SELECT `permission_id` FROM `laboratory_books`.`db_list`" +
                                "JOIN `laboratory_books`.`db_users` ON `db_list`.`db_id` = `db_users`.`db_id`" +
                                "JOIN `laboratory_books`.`users` ON `db_users`.`user_id` = `users`.`user_id`" +
                                $"WHERE `db_name` = '{dbName}' AND `user_name` = '{userName}';";
            
            var sqlCommand = new MySqlCommand(commandString, connection);

            await connection.OpenAsync();
            var permission = (sbyte)(await sqlCommand.ExecuteScalarAsync());
            await connection.CloseAsync();

            try
            {
                LaboratoryBookPermission = (Permission?)permission;                
            }
            catch
            {
                var foregraundBrush = new SolidColorBrush(Colors.Red);
                TbkConnectionStatus.Foreground = foregraundBrush;
                TbkConnectionStatus.Text = $"permission {permission} does not exist";
                return;
            }

            if ((LaboratoryBookPermission!=null)&&(BookUser!=null))
            {
                var mainWindow = new MainWindow(BookUser, (Permission)LaboratoryBookPermission, dbName);
                mainWindow.Show();
                this.Close();
            }       
        }

        //create new laboratory book event handlers

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (BookUser?.GetAccessId()>2)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }

        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var user = BookUser;

            var createWindow = new CreateWindow(ref user)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            
            createWindow.ShowDialog();                        
        }
    }
}
