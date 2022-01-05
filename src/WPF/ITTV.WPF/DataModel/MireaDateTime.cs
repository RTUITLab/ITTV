﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ITTV.WPF.DataModel.Models;

namespace ITTV.WPF.DataModel
{
    public class MireaDateTime : Singleton<MireaDateTime>
    {
        private int sleepTime;
        private bool needToCheckTime;

        public MireaDateTime()
        {
            needToCheckTime = Settings.Instance.NeedCheckTime;
            sleepTime = Settings.Instance.SleepHour;
            Settings.Instance.SettingsUpdated += () => { 
                sleepTime = Settings.Instance.SleepHour;
                needToCheckTime = Settings.Instance.NeedCheckTime;
            };
        }

        public static string GetTime(DateTime dateTime)
        {
            return dateTime.ToLongTimeString();
        }
        
        //TODO: Extract to helper service
        public static readonly List<KeyValuePair<string, (TimeSpan StartTime, TimeSpan EndTime)>> ScheduleTimeForDay = new List<KeyValuePair<string, (TimeSpan,TimeSpan)>>
        {
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("Идёт 1-я пара", (new TimeSpan(9, 0, 0), new TimeSpan(10, 30, 0))),
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("Перерыв перед 2-ой парой", (new TimeSpan(10, 30, 0), new TimeSpan(10, 40, 0))),
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("2-я пара", (new TimeSpan(10, 40, 0), new TimeSpan(12, 10, 0))),
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("Большой перерыв перед 3-ой парой", (new TimeSpan(12, 10, 0), new TimeSpan(12, 40, 0))),
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("3-я пара", (new TimeSpan(12, 40, 0), new TimeSpan(14, 10, 0))),
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("Перерыв перед 4-ой парой", (new TimeSpan(14, 10, 0), new TimeSpan(14, 20, 0))),
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("4-я пара", (new TimeSpan(14, 20, 0), new TimeSpan(15, 50, 0))),
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("Большой перерыв перед 5-ой парой", (new TimeSpan(15, 50, 0), new TimeSpan(16, 20, 0))),
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("5-я пара", (new TimeSpan(16, 20, 0), new TimeSpan(17, 50, 0))),
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("Перерыв перед 6-ой парой", (new TimeSpan(17, 50, 0), new TimeSpan(18, 0, 0))),
            new KeyValuePair<string, (TimeSpan, TimeSpan)> ("6-я пара", (new TimeSpan(18, 0, 0), new TimeSpan(19, 30, 0)))
        };

        public static string GetPara(DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                return string.Empty;
            
            var classesStage = ScheduleTimeForDay.FirstOrDefault(x => dateTime.TimeOfDay >= x.Value.StartTime
                                                                      && dateTime.TimeOfDay < x.Value.EndTime).Key;

            return classesStage ?? string.Empty;
        }

        public static string GetDay(DateTime dateTime)
        {
            return dateTime.ToLongDateString();
        }

        public static string GetWeek(DateTime dateTime)
        {
            var numberOfWeek = CalculateNumberOfWeek(dateTime);
           
            return numberOfWeek == default ? string.Empty : $"Идёт {numberOfWeek}-я неделя";
        }
        //TODO: покрыть тестами
        public static int CalculateNumberOfWeek(DateTime dateTime)
        {
            var startFirstSemesterDate = new DateTime(dateTime.Year, 9, 1).DayOfWeek == DayOfWeek.Sunday ? new DateTime(dateTime.Year, 9, 2) : new DateTime(dateTime.Year, 9, 1);
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

        public bool WorkTime()
        {
            //TODO: Add startTime value
            return !needToCheckTime || DateTime.Now.Hour < sleepTime && DateTime.Now.Hour >= 8;
        }
    }
}
