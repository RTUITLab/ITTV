namespace ITTV.WPF.MVVM.DTOs
{
    public class ScheduleLessonDto
    {
        public ScheduleLessonDto()
        { }

        public ScheduleLessonDto(int numberLesson,
            string classRoom,
            string name, 
            string teacher, 
            string type, 
            string startTime, 
            string endTime)
        {
            NumberLesson = numberLesson;
            ClassRoom = classRoom;
            Name = name;
            Teacher = teacher;
            Type = type;
            StartTime = startTime;
            EndTime = endTime;
        }
        public int NumberLesson { get; set; }
        public string ClassRoom { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public string Type { get; set; }
        
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}