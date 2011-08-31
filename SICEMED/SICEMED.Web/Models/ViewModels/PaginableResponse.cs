using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels
{
    public class PaginableResponse<T>
    {
        public long Total { get; set; }
        
        public long Page { get; set; }
        
        public long Records { get; set; }
        
        public IEnumerable<T> Rows { get; set; }

        public PaginableResponse()
        {
            Rows = new List<T>();
        }
    }
}