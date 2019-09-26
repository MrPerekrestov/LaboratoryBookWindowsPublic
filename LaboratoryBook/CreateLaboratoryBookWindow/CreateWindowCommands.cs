using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LaboratoryBook.CreateLaboratoryBookWindow
{
    public static class CreateWindowCommands
    {
        public static RoutedUICommand Close = new RoutedUICommand
            (
            "Close create window table",
            "Close",
            typeof(CreateWindowCommands),
            new InputGestureCollection()
                {
                    new KeyGesture(Key.Escape)
                }
            );
        public static RoutedUICommand Create = new RoutedUICommand
            (
            "Create new database",
            "Create",
            typeof(CreateWindowCommands),
            new InputGestureCollection()
                {
                    new KeyGesture(Key.Enter)
                }
            );
    }
}
