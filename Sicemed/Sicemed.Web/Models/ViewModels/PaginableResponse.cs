using System.Collections;

namespace Sicemed.Web.Models.ViewModels
{
    public class PaginableResponse
    {
        public long Total { get; set; }
        
        public long Page { get; set; }
        
        public long Records { get; set; }
        
        public IEnumerable Rows { get; set; }

        public PaginableResponse()
        {
            Rows = new ArrayList();
        }
    }
}