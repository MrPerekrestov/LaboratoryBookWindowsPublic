using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBook.ModifyDatabaseWindow
{
    public class UserModel:ViewModelBase
    {
        private int userID;
        public int UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
                OnPropertyChanged();
            }
        }

        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged();
            }
        }

        private sbyte permissionID;
        public sbyte PermissionID
        {
            get
            {
                return permissionID;
            }
            set
            {
                permissionID = value;

                OnPropertyChanged();
            }
        }       
    }
}
