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

namespace LaboratoryBook.UserWindow.ChangeNameWindow
{
    /// <summary>
    /// Interaction logic for ChangeNameWindow.xaml
    /// </summary>
    public partial class ChangeNameWindow : Window
    {
        private User LaboratoryBookUser { get; set; }

        public static readonly DependencyProperty UserNameProperty =
           DependencyProperty.Register
                             (
                                  "UserName",
                                   typeof(string),
                                   typeof(ChangeNameWindow)
                             );
        public string UserName
        {
            get { return (string)this.GetValue(UserNameProperty); }
            set { this.SetValue(UserNameProperty, value); }
        }

        public ChangeNameWindow(ref User user)
        {
            InitializeComponent();
            this.LaboratoryBookUser = user;
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var userName = this.UserName;
            var laboratoryBookUser = this.LaboratoryBookUser;
            
            if ((string)button.Content == "Cancel")
            {
                this.Close();
            }
            else
            {
                if (Validation.GetErrors(TbxName).Count > 0) return;

                int result = 0;
                var updateUserNameTask = new Task<int>(()=>
                {                   
                    var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
                    var connection = new MySqlConnection(connectionString);

                    var commandString = $"UPDATE `users` SET `user_name` = '{userName}' " +
                                        $"WHERE (`user_id` = '{laboratoryBookUser.GetUserID()}'); ";
                    var sqlCommand = new MySqlCommand(commandString, connection);

                    connection.Open();

                    var rowsChanged = sqlCommand.ExecuteNonQuery();                   

                    connection.Close();

                    return rowsChanged;
                });
                try
                {
                    updateUserNameTask.Start();
                    result = await updateUserNameTask;
                    if (result>0)
                    {
                        this.LaboratoryBookUser.UserName = this.UserName;
                       ((UserWindow)(this.Owner)).NameChanged = true;
                        MessageBox.Show(
                            "Name was successfully changed",
                            "Change Name",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                        "Name was not changed, please, select another name",
                        "Change name error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        "Change name error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
}
