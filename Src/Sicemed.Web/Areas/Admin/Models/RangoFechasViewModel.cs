using System;
using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Areas.Admin.Models
{
    public class RangoFechasViewModel
    {
        [DataType(DataType.Date)]
        public DateTime Dasde { get; set; }
        [DataType(DataType.Date)]
        public DateTime Hasta { get; set; }     
    }
}