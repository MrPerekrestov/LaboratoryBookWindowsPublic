using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBook.ModifyDatabaseWindow
{
    public class ColumnModel:ViewModelBase
    {
        private string columnName;

        public string ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                columnName = value;
                OnPropertyChanged();
            }
        }

        private string columnType;

        public string ColumnType
        {
            get
            {
                return columnType;
            }
            set
            {
                columnType = value;
                OnPropertyChanged();
            }
        }
    }
}
