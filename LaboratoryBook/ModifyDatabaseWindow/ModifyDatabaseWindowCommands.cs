using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LaboratoryBook.ModifyDatabaseWindow
{
    public class ModifyDatabaseWindowCommands
    {
        public static RoutedUICommand Close = new RoutedUICommand
                (
                    "Close window",
                    "Close",
                    typeof(ModifyDatabaseWindowCommands),
                    new InputGestureCollection()
                        {
                            new KeyGesture(Key.Escape)
                        }
                );

        public static RoutedUICommand AddUser = new RoutedUICommand
                (
                    "Add user",
                    "AddUser",
                    typeof(ModifyDatabaseWindowCommands)
                );

        public static RoutedUICommand RemoveUser = new RoutedUICommand
               (
                   "Remove user",
                   "RemoveUser",
                   typeof(ModifyDatabaseWindowCommands)
               );

        public static RoutedUICommand AddColumn = new RoutedUICommand
                (
                    "Add column",
                    "AddColumn",
                    typeof(ModifyDatabaseWindowCommands)
                );

        public static RoutedUICommand RemoveColumn = new RoutedUICommand
               (
                   "Remove column",
                   "RemoveColumn",
                   typeof(ModifyDatabaseWindowCommands)
               );

        public static RoutedUICommand RemoveListValue = new RoutedUICommand
               (
                   "Remove ListValue",
                   "RemoveListValue",
                   typeof(ModifyDatabaseWindowCommands)
               );

        public static RoutedUICommand AddListValue = new RoutedUICommand
              (
                  "Add ListValue",
                  "AddListValue",
                  typeof(ModifyDatabaseWindowCommands)
              );


    }
}
