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
        private static TaskType _task = TaskType.Page;

        public DataExecuteBase(string uniqueId, string title, string[] param) : base(uniqueId, title)
        {
            Parameters = param;
        }


        public TaskType Task
        {
            get => _task;
            set => SetProperty(ref _task, value);
        }

        public string[] Parameters { get; }
    }
}
