using System;
using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
    public class SqlDateTimeRangeAttribute : RangeAttribute
    {
         public SqlDateTimeRangeAttribute()
            : base(typeof(DateTime), "2012-01-01", "3000-01-01") { }
    }
}