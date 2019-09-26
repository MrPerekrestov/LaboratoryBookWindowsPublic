using LaboratoryBook.UserClass;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LaboratoryBook.LoginWindow
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static readonly DependencyProperty BookUserProperty =
            DependencyProperty.Register
                              (
                                   "BookUser",
                                    typeof(User),
                                    typeof(LoginWindow)
                              );
        public User BookUser
        {
            get { return (User)this.GetValue(BookUserProperty); }
            set { this.SetValue(BookUserProperty, value); }
        }

        public Permission?  LaboratoryBookPermission { get; set; }


        private bool IsLogged { get; set; } = false; 
       
        public LoginWindow()
        {

            InitializeComponent();

            this.Loaded += LoginWindow_Loaded;
            
            
        }

        private async void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var loadNameTask = new Task<string>(() =>
            {
                return LoginHelper.ReadLoginFromFile();
            });
           
            loadNameTask.Start();
            TbxLogin.Text = await loadNameTask;          
           
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

    }
}
