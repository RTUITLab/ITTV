using Microsoft.Samples.Kinect.ControlsBasics.DataModel;
using Microsoft.Samples.Kinect.ControlsBasics.TVSettings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics
{
    class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                // ...
            }
            else
            {
                try
                {
                    var app = new App();
                    app.InitializeComponent();
                    app.Run();
                } catch (Exception e)
                {
                    try
                    {
                        File.AppendAllLines("./logs.txt", new string[] { DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString() + "\t\t" + e.ToString()});
                    }
                    catch (Exception) { }

                    if (!Settings.Instance.IsAdmin)
                    {
                        NewsUpdateThread.Instance.StopUpdating();
                        Process.Start("ControlsBasics-WPF.exe");
                    }
                }
            }
        }
    }
}
