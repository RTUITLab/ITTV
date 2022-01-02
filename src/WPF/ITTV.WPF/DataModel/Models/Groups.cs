using System;
using System.Collections.Generic;

namespace ITTV.WPF.DataModel.Models
{
    public class Groups
    {
        public Groups()
        { }

        public Groups(BachelorGroups bachelor, MasterGroups master, DateTime? updated)
        {
            Bachelor = bachelor;
            Master = master;
            Updated = updated;
            
            SetUpdated();
        }
        public BachelorGroups Bachelor { get; set; }
        public MasterGroups Master { get; set; }

        public class BachelorGroups
        {
            public BachelorGroups()
            { }

            public BachelorGroups(List<Direction> first,
                List<Direction> second,
                List<Direction> third,
                List<Direction> fourth)
            {
                First = first;
                Second = second;
                Third = third;
                Fourth = fourth;
            }
            public List<Direction> First { get; set; }
            public List<Direction> Second { get; set; }
            public List<Direction> Third { get; set; }
            public List<Direction> Fourth { get; set; }
        }

        public class MasterGroups
        {
            public MasterGroups()
            { }
            public MasterGroups(List<Direction> first, List<Direction> second)
            {
                First = first;
                Second = second;
            }
            public List<Direction> First { get; set; }
            public List<Direction> Second { get; set; }
        }

        public class Direction
        {
            public Direction()
            { }
            public Direction(string name, string[] numbers)
            {
                this.Name = name;
                this.Numbers = numbers;
            }
            public string Name { get; set; }
            public string[] Numbers { get; set; }
        }

        public DateTime? Updated { get; private set; }

        public void SetUpdated()
        {
            Updated = DateTime.Now;
        }
    }
}
