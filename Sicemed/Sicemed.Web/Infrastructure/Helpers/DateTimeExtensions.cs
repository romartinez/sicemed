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
    }
}