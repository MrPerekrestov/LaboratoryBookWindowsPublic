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

namespace LaboratoryBook.ModifyUsersWindow.CreateUserWindow
{
    /// <summary>
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        private ModifyUsersViewModel modifyUsersViewModel { get; set; }
        public CreateUserWindow(ref ModifyUsersViewModel mainViewModel)
        {
            InitializeComponent();
            this.modifyUsersViewModel = mainViewModel;
            this.DataContext = mainViewModel.StatusList;

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button   = (Button)sender;
            var password = TbxPassword.Text;
            var userName = TbxUserName.Text;
            var statusId = ((AccessStatusModel)CbxStatus.SelectedItem).AccessId;

            var user = this.modifyUsersViewModel.LaboratoryBookUser as IAdvancedUser;

            if (button.Content.ToString() == "Cancel")
            {
                this.Close();
            }
            else if (button.Content.ToString() == "Create")
            {
                if (!CheckData()) return;
                try
                {
                    var createUserResult = await Task.Run(()=>
                    {
                        return user.CreateUser(userName, password, statusId);
                    });
                    if (createUserResult == 0) return;

                    var id = await Task.Run(() =>
                    {
                        return GetUserId(userName);
                    });

                    var userModel = new ModifyUserModel(id, userName, statusId);
                    userModel.PropertyChanged += modifyUsersViewModel.User_PropertyChanged;
                    modifyUsersViewModel.UserList.Add(userModel);
                    MessageBox.Show(
                        $"User '{userName}' was successfully created!",
                        "User creation",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch(Exception exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        "Create user error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }                             

            }
        }

        private int GetUserId(string userName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cs_login"].ConnectionString;
            var connection = new MySqlConnection(connectionString);

            var commandString = $"SELECT `user_id` FROM users WHERE user_name = '{userName}'; ";
            var sqlCommand = new MySqlCommand(commandString, connection);

            try
            {
                connection.Open();
                var id = (int)sqlCommand.ExecuteScalar();
                return (id);
            }
            finally
            {
                connection.Close();
                sqlCommand?.Dispose();
            }
        }
        private bool CheckData()
        {
            var password = TbxPassword.Text;
            var userName = TbxUserName.Text;

            if (!(userName.All(Char.IsLetterOrDigit))
               || (userName.Length < 2)
               || (userName.Length > 20))
            {
                MessageBox.Show(
                    "Login length should be 2-20 chartacters, use letters or digits",
                    "Login input error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            if (password.Length<7)
            {
                MessageBox.Show(
                    "Password length should be more than 6 chartacters",
                    "Password input error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            return true;
        }
       
    }
}
