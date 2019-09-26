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

namespace LaboratoryBook.ModifyUsersWindow
{
    /// <summary>
    /// Interaction logic for ModifyUsersWindow.xaml
    /// </summary>
    public partial class ModifyUsersWindow : Window
    {
        public User LaboratoryBookUser { get; set; }
        public ModifyUsersViewModel MainViewModel { get; set; }        
        
        public ModifyUsersWindow(ref User user)
        {
            InitializeComponent();

            this.LaboratoryBookUser = user;

            this.MainViewModel = new ModifyUsersViewModel(user);

            this.DataContext = this.MainViewModel;
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (button.Content.ToString()=="Cancel")
            {
                this.Close();
            }
            else if (button.Content.ToString()=="Create")
            {
                var mainViewModel = this.MainViewModel;
                var createUserWindow = new CreateUserWindow.CreateUserWindow(ref mainViewModel);
                createUserWindow.Owner = this;
                createUserWindow.ShowDialog();
            }
            else if (button.Content.ToString()=="Remove")
            {               
                var userToRemove = MainViewModel.SelectedUser;

                var dialogResult = MessageBox.Show(
                   $"Do you want to delete user '{userToRemove.UserName}'? ",
                   "User deleting",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Warning);

                if (dialogResult == MessageBoxResult.No)
                {
                    return;
                }
                    
                try
                {                   
                    var result = ((IAdvancedUser)LaboratoryBookUser).RemoveUser(userToRemove.UserId);
                    if (result)
                    {
                        userToRemove.PropertyChanged -= MainViewModel.User_PropertyChanged;
                        this.MainViewModel.UserList.Remove(userToRemove);
                        this.MainViewModel.SelectedUser = this.MainViewModel.UserList.First();
                    }
                }
                catch(Exception exception)
                {
                    MessageBox.Show(
                        exception.Message,
                        "User delete error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }        
    }
}
