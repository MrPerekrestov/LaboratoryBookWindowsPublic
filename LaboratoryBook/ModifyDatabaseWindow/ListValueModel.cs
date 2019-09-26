using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBook.ModifyDatabaseWindow
{
    public class ListValueModel:ViewModelBase
    {
        private string listValue;
        public string ListValue
        {
            get
            {
                return listValue;
            }
            set
            {
                listValue = value;
                OnPropertyChanged();
            }
        }
        public string OldListValue { get; set; }
        
        public ListValueModel()
        {
            listValue = String.Empty;
        }

    }
}
