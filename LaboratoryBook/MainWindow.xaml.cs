using LaboratoryBook.UserClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;
using System.Timers;

namespace LaboratoryBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer _timerCheckUpdated { get; set; } = new Timer();
        private DateTime _lastUpdated { get; set; }
        public TaskbarIcon TaskBarIcon { get; set; } = new TaskbarIcon();       
        public User        User                     { get; set; }
        public Permission  LaboratoryBookPermission { get; set; }
        public string      LaboratoryBookName       { get; set; }
               
        public static readonly DependencyProperty LaboratoryDataTableProperty = 
            DependencyProperty.Register
                              (
                                   "LaboratoryDataTable",
                                    typeof(DataTable),
                                    typeof(MainWindow)
                              );                                                                                                                  
        public DataTable LaboratoryDataTable
        {
            get { return (DataTable)this.GetValue(LaboratoryDataTableProperty); }
            set { this.SetValue(LaboratoryDataTableProperty, value); }
        }

        private bool DeleteHandled = false;

        public ObservableCollection<object> Regimes { get; set; } 
        public ObservableCollection<object> Operators { get; set; }
        public ObservableCollection<object> Materials { get; set; }
        public ObservableCollection<object> Substrates { get; set; }
        public ObservableCollection<object> PermssionIDs { get; set; }

        public MainWindow(User user, Permission laboratoryBookPermission, string laboratoryBookName)
        {
            this.User                     = user;           
            this.LaboratoryBookPermission = laboratoryBookPermission;
            this.LaboratoryBookName       = laboratoryBookName;            

            InitializeComponent();
            
            this.Loaded += MainWindowLoaded;
            this.StateChanged += (sender, args) =>
            {
                if (this.WindowState == WindowState.Minimized) this.Hide();
            };

            this.WindowState = WindowState.Maximized;
            
        }

        private async void DgLaboratoryBook_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            var laboratoryBookName = this.LaboratoryBookName;
            
            var header = e.Column.Header.ToString();
            var row = e.Row.Item as DataRowView;
            
            object changingValue = null;
            if (e.EditingElement is ComboBox comboBox)
            {
                changingValue = comboBox.SelectedValue;
            }
            else if (e.EditingElement is TextBox textBox)
            {
                changingValue = textBox.Text;

            }            
            var sampeId = row[0];

            bool changeCellResult = false;
            try
            {           
                changeCellResult = await Task.Run(() =>
                {
                    return MainWindowHelper.ChangeCell(laboratoryBookName, header, changingValue, sampeId);
                });

                await MainWindowHelper.UpdateStatistics(LaboratoryBookName, User);
            }
            catch (Exception exception)
            {
                MessageBox.Show
                        (
                            exception.Message,
                            "Cell change error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
            }
        }

        public MainWindow(User user)
        {
            this.User = user;
            
            InitializeComponent();

            CreateTaskBarIcon();
            this.StateChanged += (sender, args) =>
            {
                if (this.WindowState == WindowState.Minimized) this.Hide();
            };

            this.WindowState = WindowState.Maximized;
        }

        //set up preferences for timer which send update notifications
        public void SetUpAndStartTimer()
        {
            _timerCheckUpdated.Interval = 3000;
            _timerCheckUpdated.Elapsed += async (obj, args) =>
            {
                string[] result = await MainWindowHelper.CheckUpdatesAsync(LaboratoryBookName);
                if (_lastUpdated != DateTime.Parse(result[1])&(User.UserName!=result[0].ToString()))
                {
                    TaskBarIcon.ShowBalloonTip
                    (
                        "Book updates",
                        $"Database was changed by {result[0]} ({result[1]})",
                        BalloonIcon.Info
                    );
                }
                _lastUpdated = DateTime.Parse(result[1]);
            };
            _timerCheckUpdated.Start();
        }

        //create task bar icon and set up context menu 
        public void CreateTaskBarIcon()
        {
            TaskBarIcon.Icon = new System.Drawing.Icon(@"program_icon.ico");
            var TBmenu = new ContextMenu();
            TBmenu.Items.Clear();

            var OpenItem = new MenuItem();
            OpenItem.Header = "Show";
            OpenItem.Click += (obj, args) =>
            {
                this.Show();
                this.WindowState = WindowState.Maximized;
            };
            TBmenu.Items.Add(OpenItem);

            var CloseItem = new MenuItem();
            CloseItem.Header = "Close";
            CloseItem.Click += (obj, args) =>
            {
                this.Close();
            };
            TBmenu.Items.Add(CloseItem);

            TaskBarIcon.ContextMenu = TBmenu;

            TaskBarIcon.TrayLeftMouseUp += (obj, args) =>
            {
                this.Show();
                this.WindowState = WindowState.Maximized;
            };
        }
        //set pre defined lists
        private async Task SetLists()
        {
            var laboratoryBookName = this.LaboratoryBookName;

            var RegimesTask = Task.Run(() =>
            {
                return MainWindowHelper.SetColumnList(LaboratoryBookName, ColumnToGet.Regime);
            });
            var OperatorsTask = Task.Run(() =>
            {
                return MainWindowHelper.SetColumnList(LaboratoryBookName, ColumnToGet.Operator);
            });
            var MaterialsTask = Task.Run(() =>
            {
                return MainWindowHelper.SetColumnList(LaboratoryBookName, ColumnToGet.Material);
            });
            var SubstratesTask = Task.Run(() =>
            {
                return MainWindowHelper.SetColumnList(LaboratoryBookName, ColumnToGet.Substrate);
            });
            var PermissionIDsTask = Task.Run(() =>
            {
                return MainWindowHelper.SetColumnList(LaboratoryBookName, ColumnToGet.PermissionID);
            });

            var Tasks = new List<Task<ObservableCollection<object>>> {
                    RegimesTask,
                    OperatorsTask,
                    MaterialsTask,
                    SubstratesTask,
                    PermissionIDsTask
                };
            while (Tasks.Any())
            {
                var compleatedTask = await Task.WhenAny(Tasks.ToArray());

                if (compleatedTask == RegimesTask)
                {
                    this.Regimes = compleatedTask.Result;
                    Tasks.Remove(compleatedTask);
                }
                else if (compleatedTask == OperatorsTask)
                {
                    this.Operators = compleatedTask.Result;
                    Tasks.Remove(compleatedTask);
                }
                else if (compleatedTask == MaterialsTask)
                {
                    this.Materials = compleatedTask.Result;
                    Tasks.Remove(compleatedTask);
                }
                else if (compleatedTask == SubstratesTask)
                {
                    this.Substrates = compleatedTask.Result;
                    Tasks.Remove(compleatedTask);
                }
                else
                {
                    this.PermssionIDs = compleatedTask.Result;
                    Tasks.Remove(compleatedTask);
                }

            }
        }

        //initialize data, attach event hendlers to make datatable connected with database
        private async void MainWindowLoaded(object sender, RoutedEventArgs e)
        {   
            if (User is Guest)
            {
                BtnModifyDatabase.Visibility = Visibility.Collapsed;
                BtnModifyUsers.Visibility = Visibility.Collapsed;
            }
            try
            {
                CreateTaskBarIcon();

                SetUpAndStartTimer();

                await SetLists();              

                this.DgLaboratoryBook.AutoGeneratingColumn += DgLaboratoryBookAutoGeneratingColumn;

               
                this.LaboratoryDataTable = await Task.Run(() =>
                {
                    return User.GetDataFromLaboratoryBook(LaboratoryBookName, LaboratoryBookPermission);
                });
                this.LaboratoryDataTable.RowChanged  += LaboratoryDataTableRowChanged;
                this.LaboratoryDataTable.RowDeleting += LaboratoryDataTableRowDeleting;

                this.DgLaboratoryBook.PreviewKeyDown += DgLaboratoryBookPreviewKeyDown;

                this.DgLaboratoryBook.CellEditEnding += DgLaboratoryBook_CellEditEnding;

                TaskBarIcon.ShowBalloonTip
                    (
                        "Welcome",
                        $"Succesfully connected to {LaboratoryBookName}",
                        BalloonIcon.Info
                    );

            }
            catch (Exception exception)
            {
                MessageBox.Show
                        (
                            exception.Message,
                            "Initialisation error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
            }           
            
        }
        //change autogenerated columns if neccessary
        private void DgLaboratoryBookAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            
            if (e.PropertyType == typeof(DateTime))
            {
                var column  = e.Column as DataGridTextColumn;
                var binding = column.Binding as Binding;
                
                binding.StringFormat = "yyyy-MM-dd";
            }
            
            switch (e.PropertyName)
            {
                case "regime":
                case "operator":
                case "material":
                case "substrate":
                case "permissionID":
                    var itemsSource = new ObservableCollection<object>();

                    switch(e.PropertyName)
                    {
                        case "regime":
                            itemsSource = Regimes;
                            break;
                        case "operator":
                            itemsSource = Operators;
                            break;
                        case "material":
                            itemsSource = Materials;
                            break;
                        case "substrate":
                            itemsSource = Substrates;
                            break;
                        case "permissionID":
                            itemsSource = PermssionIDs;
                            break;
                    }

                    var column  = e.Column as DataGridTextColumn;
                    var binding = column.Binding as Binding;

                    var cbxcolumn = new DataGridComboBoxColumn
                    {
                        Header = e.Column.Header,
                        SelectedItemBinding = binding,
                        ItemsSource = itemsSource                        
                    };                    

                    e.Column = cbxcolumn;
                    break;
                
            }            

        }
       

        private async void LaboratoryDataTableRowDeleting(object sender, DataRowChangeEventArgs e)
        {
            if (this.DeleteHandled)
            {
                this.DeleteHandled = false;
                return;
            }
            
            var result = await MainWindowHelper.DeleteRowAsync
                                                (
                                                    LaboratoryBookName,
                                                    LaboratoryBookPermission,
                                                    e.Row
                                                );

            if (result)
            {
                e.Row.AcceptChanges();
                TaskBarIcon.ShowBalloonTip
                    (
                        "Row action",
                        $"Row was successfully deleted from {LaboratoryBookName} database",
                        BalloonIcon.Info
                    );
                await MainWindowHelper.UpdateStatistics(LaboratoryBookName, User);
            }
            else
            {
                e.Row.RejectChanges();
            }


        }

        private void DgLaboratoryBookPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete) return;

            var dialogResult = MessageBox.Show
                                          (
                                            "Do you want to delete selected row?",
                                            "Row delete",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question
                                          );
            if (dialogResult == MessageBoxResult.No)
            {
                e.Handled = true;
                this.DeleteHandled = true;
            }
        }

        private async void LaboratoryDataTableRowChanged(object sender, DataRowChangeEventArgs e)
        {
            bool result = false;

            var laboratoryBookName =       this.LaboratoryBookName;
            var laboratoryBookPermission = this.LaboratoryBookPermission;
            var row = e.Row;
            var dgLaboratoryBook = this.DgLaboratoryBook;

            var columnList = new List<string>();
            foreach (var column in dgLaboratoryBook.Columns)
            {
                if ((string)(column.Header) == "sampleID") continue;

                var str = column.Header.ToString();
                columnList.Add(str);
            }

            if (e.Action == DataRowAction.Add)
            {

                result = await Task.Run(() =>
                {
                    return MainWindowHelper.AddRow(laboratoryBookName, laboratoryBookPermission, row, dgLaboratoryBook,User);
                });

                TaskBarIcon.ShowBalloonTip
                (
                    "Row action",
                    $"Row was successfully added to {LaboratoryBookName} database",
                    BalloonIcon.Info
                );
            }
            if (e.Action == DataRowAction.Change)
            {
                result = await Task.Run(() =>
                {
                    return MainWindowHelper.ChangeRow(laboratoryBookName, laboratoryBookPermission, row, columnList);
                });
            }
           

            if (result)
            {
                await MainWindowHelper.UpdateStatistics(LaboratoryBookName, User);
                e.Row.AcceptChanges();
                
            }
            else
            {               
                e.Row.RejectChanges();                
            }            
        }        
    }
}
