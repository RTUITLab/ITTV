using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models.DataBase;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    public class DataExecuteBase : DataBase
    {
        private static TaskType task = TaskType.Page;
        private string[] parametrs;

        public DataExecuteBase(string uniqueId, string title, string[] param) : base(uniqueId, title)
        {
            this.Parametrs = param;
        }


        public TaskType Task
        {
            get { return task; }
            set { this.SetProperty(ref task, value); }
        }

        public string[] Parametrs
        {
            get { return this.parametrs; }
            set { this.parametrs = value; }
        }
    }
}
