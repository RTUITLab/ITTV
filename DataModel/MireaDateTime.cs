using Microsoft.Samples.Kinect.ControlsBasics.TVSettings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.ControlsBasics.DataModel
{
    class MireaDateTime : Singleton<MireaDateTime>
    {
        public string GetTime(DateTime dateTime)
        {
            return dateTime.ToLongTimeString();
        }

        public string GetPara(DateTime dateTime)
        {
            if (dateTime.DayOfWeek != DayOfWeek.Sunday)
            {
                if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 9, 0, 0) &&
                    dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 10, 30, 0))
                {
                    return "1-ая пара";
                }
                else if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 10, 30, 1) &&
                  dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 10, 39, 59))
                {
                    return "Перерыв перед 1-ой парой";
                }
                else if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 10, 40, 0) &&
                    dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 10, 0))
                {
                    return "2-ая пара";
                }
                else if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 10, 1) &&
                    dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 39, 59))
                {
                    return "Большой перерыв перед 3-ей парой";
                }
                else if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 40, 0) &&
                    dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 10, 0))
                {
                    return "3-ая пара";
                }
                else if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 10, 1) &&
                    dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 19, 59))
                {
                    return "Перерыв перед 4-ой парой";
                }
                else if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 14, 20, 0) &&
                    dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 15, 50, 0))
                {
                    return "4-ая пара";
                }
                else if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 15, 50, 1) &&
                    dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 9, 59))
                {
                    return "Большой перерыв перед 5-ой парой";
                }
                else if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 20, 0) &&
                    dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 50, 0))
                {
                    return "5-ая пара";
                }
                else if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 50, 1) &&
                    dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 17, 59, 59))
                {
                    return "Перерыв перед 6-ой парой";
                }
                else if (dateTime >= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 18, 0, 0) &&
                    dateTime <= new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 19, 30, 0))
                {
                    return "6-ая пара";
                }
            }

            return "";
        }

        public string GetDay(DateTime dateTime)
        {
            return dateTime.ToLongDateString();
        }

        public string GetWeek(DateTime dateTime)
        {
            CultureInfo cultureInfo = new CultureInfo("ru-RU");

            if (dateTime > new DateTime(dateTime.Year, 2, 9) && dateTime < new DateTime(dateTime.Year, 6, 30))
            {
                return "Идет " +
                    (cultureInfo.Calendar.GetWeekOfYear(dateTime, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek) -
                    cultureInfo.Calendar.GetWeekOfYear(new DateTime(dateTime.Year, 2, 9), cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek) +
                    1) +
                    "-я неделя";
            }
            else if (dateTime > (new DateTime(dateTime.Year, 9, 1).DayOfWeek == DayOfWeek.Sunday ? new DateTime(dateTime.Year, 9, 2) : new DateTime(dateTime.Year, 9, 1)) &&
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

        public static bool WorkTime()
        {
            if (Settings.Instance.NeedCheckTime && (DateTime.Now.Hour >= 22 || DateTime.Now.Hour < 8))
            {
                return false;
            }
            //else if (!EggVideo.IsVisible)
            //{
            //    return true;
            //}
            return true;
        }
    }
}
