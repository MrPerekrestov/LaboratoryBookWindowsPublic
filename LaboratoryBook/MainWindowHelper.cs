

using LaboratoryBook.UserClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LaboratoryBook
{
   
    public static class MainWindowHelper
    {
        public async static Task UpdateStatistics (string laboratoryBookName, User user)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = $"UPDATE `statistics`" +
                                $" SET  `user_name` = '{user.GetUserName()}'," +
                                $" `time_changed` = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                                $"  WHERE(`db_name` = '{laboratoryBookName}');";         

            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {
                await connection.OpenAsync();
                var result = await sqlCommand.ExecuteNonQueryAsync();                
            }
            finally
            {
                await connection.CloseAsync();
                sqlCommand?.Dispose();
            }
        }
        public async static Task<string[]> CheckUpdatesAsync(string laboratoryBookName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = $"SELECT * FROM `statistics`" +
                                $" WHERE (`db_name` = '{laboratoryBookName}');";

            var sqlCommand = new MySqlCommand(commandString, connection);
            
            try
            {
                await connection.OpenAsync();

                var dataTable = new DataTable();

                var reader = await sqlCommand.ExecuteReaderAsync();
                dataTable.Load(reader);
                string[] result = new[] { dataTable.Rows[0][1].ToString(), dataTable.Rows[0][2].ToString()};

                return result;               
            }
            catch (Exception e)
            {
                MessageBox.Show
                    (
                        e.Message, "Statistics update error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                return null;
            }
            finally
            {
                await connection.CloseAsync();
                sqlCommand?.Dispose();
            }            
        }
        public async static Task<bool> DeleteRowAsync(string laboratoryBookName, Permission permission, DataRow row)
        {

            if (permission == Permission.DatabaseGuest) return false;

            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);

            var commandString = $"DELETE FROM `laboratory_book_{laboratoryBookName}`" +
                                $" WHERE (`sampleID` = '{row["sampleID"]}');";

            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {
                await connection.OpenAsync();
                var i = await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Row delete error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                sqlCommand?.Dispose();
            }

            return true;
        }


        public async static Task<bool> AddRowAsync(string laboratoryBookName, Permission permission, DataRow row, DataGrid dataGrid)
        {
            var columnList = dataGrid.Columns.Select(column => (string)column.Header)
                                             .ToArray();

            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);

            var sqlCommand = new MySqlCommand(String.Empty, connection);

            var insertCommandBuilder = new StringBuilder();
            insertCommandBuilder.Append($"INSERT INTO `laboratory_book_{laboratoryBookName}`");
            insertCommandBuilder.Append(" (`date`");
           
            for (int i=2; i<row.Table.Columns.Count; i++)
            {
               insertCommandBuilder.Append($", `{row.Table.Columns[i].ColumnName}`");
            }         

            insertCommandBuilder.Append(") VALUES (");
            //first row is date, needs converstion to SQL format date time
            var date = (DateTime)row[1];
            insertCommandBuilder.Append($"'{date.ToString("yyyy-MM-dd")}'");

            for (var i = 2; i < columnList.Count(); i++)
            {
                if (!string.IsNullOrEmpty(row[columnList[i]].ToString()))
                {
                    insertCommandBuilder.Append($" ,'{row[columnList[i]]}'");
                }
                else
                {
                    insertCommandBuilder.Append($" , NULL");
                }
            }
            insertCommandBuilder.Append(");");

            var commandString = insertCommandBuilder.ToString();           
            sqlCommand.CommandText = commandString;

            await connection.OpenAsync();

            try
            {
                var i = await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (Exception exception)
            {
                MessageBox.Show
                    (
                        exception.Message,
                        "Row add error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                return false;
            }
            finally
            {
                await connection.CloseAsync();
                sqlCommand?.Dispose();
            }
            return true;
        }

        public static bool AddRow(string laboratoryBookName, Permission permission, DataRow row, DataGrid dataGrid, User user)
        {
            
            var columnList = user.GetColumnNames(laboratoryBookName);            

            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var sqlCommand = new MySqlCommand(String.Empty, connection);

            var insertCommandBuilder = new StringBuilder();
            insertCommandBuilder.Append($"INSERT INTO `laboratory_book_{laboratoryBookName}`");
            insertCommandBuilder.Append(" (`sampleID`,`date`");

            for (int i = 2; i < row.Table.Columns.Count; i++)
            {
                insertCommandBuilder.Append($", `{row.Table.Columns[i].ColumnName}`");
            }

            insertCommandBuilder.Append(") VALUES (");
            insertCommandBuilder.Append($"'{row[columnList[0]]}'");
            //first datum is date, needs converstion to SQL format date time
            var date = (DateTime)row[1];
            insertCommandBuilder.Append($" ,'{date.ToString("yyyy-MM-dd")}'");

            for (var i = 2; i < columnList.Count(); i++)
            {
                if (!string.IsNullOrEmpty(row[columnList[i]].ToString()))
                {
                    insertCommandBuilder.Append($" ,'{row[columnList[i]]}'");
                }
                else
                {
                    insertCommandBuilder.Append($" , NULL");
                }
            }
            insertCommandBuilder.Append(");");

            var commandString = insertCommandBuilder.ToString();
            sqlCommand.CommandText = commandString;

           connection.Open();

            try
            {
                var i = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                MessageBox.Show
                    (
                        exception.Message,
                        "Row add error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                return false;
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }
            return true;
        }

        public static bool ChangeCell(string laboratoryBookName, string header, object changingValue, object sampleId)
        {
            
            try
            {                
                if (header == "date") changingValue = Convert.ToDateTime(changingValue);
               
            }
            catch
            {
                changingValue = DateTime.MinValue;
            }
            
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);

            var sqlCommand = new MySqlCommand(String.Empty, connection);           
            
            connection.Open();
            
            string commandString = string.Empty;
            
            changingValue = changingValue ?? string.Empty;


            if (string.IsNullOrEmpty(changingValue.ToString()))
            {
                commandString = $"UPDATE `laboratory_book_{laboratoryBookName}`" +
                                $" SET `{header}` = NULL WHERE (`sampleID` = '{sampleId}')";
            }
            else if (changingValue is DateTime dateTime)
            {
                var dateTimeString = dateTime.ToString("yyyy-MM-dd");
                commandString = $"UPDATE `laboratory_book_{laboratoryBookName}`" +
                                $" SET `{header}` = '{dateTimeString}' WHERE (`sampleID` = '{sampleId}')";
            }
            else
            {
                commandString = $"UPDATE `laboratory_book_{laboratoryBookName}`" +
                                $" SET `{header}` = '{changingValue}' WHERE (`sampleID` = '{sampleId}')";
            }

            sqlCommand.CommandText = commandString;

            try
            {
                var i = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                MessageBox.Show
                    (
                        exception.Message,
                        "Cell change error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                connection.Close();
                sqlCommand?.Dispose();
                return false;
            }

            connection.Close();
            sqlCommand?.Dispose();
            return true;
        }

        public static bool ChangeRow(string laboratoryBookName, Permission permission, DataRow row, List<string> columnList)
        {                       

            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var sqlCommand = new MySqlCommand(String.Empty, connection);

            connection.Open();

            string commandString = null;

            foreach (var header in columnList)
            {
               
                if (string.IsNullOrEmpty(row[header].ToString()))
                {
                    commandString = $"UPDATE `laboratory_book_{laboratoryBookName}`" +
                                    $" SET `{header}` = NULL WHERE (`sampleID` = '{row["sampleID"]}')";
                }
                else if (row[header] is DateTime dateTime)
                {
                    var dateTimeString = dateTime.ToString("yyyy-MM-dd");
                    commandString = $"UPDATE `laboratory_book_{laboratoryBookName}`" +
                                    $" SET `{header}` = '{dateTimeString}' WHERE (`sampleID` = '{row["sampleID"]}')";
                }
                else
                {
                    commandString = $"UPDATE `laboratory_book_{laboratoryBookName}`" +
                                    $" SET `{header}` = '{row[header]}' WHERE (`sampleID` = '{row["sampleID"]}')";
                }

                sqlCommand.CommandText = commandString;

                try
                {
                    var i = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    MessageBox.Show
                        (
                            exception.Message,
                            "Cell change error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    connection.Close();
                    sqlCommand?.Dispose();
                    return false;
                }
            }

            connection.Close();
            sqlCommand?.Dispose();
            return true;
        }

        public async static Task<bool> ChangeRowAsync(string laboratoryBookName, Permission permission, DataRow row, DataGrid dataGrid)
        {

            var columnList = dataGrid.Columns.Where(column => (string)(column.Header) != "sampleID").
                                              Select(column => (string)column.Header);

            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);

            var sqlCommand = new MySqlCommand(String.Empty, connection);

            await connection.OpenAsync();

            string commandString = null;

            foreach (var header in columnList)
            {
                if (string.IsNullOrEmpty(row[header].ToString())) continue;

                
                if (string.IsNullOrEmpty(row[header].ToString()))
                {
                    commandString = $"UPDATE `laboratory_book_{laboratoryBookName}`" +
                                    $" SET `{header}` = NULL WHERE (`sampleID` = '{row["sampleID"]}')";
                }
                else if (row[header] is DateTime dateTime)
                {
                    var dateTimeString = dateTime.ToString("yyyy-MM-dd");
                    commandString = $"UPDATE `laboratory_book_{laboratoryBookName}`" +
                                    $" SET `{header}` = '{dateTimeString}' WHERE (`sampleID` = '{row["sampleID"]}')";
                }
                else
                {
                    commandString = $"UPDATE `laboratory_book_{laboratoryBookName}`" +
                                    $" SET `{header}` = '{row[header]}' WHERE (`sampleID` = '{row["sampleID"]}')";
                }

                sqlCommand.CommandText = commandString;

                try
                {
                    var i = await sqlCommand.ExecuteNonQueryAsync();
                }
                catch (Exception exception)
                {
                    MessageBox.Show
                        (
                            exception.Message,
                            "Cell change error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    await connection.CloseAsync();
                    sqlCommand?.Dispose();
                    return false;
                }
            }
           

            await connection.CloseAsync();
            sqlCommand?.Dispose();
            return true;
        }

        public async static Task<ObservableCollection<object>> SetColumnListAsync(string laboratoryBookName, ColumnToGet columnToGet)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);            

            string commandString = null;

            switch (columnToGet)
            {
                case ColumnToGet.Material:
                    commandString = $"SELECT `material` FROM materials_{laboratoryBookName};";
                    break;
                case ColumnToGet.Operator:
                    commandString = "SELECT `user_name` FROM `db_users` "+
                                    "JOIN `users` ON `users`.`user_id` = `db_users`.`user_id` "+
                                    "JOIN `db_list` ON `db_list`.`db_id` = `db_users`.`db_id` "+
                                    $"WHERE `db_name`= '{laboratoryBookName}';";
                    break;
                case ColumnToGet.PermissionID: 
                    commandString = "SELECT `permission_id` FROM `permission`;";
                    break;
                case ColumnToGet.Regime:
                    commandString = $"SELECT `regime` FROM regimes_{laboratoryBookName};";
                    break;
                case ColumnToGet.Substrate:
                    commandString = $"SELECT `substrate` FROM substrates_{laboratoryBookName};";
                    break;
            }

            var sqlCommand = new MySqlCommand(commandString, connection);
            var requestedDataTable = new DataTable();

            try
            {
                await connection.OpenAsync();

                var reader = await sqlCommand.ExecuteReaderAsync();
                
                requestedDataTable.Load(reader);
            }
            catch(Exception exception)
            {
                MessageBox.Show
                    (
                        exception.Message,
                        "Load error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
            }
            finally
            {
                await connection.CloseAsync();
                sqlCommand?.Dispose();
            }
            var result = new ObservableCollection<object>();

            foreach (DataRow dr in requestedDataTable.Rows)
            {
                result.Add(dr[0]);
            }
            return result;
        }

        public static ObservableCollection<object> SetColumnList(string laboratoryBookName, ColumnToGet columnToGet)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            string commandString = null;

            switch (columnToGet)
            {
                case ColumnToGet.Material:
                    commandString = $"SELECT `material` FROM materials_{laboratoryBookName};";
                    break;
                case ColumnToGet.Operator:
                    commandString = "SELECT `user_name` FROM `db_users` " +
                                    "JOIN `users` ON `users`.`user_id` = `db_users`.`user_id` " +
                                    "JOIN `db_list` ON `db_list`.`db_id` = `db_users`.`db_id` " +
                                    $"WHERE `db_name`= '{laboratoryBookName}';";
                    break;
                case ColumnToGet.PermissionID:
                    commandString = "SELECT `permission_id` FROM `permission`;";
                    break;
                case ColumnToGet.Regime:
                    commandString = $"SELECT `regime` FROM regimes_{laboratoryBookName};";
                    break;
                case ColumnToGet.Substrate:
                    commandString = $"SELECT `substrate` FROM substrates_{laboratoryBookName};";
                    break;
            }

            var sqlCommand = new MySqlCommand(commandString, connection);
            var requestedDataTable = new DataTable();

            try
            {
               connection.Open();

                var reader = sqlCommand.ExecuteReader();

                requestedDataTable.Load(reader);
            }
            catch (Exception exception)
            {
                MessageBox.Show
                    (
                        exception.Message,
                        "Load error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }
            var result = new ObservableCollection<object>();

            foreach (DataRow dr in requestedDataTable.Rows)
            {
                result.Add(dr[0]);
            }
            return result;
        }



    }
}
