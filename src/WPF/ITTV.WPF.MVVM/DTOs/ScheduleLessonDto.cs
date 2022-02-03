using System;

namespace ITTV.WPF.MVVM.DTOs
{
    public class ScheduleLessonDto
    {
        public ScheduleLessonDto()
        { }

        public ScheduleLessonDto(string classRoom,
            string name, 
            string teacher, 
            string type, 
            string startTime, 
            string endTime)
        {
            ClassRoom = classRoom;
            Name = name;
            Teacher = teacher;
            Type = type;
            StartTime = startTime;
            EndTime = endTime;
        }
        public string ClassRoom { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public string Type { get; set; }
        
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}