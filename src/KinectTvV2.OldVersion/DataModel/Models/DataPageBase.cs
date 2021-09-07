using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    public class DataPageBase : DataBase
    {
        private static TaskType task = TaskType.Page;
        private Type navigationPage;
        private string[] parametrs;

        public DataPageBase(string uniqueId, string title, Type navigationPage, string[] param) : base(uniqueId, title)
        {
            this.navigationPage = navigationPage;
            this.Parametrs = param;
        }


        public TaskType Task
        {
            get { return task; }
            set { this.SetProperty(ref task, value); }
        }

        public Type NavigationPage
        {
            get { return this.navigationPage; }
            set { this.SetProperty(ref this.navigationPage, value); }
        }

        public string[] Parametrs
        {
            get { return this.parametrs; }
            set { this.parametrs = value; }
        }
    }
}
