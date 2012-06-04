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
    }
}