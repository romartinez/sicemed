using System.Linq;
using System.Web.Mvc;

namespace Sicemed.Web.Infrastructure.Exceptions
{
    public class ValidationErrorException : ToClientException
    {
        public ValidationErrorException() { }

        public ValidationErrorException(params string[] error)
            : base("ValidationErrorException")
        {
            this.Data.Add("errors", new[] { error });
        }

        public ValidationErrorException(ModelStateDictionary modelState)
            : base("ValidationErrorException")
        {
            var erroresValidacion = modelState.Values.Where(x => x.Errors.Any()).SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            this.Data.Add("errors", erroresValidacion);
        }
    }

}