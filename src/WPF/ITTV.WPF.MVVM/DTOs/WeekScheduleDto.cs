using System.Collections.Generic;

namespace ITTV.WPF.MVVM.DTOs
{
    public class WeekScheduleDto
    {
        public List<ScheduleLessonDto> MondayLessons { get; set; }
        public List<ScheduleLessonDto> TuesdayLessons { get; set; }
        public List<ScheduleLessonDto> WednesdayLessons { get; set; }
        public List<ScheduleLessonDto> ThursdayLessons { get; set; }
        public List<ScheduleLessonDto> FridayLessons { get; set; }
        public List<ScheduleLessonDto> SaturdayLessons { get; set; }
    }
}