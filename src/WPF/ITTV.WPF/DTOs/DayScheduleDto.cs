namespace ITTV.WPF.DTOs
{
    public class DayScheduleDto
    {
        public DayScheduleDto()
        { }

        public DayScheduleDto(string dayName, 
            ScheduleLessonDto[] lessons)
        {
            DayName = dayName;
            Lessons = lessons;
        }
        public string DayName { get; set; }
        public ScheduleLessonDto[] Lessons { get; set; }
    }
}