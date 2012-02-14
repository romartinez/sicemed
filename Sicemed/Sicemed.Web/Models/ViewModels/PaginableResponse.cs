using System;
using System.Collections;

namespace Sicemed.Web.Models.ViewModels
{
    [Serializable]
    public class PaginableResponse
    {
        public PaginableResponse()
        {
            Rows = new ArrayList();
        }

        public long Total { get; set; }

        public long Page { get; set; }

        public long Records { get; set; }

        public IEnumerable Rows { get; set; }
    }
}