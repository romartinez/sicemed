namespace Sicemed.Web.Infrastructure.Exceptions
{
    public class ToClientException : SicemedException
    {
        public ToClientException() { }
        public ToClientException(string message) : base(message) { }
    }
}