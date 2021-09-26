﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    public class DataPageBase : DataBase
    {
        public DataPageBase(string uniqueId, string title, Type navigationPage, string[] param) : base(uniqueId, title)
        {
            NavigationPage = navigationPage;
            Parameters = param;
        }


        public static TaskType Task => TaskType.Page;

        public Type NavigationPage { get; }

        public string[] Parameters { get; }
    }
}
