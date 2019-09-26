using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace LaboratoryBook.ModifyDatabaseWindow.AddColumnWindow
{
    
    public partial class AddColumnWindow : Window
    {
        private ModifyDatabaseViewModel MainViewModel { get; set; }

        private ObservableCollection<ColumnModel> ExistingColumns { get; set; }

        public AddColumnWindow(ref ModifyDatabaseViewModel mainViewModel)
        {
            InitializeComponent();

            this.MainViewModel = mainViewModel;

            this.ExistingColumns          = MainViewModel.GetAllColumns(MainViewModel.LaboratoryBookID);
            this.CbxColumns.ItemsSource   = this.ExistingColumns;
            this.CbxColumns.SelectedIndex = ExistingColumns.Count - 1;

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
           
            if (button.Content.ToString() == "Close")
            {
                this.Close();
            }

            if (button.Content.ToString() == "Add")
            {
                var newColumn = await AddNewColumn();
                if (newColumn != null)
                {
                    newColumn.PropertyChanged += MainViewModel.Column_PropertyChanged;
                    MainViewModel.LaboratoryBookColumns.Add(newColumn);
                    
                    var copyOfNewColumn = new ColumnModel()
                    {
                        ColumnName = newColumn.ColumnName,
                        ColumnType = newColumn.ColumnType
                    };

                    MainViewModel.ColumnListBeforeChange.Add(copyOfNewColumn);
                }
                this.Close();
            }
        }

        private async Task<ColumnModel> AddNewColumn()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection       = new MySqlConnection(connectionString);
            
            var afterColumn = CbxColumns.SelectedItem as ColumnModel;

            var columnName    = TbxColumnName.Text;
            var columnType    = TbxColumnType.Text;
            var commandString = $"ALTER TABLE `laboratory_book_{MainViewModel.LaboratoryBookName}` "+
                                $"ADD COLUMN `{columnName}` {columnType} NULL AFTER `{afterColumn.ColumnName}`; ";           

            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {
                await connection.OpenAsync();

                var result = await sqlCommand.ExecuteNonQueryAsync();


                return new ColumnModel()
                {
                    ColumnName = columnName,
                    ColumnType = columnType
                };
            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                              exception.Message,
                              "Add column error",
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
    }
}
