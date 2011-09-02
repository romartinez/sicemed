using System;

namespace Sicemed.Web.Infrastructure.Exceptions
{
    public class SicemedException : ApplicationException
    {
        public SicemedException() { }
        public SicemedException(string message) : base(message) { }
    }
}