using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LaboratoryBook
{
    public static class MainWindowCommands
    {
        public static RoutedUICommand Refresh = new RoutedUICommand
            (
            "Refresh data table",
            "Refresh",
            typeof(MainWindowCommands),
            new InputGestureCollection()
                {

                    new KeyGesture(Key.R, ModifierKeys.Control)
                }
            );

        public static RoutedUICommand ModifyDatabase = new RoutedUICommand
            (
            "Modify database",
            "ModifyDatabase",
            typeof(MainWindowCommands)            
            );

        public static RoutedUICommand SetColumns = new RoutedUICommand
            (
            "Refresh data table",
            "Refresh",
            typeof(MainWindowCommands)
            );

        public static RoutedUICommand ModifyUserInfo = new RoutedUICommand
            (
            "Modify user info",
            "ModifyUserInfo",
            typeof(MainWindowCommands)
            );

        public static RoutedUICommand ModifyUsers = new RoutedUICommand
            (
            "Modify users ",
            "ModifyUsers",
            typeof(MainWindowCommands)
            );

        public static RoutedUICommand Search = new RoutedUICommand
            (
            "Search in each column of database",
            "Search",
            typeof(MainWindowCommands), 
            new InputGestureCollection()
                {
                    new KeyGesture(Key.Enter)
                }
            );
    }
}
