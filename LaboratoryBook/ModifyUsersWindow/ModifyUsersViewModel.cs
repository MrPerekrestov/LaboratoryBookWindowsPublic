using LaboratoryBook.UserClass;
using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBook.ModifyUsersWindow
{
    public class ModifyUsersViewModel:ViewModelBase
    {
        public User LaboratoryBookUser { get; set; }

        public ObservableCollection<AccessStatusModel> StatusList { get; set; }
        public ObservableCollection<ModifyUserModel> UserList { get; set; }

        private ModifyUserModel selectedUser;
        public ModifyUserModel SelectedUser
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


        public ModifyUsersViewModel(User user)
        {
            this.LaboratoryBookUser = user;

            this.StatusList = ((IAdvancedUser)user).GetAccessStatusList();

            this.UserList = ((IAdvancedUser)user).GetAvailableUsers();                  

            if (this.UserList.Any()) this.SelectedUser = this.UserList[0];
            foreach (var _user in this.UserList)
            {
                _user.PropertyChanged += User_PropertyChanged;
            }
        }

        public void User_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                var changedUserInfo = sender as ModifyUserModel;
                var userAsAdvanced = this.LaboratoryBookUser as IAdvancedUser;
                var changeOfStatusResult = userAsAdvanced.ChangeUserStatus(changedUserInfo);
            }
            catch(Exception exception)
            {
                MessageBox.Show(
                    exception.Message,
                    "User status changing",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            
        }
    }
}
