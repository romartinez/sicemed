using System;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime SetTimeWith(this DateTime originalDate, DateTime time)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day,
                time.Hour, time.Minute, time.Second);
        }

        public static DateTime ToMidnigth(this DateTime originalDate)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, 0, 0, 0);
        }
        
        public static DateTime OnlyTime(this DateTime originalDate)
        {
            return new DateTime(2000, 1, 1, originalDate.Hour, originalDate.Minute, originalDate.Second);
        }

        public static string ToFullDate(this DateTime date)
        {
            return date.ToShortDateString() + " " + date.ToShortTimeString();
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }
}