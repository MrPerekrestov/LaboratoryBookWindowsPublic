using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LaboratoryBook.UserWindow.ChangeNameWindow
{
    public class ChangeNameValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            
            if ((value.ToString().All(Char.IsLetterOrDigit))
                &&(value.ToString().Length>2)
                && (value.ToString().Length <21))
            {
                return ValidationResult.ValidResult;
               
            }
            else if ((value.ToString().Length < 3) || (value.ToString().Length > 20))
            {
                return new ValidationResult(false, "length shoud be in a range of 2-20 characters");
            }
            else
            {
                return new ValidationResult(false, "Use alphabetic characters");
            }
        }
    }
}
