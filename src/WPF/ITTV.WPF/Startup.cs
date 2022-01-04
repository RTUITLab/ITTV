using System;
using System.Diagnostics;
using System.IO;
using ITTV.WPF.DataModel;
using ITTV.WPF.DataModel.Models;

namespace ITTV.WPF
{
    public static class Startup
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                var app = new App();
                app.InitializeComponent();
                app.Run();
            } 
            catch (Exception e)
            {
                File.AppendAllLines(AllPaths.FileLogsPath, new[] { DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString() + "\t\t" + e});
          
                if (!Settings.Instance.IsAdmin)
                {
                    var assemblyName = typeof(Startup).Assembly.GetName().Name;
                    Process.Start(assemblyName);

                    NewsUpdateThread.Instance.StopUpdating();
                }
            }
        }
    }
}
