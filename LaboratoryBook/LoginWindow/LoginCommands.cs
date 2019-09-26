using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LaboratoryBook.LoginWindow
{
    public static class LoginCommands
    {

        public static RoutedUICommand Close = new RoutedUICommand
            (
                "Close login window",
                "Close",
                typeof(LoginCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Escape)
                }
            );


        public static RoutedUICommand Login = new RoutedUICommand
            (
                "Login to database",
                "Login",
                typeof(LoginCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Enter)
                }
            );

        public static RoutedUICommand Connect = new RoutedUICommand
            (
                "Connect to database",
                "Connect",
                typeof(LoginCommands)
            );

        public static RoutedUICommand New = new RoutedUICommand
            (
                "Create new laboratory book",
                "New",
                typeof(LoginCommands)
            );
    }
}
