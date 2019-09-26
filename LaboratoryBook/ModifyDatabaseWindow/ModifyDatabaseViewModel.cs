using LaboratoryBook.UserClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LaboratoryBook.ModifyDatabaseWindow
{ 
    
    public class ModifyDatabaseViewModel: ViewModelBase
    {
        public int LaboratoryBookID { get; private set; }
        public string LaboratoryBookName { get; private set; }
        public User LaboratoryBookUser { get; private set; }
        

        private sbyte selectedPermissionID;
        public sbyte SelectedPermissionID
        {
            get
            {
                return selectedPermissionID;
            }
            set
            {
                selectedPermissionID = value;
                OnPropertyChanged();
            }
        }

        private UserModel selectedUser;
        public UserModel SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                selectedUser = value;
                OnPropertyChanged();
            }
        }
        private ColumnModel selectedColumn;
        public ColumnModel SelectedColumn
        {
            get
            {
                return selectedColumn;
            }
            set
            {
                selectedColumn = value;
                OnPropertyChanged();
            }
        }

        private ListModel selectedList;
        public ListModel SelectedList
        {
            get
            {
                return selectedList;
            }
            set
            {
                selectedList = value;
                OnPropertyChanged();
            }
        }

        private string[] ForbidenToModifyColumnNames = new string[]
        {
            "sampleID",
            "date",
            "material",
            "substrate",
            "thickness",
            "depositionTime",
            "depositionPressure",
            "regime",
            "description",
            "operator",
            "permissionID",
        };

       
        public ObservableCollection<UserModel> LaboratoryBookUsers { get; set; } 
        public ObservableCollection<sbyte> LaboratoryBookPermissions { get; set; }
        public ObservableCollection<ColumnModel> LaboratoryBookColumns { get; set; }
        public List<ColumnModel> ColumnListBeforeChange { get; set; }
        public ObservableCollection<ListModel> LaboratoryBookLists { get; set; }

        public ModifyDatabaseViewModel(User laboratoryBookUser, string laboratoryBookName)
        {
            this.LaboratoryBookUser = laboratoryBookUser;
            this.LaboratoryBookName = laboratoryBookName;

            this.LaboratoryBookID = GetLaboratoryBookID(laboratoryBookName);
            // set users list
            this.LaboratoryBookUsers = GetUsers(this.LaboratoryBookID);
            foreach (var user in LaboratoryBookUsers)
            {
                user.PropertyChanged += User_PropertyChanged;
            }
            if (LaboratoryBookUsers.Any()) this.SelectedUser = LaboratoryBookUsers[0];
            //set permissions list
            this.LaboratoryBookPermissions = GetPermissions();

            //set columns list
            this.LaboratoryBookColumns = GetColumns(this.LaboratoryBookID);

            if (LaboratoryBookColumns.Count() > 0) this.SelectedColumn = this.LaboratoryBookColumns[0];
            foreach (var column in LaboratoryBookColumns)
            {
                column.PropertyChanged += Column_PropertyChanged;
            }
            this.ColumnListBeforeChange = new List<ColumnModel>();
            foreach (var column in LaboratoryBookColumns)
            {
                var col = new ColumnModel
                {
                    ColumnName = column.ColumnName,
                    ColumnType = column.ColumnType
                };
                
                ColumnListBeforeChange.Add(col);
            }

            //set laboratory book lists 
            this.LaboratoryBookLists = new ObservableCollection<ListModel>();
            var taskList             = new List<Task<ListModel>>();

            var materialTask  = Task.Run(() => GetListModel("material", laboratoryBookName));
            var substrateTask = Task.Run(() => GetListModel("substrate", laboratoryBookName));
            var regimeTask    = Task.Run(() => GetListModel("regime", laboratoryBookName));
            
            taskList.AddRange(new List<Task<ListModel>>() { materialTask, substrateTask, regimeTask });           
            
            while (taskList.Any())
            {
                var finishedTaskIndex = Task.WaitAny(taskList.ToArray());

                var listModel = taskList[finishedTaskIndex].Result;

                foreach (ListValueModel valueModel in listModel.Values)
                {
                    valueModel.PropertyChanged+= ListModel_PropertyChanged;
                }
                
                LaboratoryBookLists.Add(listModel);      
                
                taskList.Remove(taskList[finishedTaskIndex]);
            }

            if (this.LaboratoryBookLists.Any())
            {
                this.SelectedList = this.LaboratoryBookLists[0];
            }

        }

        public async void  ListModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var user = sender as UserModel;

            string oldValue = SelectedList.SelectedValue.OldListValue;
            string newValue = SelectedList.SelectedValue.ListValue;

            if (String.Equals(oldValue, newValue, StringComparison.Ordinal)) return;
           
            var commandString = $"UPDATE `{SelectedList.ListName}s_{LaboratoryBookName}` " +
                                $"SET `{SelectedList.ListName}` = '{newValue}' " +
                                $"WHERE (`{SelectedList.ListName}` = '{oldValue}'); ";
            
            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {
                await connection.OpenAsync();
                var result = await sqlCommand.ExecuteNonQueryAsync();               

                SelectedList.SelectedValue.OldListValue = SelectedList.SelectedValue.ListValue;

            }
            catch (Exception exception)
            {
                SelectedList.SelectedValue.ListValue = SelectedList.SelectedValue.OldListValue;

                MessageBox.Show
                           (
                              exception.Message,
                              "Lists update error",
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

        //event handler which update user preferences after changes in the view model

        public async void User_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { 
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);

            var user = sender as UserModel;
           
            var commandString = "UPDATE `db_users` " +
                                $"SET `permission_id` = '{user.PermissionID}' " +
                                $"WHERE (`user_id` = '{user.UserID}') and (`db_id` = '{LaboratoryBookID}'); ";
            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {
                await connection.OpenAsync();
                var result = await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "User update error",
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

        //event handler which update laboratory book after changes in view model

        public async void Column_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var column = sender as ColumnModel;
           
            var commandString = $"SELECT db_name FROM db_list WHERE db_id = {this.LaboratoryBookID}";
                      
            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {
                await connection.OpenAsync();
                var laboratoryBookName =(string) (await sqlCommand.ExecuteScalarAsync());
                ColumnModel oldColumn = column;
                foreach (ColumnModel oldCol in ColumnListBeforeChange)
                {
                    var columnMatched = false;
                    foreach (ColumnModel newCol in LaboratoryBookColumns)
                    {
                        if (oldCol.ColumnName == newCol.ColumnName) columnMatched = true;
                    }

                    if (!columnMatched)
                    {
                        oldColumn = oldCol;
                        break;
                    }
                }
                
                sqlCommand.CommandText = $"ALTER TABLE laboratory_book_{laboratoryBookName} " +
                                         $"CHANGE COLUMN `{oldColumn.ColumnName}` `{column.ColumnName}` {column.ColumnType} " +
                                         $"NULL DEFAULT NULL; ";

                var result = await sqlCommand.ExecuteNonQueryAsync();              

                ColumnListBeforeChange = new List<ColumnModel>();

                foreach (var _column in LaboratoryBookColumns)
                {
                    var col = new ColumnModel();
                    col.ColumnName = _column.ColumnName;
                    col.ColumnType = _column.ColumnType;

                    ColumnListBeforeChange.Add(col);
                }
            }
            catch (Exception exception)
            {                
                MessageBox.Show
                           (
                              exception.Message,
                              "Column update error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error
                            );

                this.LaboratoryBookColumns.Clear();

                foreach (var _column in ColumnListBeforeChange)
                {
                    var col = new ColumnModel();
                    col.ColumnName = _column.ColumnName;
                    col.ColumnType = _column.ColumnType;
                    col.PropertyChanged += Column_PropertyChanged;
                    LaboratoryBookColumns.Add(col);
                }
                this.SelectedColumn = LaboratoryBookColumns[0];
            }
            finally
            {
                await connection.CloseAsync();
                sqlCommand?.Dispose();
            }
        }
        
        
        private ListModel GetListModel(string listName, string laboratoryBookName)
        {
            
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);
            
            var sqlCommand = new MySqlCommand()
            {
                Connection = connection,
                CommandText = $"SELECT `{listName}` FROM `{listName}s_{laboratoryBookName}`;"
            };

            connection.Open();

            var dbReader  = sqlCommand.ExecuteReader();
            var dataTable = new DataTable();
            dataTable.Load(dbReader);  
            
            var listModel = new ListModel();
          
            foreach(DataRow row in dataTable.Rows)
            {
                var value = new ListValueModel()
                {
                    ListValue    = (string)row[$"{listName}"],
                    OldListValue = (string)row[$"{listName}"]
                }; 
                
                listModel.Values.Add(value);
            }
            listModel.ListName = listName;

            if (listModel.Values.Any()) listModel.SelectedValue = listModel.Values.First();

            connection.Close();
            sqlCommand?.Dispose();

            return listModel;                 
           
        }

        //get columns which can be modified

        private ObservableCollection<ColumnModel> GetColumns(int laboratoryBookID)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);

            var sqlCommand = new MySqlCommand()
            {
                Connection  = connection,
                CommandText = $"SELECT db_name FROM db_list WHERE db_id = '{laboratoryBookID}'; "
            };

            try
            {
                connection.Open();

                var laboratoryBookName = (string) sqlCommand.ExecuteScalar();

                sqlCommand.CommandText = $"SHOW columns FROM laboratory_book_{laboratoryBookName}; ";

                var dataBaseReader = sqlCommand.ExecuteReader();
                var columnsTable = new DataTable();
                columnsTable.Load(dataBaseReader);

                var ColumnList = new ObservableCollection<ColumnModel>();
                foreach (DataRow row in columnsTable.Rows)
                {
                    var column = new ColumnModel()
                    {
                        ColumnName = (string)row[0],
                        ColumnType = (string)row[1]
                    };
                    if (!ForbidenToModifyColumnNames.Contains(column.ColumnName))
                    {
                        ColumnList.Add(column);                        
                    }
                }
                
                return ColumnList;

            }
            catch(Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Get columns error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error
                            );
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }
            return null;
        }

        //get all columns from laboratory book

        public ObservableCollection<ColumnModel> GetAllColumns(int laboratoryBookID)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var sqlCommand = new MySqlCommand()
            {
                Connection = connection,
                CommandText = $"SELECT db_name FROM db_list WHERE db_id = '{laboratoryBookID}'; "
            };

            try
            {
                connection.Open();

                var laboratoryBookName = (string)sqlCommand.ExecuteScalar();

                sqlCommand.CommandText = $"SHOW columns FROM laboratory_book_{laboratoryBookName}; ";

                var dataBaseReader = sqlCommand.ExecuteReader();
                var columnsTable = new DataTable();
                columnsTable.Load(dataBaseReader);

                var ColumnList = new ObservableCollection<ColumnModel>();
                foreach (DataRow row in columnsTable.Rows)
                {
                    var column = new ColumnModel()
                    {
                        ColumnName = (string)row[0],
                        ColumnType = (string)row[1]
                    };

                    ColumnList.Add(column);
                }
                
                return ColumnList;

            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Get columns error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error
                            );
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }
            return null;
        }

        //get book ID of current laboratory book

        private int GetLaboratoryBookID(string laboratoryBookName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = "SELECT db_id " +
                                "FROM db_list " +
                                $"WHERE db_name = '{laboratoryBookName}';";
            var sqlCommand = new MySqlCommand(commandString, connection);

            int? laboratoryBookID = null; 
            try
            {
                connection.Open();
                laboratoryBookID = (int) (sqlCommand.ExecuteScalar());                              

            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Get book ID error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error
                            );
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }

            return (int) laboratoryBookID;
        }

        //get a set of laboratory book permissions which are common for all laboratory book

        private ObservableCollection<sbyte> GetPermissions()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);

            var commandString = "SELECT permission_id FROM permission; ";
            var sqlCommand    = new MySqlCommand(commandString, connection);

            var permissionDataTable = new DataTable();
            var result              = new ObservableCollection<sbyte>();

            try
            {
                connection.Open();

                var dbReader = sqlCommand.ExecuteReader();
                permissionDataTable.Load(dbReader);

                foreach (DataRow row in permissionDataTable.Rows)
                {                    
                    result.Add((sbyte)row[0]);
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Search error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error
                            );
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }
            return result;
        }

        //get users which have an access to this laboratory book

        private ObservableCollection<UserModel> GetUsers(int laboratoryBookID)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);

            var commandStringBuilder = new StringBuilder();

            
            commandStringBuilder.Append("SELECT users.user_id, user_name, permission_id ");
            commandStringBuilder.Append("FROM users ");
            commandStringBuilder.Append("JOIN db_users ");
            commandStringBuilder.Append("ON users.user_id = db_users.user_id ");            
            commandStringBuilder.Append($"WHERE db_users.db_id = '{laboratoryBookID}';");

            var commandString = commandStringBuilder.ToString();
            var sqlCommand    = new MySqlCommand(commandString, connection);

            var usersDataTable = new DataTable();
            var result         = new ObservableCollection<UserModel>();

            try
            {
                connection.Open();

                var dbReader = sqlCommand.ExecuteReader();
                usersDataTable.Load(dbReader);
                
                foreach(DataRow row in usersDataTable.Rows)
                {
                    
                    var user = new UserModel()
                    {
                        UserID = (int)row[0],
                        UserName = (string)row[1],
                        PermissionID = (sbyte)row[2] 
                    };
                   
                    if (user.UserID != LaboratoryBookUser.UserID)
                    {                        
                        result.Add(user);
                    }
                }
                  
            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Search error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error
                            );
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }

            return result;
        }
    }
}
