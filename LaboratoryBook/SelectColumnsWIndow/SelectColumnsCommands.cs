using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LaboratoryBook.SelectColumnsWindow
{
   public static class SelectColumnsCommands
    {
        public static RoutedUICommand Remove = new RoutedUICommand
           (
           "Remove item from list",
           "Remove",
           typeof(SelectColumnsCommands),
           new InputGestureCollection()
               {

                    new KeyGesture(Key.Right)
               }
           );

        public static RoutedUICommand Add = new RoutedUICommand
           (
           "Add item item to list",
           "Add",
           typeof(SelectColumnsCommands),
           new InputGestureCollection()
               {

                    new KeyGesture(Key.Right)
               }
           );

        public static RoutedUICommand Apply = new RoutedUICommand
          (
          "Apply changes",
          "Apply",
          typeof(SelectColumnsCommands),
          new InputGestureCollection()
              {

                    new KeyGesture(Key.Right)
              }
          );

        public static RoutedUICommand Close = new RoutedUICommand
          (
          "Close window",
          "Close",
          typeof(SelectColumnsCommands),
          new InputGestureCollection()
              {

                    new KeyGesture(Key.Escape)
              }
          );
    }
}
