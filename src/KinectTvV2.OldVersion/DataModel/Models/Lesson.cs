using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel.Models
{
    public class Lesson
    {
        public class LessonInfo
        {
            public string classRoom;
            public string name;
            public string teacher;
            public string type;
        }

        public class TimeInfo
        {
            public string end;
            public string start;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LessonInfo lesson;
        public TimeInfo time;
    }

    public class FullSchedule
    {
        public Week first;
        public Week second;

        public class Week
        {
            public List<Lesson> monday;
            public List<Lesson> tuesday;
            public List<Lesson> wednesday;
            public List<Lesson> thursday;
            public List<Lesson> friday; 
            public List<Lesson> saturday;
        }
    }
}
