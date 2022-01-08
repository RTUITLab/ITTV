using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ITTV.WPF.Core.Helpers
{
    public static class MireaTimeHelper
    {
        private static readonly List<KeyValuePair<string, (TimeSpan StartTime, TimeSpan EndTime)>> ScheduleOfClassesForDay = 
            new List<KeyValuePair<string, (TimeSpan,TimeSpan)>>
            {
                new("Идёт 1-я пара", (new TimeSpan(9, 0, 0), new TimeSpan(10, 30, 0))),
                new("Перерыв перед 2-ой парой", (new TimeSpan(10, 30, 0), new TimeSpan(10, 40, 0))),
                new("2-я пара", (new TimeSpan(10, 40, 0), new TimeSpan(12, 10, 0))),
                new("Большой перерыв перед 3-ой парой", (new TimeSpan(12, 10, 0), new TimeSpan(12, 40, 0))),
                new("3-я пара", (new TimeSpan(12, 40, 0), new TimeSpan(14, 10, 0))),
                new("Перерыв перед 4-ой парой", (new TimeSpan(14, 10, 0), new TimeSpan(14, 20, 0))),
                new("4-я пара", (new TimeSpan(14, 20, 0), new TimeSpan(15, 50, 0))),
                new("Большой перерыв перед 5-ой парой", (new TimeSpan(15, 50, 0), new TimeSpan(16, 20, 0))),
                new("5-я пара", (new TimeSpan(16, 20, 0), new TimeSpan(17, 50, 0))),
                new("Перерыв перед 6-ой парой", (new TimeSpan(17, 50, 0), new TimeSpan(18, 0, 0))),
                new("6-я пара", (new TimeSpan(18, 0, 0), new TimeSpan(19, 30, 0)))
            };
        
        public static string GetLongTime(DateTime dateTime)
        {
            return dateTime.ToLongTimeString();
        }

        public static string GetStageOfClasses(DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                return string.Empty;
            
            var classesStage = ScheduleOfClassesForDay.FirstOrDefault(x => dateTime.TimeOfDay >= x.Value.StartTime
                                                                      && dateTime.TimeOfDay < x.Value.EndTime).Key;

            return classesStage ?? string.Empty;
        }

        public static string GetLongDate(DateTime dateTime)
        {
            return dateTime.ToLongDateString();
        }

        public static string GetWeekOfSemester(DateTime dateTime)
        {
            var numberOfWeek = CalculateNumberOfWeek(dateTime);
           
            return numberOfWeek == default ? string.Empty : $"Идёт {numberOfWeek}-я неделя";
        }
        //TODO: покрыть тестами
        private static int CalculateNumberOfWeek(DateTime dateTime)
        {
            var startFirstSemesterDate = new DateTime(dateTime.Year, 9, 1).DayOfWeek == DayOfWeek.Sunday ? 
                new DateTime(dateTime.Year, 9, 2) : new DateTime(dateTime.Year, 9, 1);
            var endFirstSemesterDate = new DateTime(dateTime.Year, 12, 29);
            
            var startSecondSemesterDate = new DateTime(dateTime.Year, 2, 9);
            var endSecondSemesterDate = new DateTime(dateTime.Year, 6, 30);

            if (dateTime >= startFirstSemesterDate && dateTime <= endFirstSemesterDate)
                return CalculateWeekForSemester(startFirstSemesterDate, dateTime);
            
            if (dateTime >= startSecondSemesterDate && dateTime <= endSecondSemesterDate)
                return CalculateWeekForSemester(startSecondSemesterDate, dateTime);

            return default;
        }

        private static int CalculateWeekForSemester(DateTime startSemesterDate, DateTime dateTime)
        {
            var cultureInfo = new CultureInfo("ru-RU");

            var week = cultureInfo.Calendar.GetWeekOfYear(dateTime, cultureInfo.DateTimeFormat.CalendarWeekRule,
                    cultureInfo.DateTimeFormat.FirstDayOfWeek)
                - cultureInfo.Calendar.GetWeekOfYear(startSemesterDate, cultureInfo.DateTimeFormat.CalendarWeekRule,
                    cultureInfo.DateTimeFormat.FirstDayOfWeek)
                + 1;
            return week;
        }
    }
}