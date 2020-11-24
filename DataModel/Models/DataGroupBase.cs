using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models.DataBase;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    public class DataGroupBase : DataBase
    {
        private static TaskType task = TaskType.ChangeGroup;
        private string newGroup;

        public DataGroupBase(string uniqueId, string title, string newGroup) : base(uniqueId, title)
        {
            this.NewGroup = newGroup;
        }

        public static TaskType Task { get => task; set => task = value; }
        public string NewGroup { get => newGroup; set => newGroup = value; }
    }
}
