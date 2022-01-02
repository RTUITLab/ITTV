using System;
using System.Diagnostics;
using System.IO;
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
                File.AppendAllLines("./logs.txt", new[] { DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString() + "\t\t" + e});
                
                if (!Settings.Settings.Instance.IsAdmin)
                {
                    NewsUpdateThread.Instance.StopUpdating();
                    Process.Start("ControlsBasics-WPF.exe");
                }
            }
        }
    }
}
