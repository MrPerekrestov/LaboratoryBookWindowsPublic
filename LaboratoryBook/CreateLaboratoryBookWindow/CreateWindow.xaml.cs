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

namespace LaboratoryBook.CreateLaboratoryBookWindow
{
    public partial class CreateWindow : Window
    {
        public User User { get;  }

        public CreateWindow(ref User user)
        {
            this.User = user; 
            InitializeComponent();
        }
    }
}
