using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryBook.ModifyDatabaseWindow
{
    public partial class ModifyDatabaseWindow : Window
    {
        //close command event handlers
        public void СloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        // AddUser command event handlers
        public void AddUserCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void AddUserCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel     = this.MainViewModel;
            var addUserWindow = new AddUserWindow.AddUserWindow(ref viewModel);

            addUserWindow.Owner = this;
            addUserWindow.ShowDialog();
        }

        //RemoveUser command event handlers

        public void RemoveUserCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
           if (MainViewModel?.SelectedUser!=null) e.CanExecute = true;
        }

        public async void RemoveUserCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
           var dialogResult =  MessageBox.Show(
                        $"Do you wan to remove use {MainViewModel.SelectedUser.UserName}?",
                        "Remove user",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                        );

            if (dialogResult == MessageBoxResult.No) return;
           
           
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = "DELETE FROM `db_users` " +
                               $"WHERE(`user_id` = '{MainViewModel.SelectedUser.UserID}') and (`db_id` = '{MainViewModel.LaboratoryBookID}');";
            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {
                await connection.OpenAsync();
                var result = await sqlCommand.ExecuteNonQueryAsync();

                MainViewModel.SelectedUser.PropertyChanged -= MainViewModel.User_PropertyChanged;
                MainViewModel.LaboratoryBookUsers.Remove(MainViewModel.SelectedUser);

                if (MainViewModel.LaboratoryBookUsers.Count() > 0) MainViewModel.SelectedUser = MainViewModel.LaboratoryBookUsers[0];

            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Delete user error",
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

            //add column event handlers

            public void AddColumnCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
            }

            public void AddColumnCommand_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                var viewModel = this.MainViewModel;
                var addUserWindow = new AddColumnWindow.AddColumnWindow(ref viewModel);
                
                addUserWindow.Owner = this;
                addUserWindow.ShowDialog();
            }

        //remove column event handlers

        public void RemoveColumnCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainViewModel?.SelectedColumn != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        public async void RemoveColumnCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show(
                        $"Do you wan to remove column {MainViewModel.SelectedColumn.ColumnName}?",
                        "Remove column",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                        );

            if (dialogResult == MessageBoxResult.No) return;

            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);


            var commandString = $"ALTER TABLE `laboratory_book_{MainViewModel.LaboratoryBookName}` "+ 
                                $"DROP COLUMN `{MainViewModel.SelectedColumn.ColumnName}`; ";           
            var sqlCommand    = new MySqlCommand(commandString, connection);

            try
            {
                await connection.OpenAsync();
                var result = await sqlCommand.ExecuteNonQueryAsync();

                MainViewModel.SelectedColumn.PropertyChanged -= MainViewModel.Column_PropertyChanged;
                MainViewModel.LaboratoryBookColumns.Remove(MainViewModel.SelectedColumn);                

                if (MainViewModel.LaboratoryBookColumns.Any()) MainViewModel.SelectedColumn = MainViewModel.LaboratoryBookColumns[0];

            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Delete column error",
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

        //Add list valus command event handlers
        public void AddListValueCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void AddListValueCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var mainViewModel = this.MainViewModel;

            var addListValueWindow = new AddListValueWindow.AddListValueWindow(ref mainViewModel)
            {
                Owner = this

            };

            addListValueWindow.ShowDialog();
        }

        //Remove list valus command event handlers
        public void RemoveListValueCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainViewModel?.SelectedList?.SelectedValue != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        public async void RemoveListValueCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show(
                        $"Do you wan to remove list value {MainViewModel.SelectedList.SelectedValue.ListValue}?",
                        "Remove list value",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                        );

            if (dialogResult == MessageBoxResult.No) return;

            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);
            var selectedValue = MainViewModel.SelectedList.SelectedValue;
     
            var commandString = $"DELETE FROM `{MainViewModel.SelectedList.ListName}s_{MainViewModel.LaboratoryBookName}` " +
                                $"WHERE (`{MainViewModel.SelectedList.ListName}` = '{MainViewModel.SelectedList.SelectedValue.ListValue}'); ";

            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {
                await connection.OpenAsync();
                var result = await sqlCommand.ExecuteNonQueryAsync();
                if (result>0)
                {
                    selectedValue.PropertyChanged -= MainViewModel.ListModel_PropertyChanged;
                    MainViewModel.SelectedList.Values.Remove(selectedValue);

                    if (MainViewModel.SelectedList.Values.Any())
                    {
                        MainViewModel.SelectedList.SelectedValue = MainViewModel.SelectedList.Values[0];
                    }

                }
            }
            catch(Exception exception)
            {
                MessageBox.Show
                          (
                             exception.Message,
                             "Delete list value error",
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

