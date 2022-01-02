using System;
using System.Collections.Generic;

namespace ITTV.WPF.DataModel.Models
{
    public class Groups
    {
        public BachelorGroups bachelor;
        public MasterGroups master;

        public class BachelorGroups
        {
            public List<Direction> first;
            public List<Direction> second;
            public List<Direction> third;
            public List<Direction> fourth;
        }

        public class MasterGroups
        {
            public List<Direction> first;
            public List<Direction> second;
        }

        public class Direction
        {
            public string name;
            public List<string> numbers;
        }

        public DateTime? Updated { get; private set; }

        public void SetUpdated()
        {
            Updated = DateTime.Now;
        }
    }
}
