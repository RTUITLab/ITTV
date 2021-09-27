using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
using Microsoft.Samples.Kinect.ControlsBasics.TVSettings;
using System;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Samples.Kinect.ControlsBasics
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
                
                if (!Settings.Instance.IsAdmin)
                {
                    NewsUpdateThread.Instance.StopUpdating();
                    Process.Start("ControlsBasics-WPF.exe");
                }
            }
        }
    }
}
