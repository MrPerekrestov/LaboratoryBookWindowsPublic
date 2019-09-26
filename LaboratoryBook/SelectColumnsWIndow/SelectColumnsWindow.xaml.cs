using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LaboratoryBook.SelectColumnsWindow
{
    /// <summary>
    /// Interaction logic for SelectColumsWindow.xaml
    /// </summary>
    public partial class SelectColumnsWindow : Window
    {
        public List<string> Columns  { get; private set; }
        public List<string> CollapsedColumns { get; private set; }
        public bool CanApply { get; private set; } = false;
        public bool CanOk { get; private set; } = false;
        public DataGrid DataGrid { get; set; }


        public SelectColumnsWindow(ref DataGrid dataGrid)
        {
            InitializeComponent();
            SetValues(ref dataGrid);
        }

        private void SetValues(ref DataGrid dataGrid)
        {
            this.DataGrid = dataGrid;
            LvAllColumns.Items.Clear();
            this.Columns = dataGrid.Columns.Select(column => (string)column.Header)
                                      .ToList();
            LvAllColumns.ItemsSource = Columns;

            LvSelectedColumns.Items.Clear();

            var visibleColumnHeaders = dataGrid.Columns.Where(column => column.Visibility == Visibility.Visible)
                                                       .Select(column => (string)column.Header);

            foreach (var header in visibleColumnHeaders)
            {
                LvSelectedColumns.Items.Add(header);
            }
            
        }

    }
}
