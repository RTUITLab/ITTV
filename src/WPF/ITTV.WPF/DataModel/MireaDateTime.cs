using System;
using System.Globalization;

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

        public static string GetPara(DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Sunday) return "";
            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 9, 0, 0) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 10, 30, 0))
            {
                return "1-ая пара";
            }

            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 10, 30, 1) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 10, 39, 59))
            {
                return "Перерыв перед 2-ой парой";
            }

            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 10, 40, 0) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 10, 0))
            {
                return "2-ая пара";
            }

            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 10, 1) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 39, 59))
            {
                return "Большой перерыв перед 3-ей парой";
            }

            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 40, 0) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 10, 0))
            {
                return "3-ая пара";
            }

            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 10, 1) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 19, 59))
            {
                return "Перерыв перед 4-ой парой";
            }

            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 20, 0) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 15, 50, 0))
            {
                return "4-ая пара";
            }

            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 15, 50, 1) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 19, 59))
            {
                return "Большой перерыв перед 5-ой парой";
            }

            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 20, 0) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 50, 0))
            {
                return "5-ая пара";
            }

            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 50, 1) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 59, 59))
            {
                return "Перерыв перед 6-ой парой";
            }

            if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 18, 0, 0) &&
                dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 19, 30, 0))
            {
                return "6-ая пара";
            }

            return "";
        }

        public static string GetDay(DateTime dateTime)
        {
            return dateTime.ToLongDateString();
        }

        public static string GetWeek(DateTime dateTime)
        {
            var cultureInfo = new CultureInfo("ru-RU");

            if (dateTime > new DateTime(dateTime.Year, 2, 9) && dateTime < new DateTime(dateTime.Year, 6, 30))
            {
                return "Идет " +
                    (cultureInfo.Calendar.GetWeekOfYear(dateTime, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek) -
                    cultureInfo.Calendar.GetWeekOfYear(new DateTime(dateTime.Year, 2, 9), cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek) +
                    1) +
                    "-я неделя";
            }

            if (dateTime > (new DateTime(dateTime.Year, 9, 1).DayOfWeek == DayOfWeek.Sunday ? new DateTime(dateTime.Year, 9, 2) : new DateTime(dateTime.Year, 9, 1)) &&
                dateTime < new DateTime())
            {
                return "Идет " +
                       (cultureInfo.Calendar.GetWeekOfYear(dateTime, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek) -
                        cultureInfo.Calendar.GetWeekOfYear(new DateTime(dateTime.Year, 9, 1).DayOfWeek == DayOfWeek.Sunday ? new DateTime(dateTime.Year, 9, 2) : new DateTime(dateTime.Year, 9, 1), cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek) +
                        1) +
                       "-я неделя";
            }
            return "";
        }

        public bool WorkTime()
        {
            return !needToCheckTime || (DateTime.Now.Hour < sleepTime && DateTime.Now.Hour >= 8);
        }
    }
}
