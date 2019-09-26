using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBook.ModifyUsersWindow
{
    public class AccessStatusModel:ViewModelBase
    {
        private string accessName;

        public string AccessName
        {
            get
            {
                return accessName;
            }
            set
            {
                accessName = value;
                OnPropertyChanged();
            }
        }

        private int accessId;

        public int AccessId
        {
            get
            {
                return accessId;
            }
            set
            {
                accessId = value;
                OnPropertyChanged();
            }
        }

        public AccessStatusModel( int _accessId, string _accessName)
        {
            this.AccessId   = _accessId;
            this.AccessName = _accessName;            
        }
    }
}

