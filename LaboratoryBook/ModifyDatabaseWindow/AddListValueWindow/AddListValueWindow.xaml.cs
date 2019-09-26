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

namespace LaboratoryBook.ModifyDatabaseWindow.AddListValueWindow
{
    /// <summary>
    /// Interaction logic for AddListValueWindow.xaml
    /// </summary>
    public partial class AddListValueWindow : Window
    {
        private ModifyDatabaseViewModel MainViewModel { get; set; }

        public AddListValueWindow(ref ModifyDatabaseViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            
            if (clickedButton.Content.ToString() == "Close")
            {
                this.Close();
            }
            
            if (clickedButton.Content.ToString() == "Add")
            {
                
                if (string.IsNullOrEmpty(TbxNewListValue.Text)||
                    string.IsNullOrWhiteSpace(TbxNewListValue.Text))
                {
                    MessageBox.Show
                          (
                             "Input error",
                             "New list value should not be emplty string",
                             MessageBoxButton.OK,
                             MessageBoxImage.Error
                           );
                    return;
                }

                var selectedList = this.MainViewModel.SelectedList;

                var newListValue = new ListValueModel()
                {
                    ListValue    = TbxNewListValue.Text,
                    OldListValue = TbxNewListValue.Text
                };                

                var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
                var connection       = new MySqlConnection(connectionString);        

                var commandString = $"INSERT INTO `{selectedList.ListName}s_{MainViewModel.LaboratoryBookName}` (`{selectedList.ListName}`) " +
                                    $"VALUES ('{newListValue.ListValue}');"; 

                var sqlCommand = new MySqlCommand(commandString, connection);

                try
                {
                    await connection.OpenAsync();

                    var result = await sqlCommand.ExecuteNonQueryAsync();
                    if (result>0)
                    {
                        newListValue.PropertyChanged += MainViewModel.ListModel_PropertyChanged;
                        selectedList.Values.Add(newListValue);
                    }

                    this.Close();

                }
                catch(Exception exception)
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
}
