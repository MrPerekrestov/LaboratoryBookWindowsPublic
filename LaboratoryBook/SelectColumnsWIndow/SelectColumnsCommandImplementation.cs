using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LaboratoryBook.SelectColumnsWindow
{ 

    public partial class SelectColumnsWindow : Window
    {

        //remove command implementation
        public void RemoveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((LvSelectedColumns?.SelectedItems?.Count > 0))
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        public void RemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = new List<object>();

            foreach (var selectedItem in LvSelectedColumns.SelectedItems)
            {
                selectedItems.Add(selectedItem);
            }   
            
            foreach (var selectedItem in selectedItems)
            {
                LvSelectedColumns.Items.Remove(selectedItem);
            }
            CanApply = true;            
        }
        //add command implementation

        public void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((LvAllColumns?.SelectedItems?.Count > 0))
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        public void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = new List<string>();

            foreach (var selectedItem in LvAllColumns.SelectedItems)
            {
                selectedItems.Add((string)selectedItem);
            }

            var existingItems = new List<string>();

            foreach (var item in LvSelectedColumns.Items)
            {
                existingItems.Add((string)item);
            }

            foreach (var selectedItem in selectedItems)
            {
                if (!existingItems.Contains(selectedItem))
                {
                    LvSelectedColumns.Items.Add(selectedItem);
                }
                else continue;                 
            }
            CanApply = true;
        }

        //apply command implementation

        public void ApplyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CanApply)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        public void ApplyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            bool columnIsVisible = false;

            foreach (var column in DataGrid.Columns)
            {
                foreach(var selectedColumn in LvSelectedColumns.Items)
                {
                    if ((string)column.Header ==(string)selectedColumn)
                    {                        
                        column.Visibility = Visibility.Visible;
                        columnIsVisible = true;
                        break;
                    }
                }

                if (!columnIsVisible)
                {
                    column.Visibility = Visibility.Collapsed;
                }

                columnIsVisible = false;
            }            
            
            CanApply = false;           
        }

        //Close command implementation

        public void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
