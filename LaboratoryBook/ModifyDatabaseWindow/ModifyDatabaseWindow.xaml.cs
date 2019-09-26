using LaboratoryBook.UserClass;
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

namespace LaboratoryBook.ModifyDatabaseWindow
{
    /// <summary>
    /// Interaction logic for ModifyDatabaseWindow.xaml
    /// </summary>
    public partial class ModifyDatabaseWindow : Window
    {
        public User User { get; set; }
        public string LaboratoryBookName { get; set; }
        public ModifyDatabaseViewModel MainViewModel {get; set;}
        

        public ModifyDatabaseWindow(ref User user, string laboratoryBookName)
        {            
            InitializeComponent();

            this.User = user;
            this.LaboratoryBookName = laboratoryBookName;
            this.Loaded += ModifyDatabaseWindow_Loaded;          
        }

        private async void ModifyDatabaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var user = User;
            var laboratoryBookName = LaboratoryBookName;
            var getMaiViewModel = new Task<ModifyDatabaseViewModel>(() =>
                {
                    return new ModifyDatabaseViewModel(user, laboratoryBookName);
                }
            );
            getMaiViewModel.Start();

            this.MainViewModel = await getMaiViewModel;
            this.DataContext = MainViewModel;
            this.CbxListValues.Focus();
        }

        private void CbxList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbxListValues.Items.Count>0)
            {
                CbxListValues.SelectedIndex = 0;
            }
        }

    }
}
