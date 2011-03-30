using System;
using System.Web;
using System.Web.Security;
using NHibernate;
using Sicemed.Web.Models;
using Sicemed.Web.Plumbing;
using Sicemed.Web.Plumbing.Helpers;
using Sicemed.Web.Plumbing.Queries.Parametros;

namespace Sicemed.Web.Services.ApplicationServices.Cuenta
{
    public class MembershipApplicationService : IMembershipApplicationService
    {
        private readonly IConsultaParametro _consultaParametro;
        private readonly ISessionFactory _sessionFactory;

        public MembershipApplicationService(IConsultaParametro consultaParametro, ISessionFactory sessionFactory)
        {
            _consultaParametro = consultaParametro;
            _sessionFactory = sessionFactory;
        }

        #region IMembershipApplicationService Members
        private int? _largoMinimoPassword;

        public int LargoMinimoPassword
        {
            get
            {
                if (!_largoMinimoPassword.HasValue)
                {
                    _largoMinimoPassword = _consultaParametro.Execute<int>(Parametro.Keys.MIN_PASSWORD_LENGTH);
                }
                return _largoMinimoPassword.Value;
            }
        }

        public Usuario ValidarUsuario(string nombreUsuario, string password)
        {
            Check.Require(!String.IsNullOrEmpty(nombreUsuario));
            Check.Require(!String.IsNullOrEmpty(password));

            using (var session = _sessionFactory.OpenStatelessSession())
            {
                var usuario =
                    session.QueryOver<Usuario>().Where(u => u.NombreUsuario == nombreUsuario && u.Password == password).
                        SingleOrDefault();
                return usuario;
            }
        }

        public MembershipCreateStatus CrearUsuario(string nombreUsuario, string password, string email, out Usuario usuarioCreado)
        {
            usuarioCreado = default(Usuario);

            if (String.IsNullOrEmpty(nombreUsuario) || nombreUsuario.Length < 4)
                return MembershipCreateStatus.InvalidUserName;

            if (EsPasswordValido(password))
                return MembershipCreateStatus.InvalidPassword;

            if (String.IsNullOrEmpty(email) || !ValidationHelper.IsValidEmail(email))
                return MembershipCreateStatus.InvalidEmail;

            try
            {
                var session = _sessionFactory.GetCurrentSession();
                using (var tx = session.BeginTransaction())
                {
                    var nombreUsuarioExistente =
                        session.QueryOver<Usuario>().Where(u => u.NombreUsuario == nombreUsuario).FutureValue<Usuario>();

                    var emailExistente =
                        session.QueryOver<Usuario>().Where(u => u.Email == email).FutureValue<Usuario>();

                    if (nombreUsuarioExistente.Value != default(Usuario) && nombreUsuarioExistente.Value.NombreUsuario == nombreUsuario)
                        return MembershipCreateStatus.DuplicateUserName;

                    if (emailExistente.Value != default(Usuario) && emailExistente.Value.Email == email)
                        return MembershipCreateStatus.DuplicateEmail;


                    usuarioCreado = new Usuario()
                    {
                        NombreUsuario = nombreUsuario,
                        Password = password,
                        Email = email
                    };

                    session.Save(usuarioCreado);
                    tx.Commit();
                    return MembershipCreateStatus.Success;
                }
            }
            catch (Exception ex)
            {
                return MembershipCreateStatus.ProviderError;
            }
        }

        private bool EsPasswordValido(string password)
        {
            return String.IsNullOrEmpty(password) || password.Length < LargoMinimoPassword;
        }

        public bool CambiarPassword(string nombreUsuario, string passwordViejo, string passwordNuevo)
        {
            try
            {
                Check.Require(!String.IsNullOrEmpty(nombreUsuario));
                Check.Require(!String.IsNullOrEmpty(passwordViejo));
                Check.Require(!String.IsNullOrEmpty(passwordNuevo));

                if (!EsPasswordValido(passwordNuevo)) return false;

                var session = _sessionFactory.GetCurrentSession();
                using (var tx = session.BeginTransaction())
                {
                    var user =
                        session.QueryOver<Usuario>().Where(
                            u => u.NombreUsuario == nombreUsuario && u.Password == passwordViejo).SingleOrDefault();
                    
                    if(user == default(Usuario)) return false;

                    user.Password = passwordNuevo;

                    session.Update(user);
                    tx.Commit();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Usuario GetCurrentUser()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return null;
            return (Usuario)HttpContext.Current.User;
        }

        #endregion
    }
}