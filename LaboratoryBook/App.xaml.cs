using LaboratoryBook.UserClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace LaboratoryBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App: Application 
    {
     //   public TUser LaboratoryBookUser { get; set; }
       public void Application_Start(object sender, StartupEventArgs args)
        {
            Process currentProcess = Process.GetCurrentProcess();
            var runningProcess = (from process in Process.GetProcesses()
                                  where
                                    process.Id != currentProcess.Id &&
                                    process.ProcessName.Equals(
                                      currentProcess.ProcessName,
                                      StringComparison.Ordinal)
                                  select process).FirstOrDefault();
            if (runningProcess != null) this.Shutdown();
        }
    }
}
