using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryBook
{
    public partial class MainWindow : Window
    {
        //Refresh command implementation

        public void RefreshCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public async void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {            
            try
            {
                await SetLists();
                

                this.LaboratoryDataTable = await Task.Run(() =>
                {
                    return User.GetDataFromLaboratoryBook(LaboratoryBookName, LaboratoryBookPermission);
                });

                this.LaboratoryDataTable.RowChanged += LaboratoryDataTableRowChanged;
                this.LaboratoryDataTable.RowDeleting += LaboratoryDataTableRowDeleting;
                

            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                               exception.Message,
                               "Refresh error",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error
                           );
            }
        }

        //Search command implementation

        public void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {                           
                e.CanExecute = true;   
        }

        public async void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var bookName = this.LaboratoryBookName;
                var columnNames = await Task.Run(() =>
                {
                    return User.GetColumnNames(bookName);
                });

                var searchText = TbxSearch.Text;

                await SetLists();

                this.LaboratoryDataTable = await Task.Run(() =>
                {
                    return User.SearchInLaboratoryBook(
                        LaboratoryBookName,
                        LaboratoryBookPermission,
                        columnNames.ToArray(),
                        searchText);
                });

                this.LaboratoryDataTable.RowChanged += LaboratoryDataTableRowChanged;
                this.LaboratoryDataTable.RowDeleting += LaboratoryDataTableRowDeleting;
            }
            catch(Exception exception)
            {
                MessageBox.Show
                         (
                             exception.Message,
                             "Search error",
                             MessageBoxButton.OK,
                             MessageBoxImage.Error
                         );
            }
        }
        //set columns implementation

        public void SetColumnsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void SetColumnsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectColumsWindow = new SelectColumnsWindow.SelectColumnsWindow(ref DgLaboratoryBook)
            {
                Owner = this
            };
            selectColumsWindow.Show();
        }

        //modify database implementation

        public void ModifyDatabaseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public async void ModifyDatabaseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var user       = this.User;
            var bookName   = this.LaboratoryBookName;
            var permission = this.LaboratoryBookPermission;
            var searchText = TbxSearch.Text;

            var modifyDatabaseWindow = new ModifyDatabaseWindow.ModifyDatabaseWindow(ref user, bookName);
            modifyDatabaseWindow.Owner = this;
            modifyDatabaseWindow.ShowDialog();

            try
            {               
                var columnNames = await Task.Run(() =>
                {
                    return User.GetColumnNames(bookName);
                });               

                await SetLists();

                this.LaboratoryDataTable = await Task.Run(() =>
                {
                    return User.SearchInLaboratoryBook(
                        bookName,
                        permission,
                        columnNames.ToArray(),
                        searchText);
                });

                LaboratoryDataTable.RowChanged += LaboratoryDataTableRowChanged;
                LaboratoryDataTable.RowDeleting += LaboratoryDataTableRowDeleting;

                //this.DgLaboratoryBook.PreviewKeyDown += DgLaboratoryBookPreviewKeyDown;

                //this.DgLaboratoryBook.CellEditEnding += DgLaboratoryBook_CellEditEnding;

            }
            catch (Exception exception)
            {
                MessageBox.Show
                           (
                               exception.Message,
                               "Refresh error",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error
                           );
            }

        }

        //modify user info command implementation

        public void ModifyUserInfoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public async void ModifyUserInfoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var user = this.User;
            var userWindow = new UserWindow.UserWindow(ref user)
            {
                Owner = this
            };

            userWindow.ShowDialog();

            if (!userWindow.NameChanged) return;

            try
            {
                var bookName = this.LaboratoryBookName;
                var columnNames = await Task.Run(() =>
                {
                    return User.GetColumnNames(bookName);
                });

                var searchText = TbxSearch.Text;

                await SetLists();

                this.LaboratoryDataTable = await Task.Run(() =>
                {
                    return User.SearchInLaboratoryBook(
                        LaboratoryBookName,
                        LaboratoryBookPermission,
                        columnNames.ToArray(),
                        searchText);
                });



                this.LaboratoryDataTable.RowChanged += LaboratoryDataTableRowChanged;
                this.LaboratoryDataTable.RowDeleting += LaboratoryDataTableRowDeleting;

                //this.DgLaboratoryBook.PreviewKeyDown += DgLaboratoryBookPreviewKeyDown;

                //this.DgLaboratoryBook.CellEditEnding += DgLaboratoryBook_CellEditEnding;
            }
            catch (Exception exception)
            {
                MessageBox.Show
                         (
                             exception.Message,
                             "Search error",
                             MessageBoxButton.OK,
                             MessageBoxImage.Error
                         );
            }
        }

        //modify users command implementation

        public void ModifyUsersCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public async void ModifyUsersCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var user = this.User;
            var userWindow = new ModifyUsersWindow.ModifyUsersWindow(ref user)
            {
                Owner = this
            };

            userWindow.ShowDialog();

            try
            {
                var bookName = this.LaboratoryBookName;
                var columnNames = await Task.Run(() =>
                {
                    return User.GetColumnNames(bookName);
                });

                var searchText = TbxSearch.Text;

                await SetLists();

                this.LaboratoryDataTable = await Task.Run(() =>
                {
                    return User.SearchInLaboratoryBook(
                        LaboratoryBookName,
                        LaboratoryBookPermission,
                        columnNames.ToArray(),
                        searchText);
                });


                this.LaboratoryDataTable.RowChanged += LaboratoryDataTableRowChanged;
                this.LaboratoryDataTable.RowDeleting += LaboratoryDataTableRowDeleting;
                
            }
            catch (Exception exception)
            {
                MessageBox.Show
                         (
                             exception.Message,
                             "Search error",
                             MessageBoxButton.OK,
                             MessageBoxImage.Error
                         );
            }

        }
    }
}
