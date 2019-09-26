using LaboratoryBook.ModifyUsersWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBook.UserClass
{
    public interface IAdvancedUser
    {
       
        ObservableCollection<ModifyUserModel> GetAvailableUsers();
        
        ObservableCollection<AccessStatusModel> GetAccessStatusList();
        bool ChangeUserStatus(ModifyUserModel changedUser);

        bool RemoveUser(int userId);

        int CreateUser(string userName, string password, int accessId);



    }
}
