using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LaboratoryBook.ModifyUsersWindow
{
    public class NumberToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case 1: return "Guest";
                case 2: return "Laborant";
                case 3: return "Moderator";
                case 4: return "Administer";
            }
            return "error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)value)
            {
                case "Guest": return 1;
                case "Laborant": return 2;
                case "Moderator": return 3;
                case "Administer": return 4;
            }
            return 1;
        }
    }
}
