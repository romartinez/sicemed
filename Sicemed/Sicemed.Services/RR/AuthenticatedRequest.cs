using System.ComponentModel.DataAnnotations;
using Sicemed.Model;

namespace Sicemed.Services.RR {
    public class AuthenticatedRequest : BaseRequest {
        [Required]
        public PrincipalBase Principal { get; set; }
    }
}
