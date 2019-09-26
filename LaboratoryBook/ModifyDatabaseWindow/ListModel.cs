using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBook.ModifyDatabaseWindow
{
    public class ListModel:ViewModelBase
    {
        private string listName;
        public string ListName
        {
            get
            {
                return listName;
            }
            set
            {
                listName = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ListValueModel> values;
        public ObservableCollection<ListValueModel> Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
                OnPropertyChanged();
            }        
        }

        private ListValueModel selectedValue;
        public ListValueModel SelectedValue
        {
            get
            {
                return selectedValue;
            }
            set
            {
                selectedValue = value;
                OnPropertyChanged();
            }
        }

        public ListModel()
        {
            this.Values        = new ObservableCollection<ListValueModel>();
            this.ListName      = String.Empty;
            this.SelectedValue = new ListValueModel();
        }
    }
}
