using System;
using Agatha.Common;
using Sicemed.Model;
using Sicemed.Services.RR.Accounts;

namespace Sicemed.Services.Handlers {
    public class RegisterUserHandler : NHibernateBaseRequestHandler<RegisterUserRequest, RegisterUserResponse> {
        
        public override Response Handle(RegisterUserRequest request) {
            var newUser = new Usuario() {
                FechaNacimiento = DateTime.Now, NumeroDocumento = "21444445", TipoDocumento = "DD"
            };
            Session.Save(newUser);
            return new RegisterUserResponse();
        }
    }
}
