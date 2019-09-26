using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LaboratoryBook.UserWindow
{
    public static class UserWindowCommands
    {
        public static RoutedUICommand Close = new RoutedUICommand
          (
          "Remove item from list",
          "Remove",
          typeof(UserWindowCommands),
          new InputGestureCollection()
              {

                    new KeyGesture(Key.Escape)
              }
          );

        public static RoutedUICommand ChangeName = new RoutedUICommand
          (
          "Change user name",
          "ChageName",
          typeof(UserWindowCommands)         
          );

        public static RoutedUICommand ChangePassword = new RoutedUICommand
          (
          "Change user password",
          "ChangePassword",
          typeof(UserWindowCommands)
          );
    }
}
