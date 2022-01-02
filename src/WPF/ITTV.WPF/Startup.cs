using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ITTV.WPF.DataModel;

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
                    Process.Start($"{assemblyName}.exe");

                    NewsUpdateThread.Instance.StopUpdating();
                }
            }
        }
    }
}
