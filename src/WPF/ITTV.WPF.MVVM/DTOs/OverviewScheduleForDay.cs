namespace ITTV.WPF.MVVM.DTOs
{
    public class OverviewScheduleForDay
    {
        public OverviewScheduleForDay(string dayOfWeekName)
        {
            DayOfWeekName = dayOfWeekName;
        }

        public OverviewScheduleForDay(string dayOfWeekName, OverviewScheduleForLesson[] scheduleForLessons)
        {
            DayOfWeekName = dayOfWeekName;
            ScheduleForLessons = scheduleForLessons;
        }
        public string DayOfWeekName { get; set; }
        public OverviewScheduleForLesson[] ScheduleForLessons { get; set; }
    }

    public class OverviewScheduleForLesson
    {
        public OverviewScheduleForLesson()
        { }

        public OverviewScheduleForLesson(int numberLesson,
            ScheduleLessonDto firstWeekLesson, 
            ScheduleLessonDto secondWeekLesson)
        {
            NumberLesson = numberLesson;
            FirstWeekLesson = firstWeekLesson;
            SecondWeekLesson = secondWeekLesson;
        }
        public int NumberLesson { get; set; }
        public ScheduleLessonDto FirstWeekLesson { get; set; }
        public ScheduleLessonDto SecondWeekLesson { get; set; }
    }
}