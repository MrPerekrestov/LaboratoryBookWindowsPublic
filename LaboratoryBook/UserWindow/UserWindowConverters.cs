using LaboratoryBook.UserClass;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LaboratoryBook.UserWindow
{
    public class UserClassToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = value as User;

            if (user is Administer) return "Administer";
            if (user is Moderator)  return "Moderator";
            if (user is Laborant)   return "Laborant";
            if (user is Guest)      return "Guest";

            return "User";            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
