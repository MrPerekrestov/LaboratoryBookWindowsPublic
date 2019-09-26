using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryBook.CreateLaboratoryBookWindow
{
    public partial class CreateWindow : Window
    {
        //close event handlers
        public void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        //create event handlers
        public void CreateCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public async void CreateCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);
            var bookName = TbxBookName.Text;
            var createScriptLines   = File.ReadAllLines("CreateLaboratoryBookTemplate.txt");
            var commandBuilder      = new StringBuilder();
            
            foreach (var line in createScriptLines)
            {
                commandBuilder.Append(" "+line+" ");
            }

            var createAllTablesString = commandBuilder.ToString();
            createAllTablesString = createAllTablesString.Replace("test_db", bookName);                                         
            
            var sqlCommand = new MySqlCommand(String.Empty, connection);            
           
            try
            {
                await connection.OpenAsync();
               
                //get user_id
                sqlCommand.CommandText = $"SELECT `user_id` FROM `users` WHERE user_name = '{User.GetUserName()}';";
                var userID = await sqlCommand.ExecuteScalarAsync();
                //add database to db_list
                sqlCommand.CommandText = $"INSERT INTO `db_list` (`db_name`, `creator_id`) VALUES ('{bookName}', '{userID.ToString()}');";
                var result = await sqlCommand.ExecuteNonQueryAsync();                

                //create all tables
                sqlCommand.CommandText = createAllTablesString;
                result = await sqlCommand.ExecuteNonQueryAsync();

                //get db_id
                sqlCommand.CommandText = $"SELECT `db_id` FROM `db_list` WHERE `db_name` = '{bookName}';";
                var bookID = await sqlCommand.ExecuteScalarAsync();

                //link creator of database with database
                sqlCommand.CommandText = $"INSERT INTO `db_users` (`user_id`, `db_id`, `permission_id`) VALUES ('{userID}', '{bookID}', '4');";
                result = await sqlCommand.ExecuteNonQueryAsync();

                //add statistics 
                sqlCommand.CommandText = $"INSERT INTO `statistics` (`db_name`, `user_name`, `time_changed`) " +
                                         $"VALUES ('{bookName}', '{User.GetUserName()}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');";
                result = await sqlCommand.ExecuteNonQueryAsync();
                MessageBox.Show
                    (
                        $"Book {bookName} was succesfully created",
                        "Book created",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                var mainWindow = new MainWindow(User, Permission.DatabaseAdminister, bookName);
                mainWindow.Show();

                this.Owner.Close();
                this.Close();

            }
            catch (Exception exception)
            {
                MessageBox.Show
                    (
                        exception.Message,
                        "SQL error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
            }
            finally
            {
                await connection.CloseAsync();
                sqlCommand?.Dispose();
            }
        }
    }
}
