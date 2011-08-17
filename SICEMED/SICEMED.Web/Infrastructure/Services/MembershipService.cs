using System;
using System.Web;
using Castle.Core.Logging;
using EfficientlyLazy.Crypto;
using NHibernate;
using Sicemed.Web.Infrastructure.Enums;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }
        MembershipStatus Login(string email, string password, out Usuario user, bool rememberMe = false);
        void SignOut();
        MembershipStatus RecoverPassword(string email);
        MembershipStatus ChangePassword(string email, string givenToken, string newPassword);
        MembershipStatus UnlockUser(string email);
        MembershipStatus LockUser(string email, string reason);
        MembershipStatus CreateUser(Usuario user, string email, string password);
        Usuario GetCurrentUser();
    }

    public class MembershipService : IMembershipService
    {
        private const string SHA1_SALT = "!-3453dfg4";
        private const int WINDOW_MINUTES = 30;
        private const int MAX_FAILED_ATTEMPS = 3;
        private readonly ICryptoEngine _cryptoEngine;
        private readonly IFormAuthenticationStoreService _formsAuthenticationStoreService;
        private readonly IMailSenderService _mailSenderService;
        private readonly ISessionFactory _sessionFactory;
        private ILogger _logger = NullLogger.Instance;
        
        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public MembershipService(ISessionFactory sessionFactory,
                                 ICryptoEngine cryptoEngine,
                                 IMailSenderService mailSenderService,
                                 IFormAuthenticationStoreService formsAuthenticationStoreService)
        {
            _sessionFactory = sessionFactory;
            _cryptoEngine = cryptoEngine;
            _mailSenderService = mailSenderService;
            _formsAuthenticationStoreService = formsAuthenticationStoreService;
        }

        #region IMembershipService Members

        public MembershipStatus Login(string email, string password, out Usuario user, bool rememberMe = false)
        {
            if(Logger.IsDebugEnabled) Logger.DebugFormat("The user '{0}' is logging in.", email);
            
            var passwordHash = DataHashing.Compute(Algorithm.SHA1, password + SHA1_SALT);
            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                user = session.QueryOver<Usuario>().Where(u => u.Membership.Email == email).SingleOrDefault();

                var status = ValidateUser(email, user);
                if(status != MembershipStatus.USER_FOUND) return status;

                if (user.Membership.Password == passwordHash)
                {
                    if(Logger.IsDebugEnabled) 
                        Logger.DebugFormat("The user '{0}' was found and his password correct. Updating his LastLoginDate.", email);

                    user.Membership.LastLoginDate = DateTime.Now;
                    session.Update(user);

                    _formsAuthenticationStoreService.CreateLogInCookie(user, rememberMe);

                    tx.Commit();
                    return status;
                }

                status = MembershipStatus.BAD_PASSWORD;

                if (user.Membership.FailedPasswordAttemptWindowStart.HasValue
                    && user.Membership.FailedPasswordAttemptWindowStart.Value.AddMinutes(WINDOW_MINUTES) >= DateTime.Now)
                {
                    user.Membership.FailedPasswordAttemptCount++;
                    if(Logger.IsDebugEnabled)
                        Logger.DebugFormat("The user '{0}' was found and his password incorrect. Incorrect attempt #{1}.", email, user.Membership.FailedPasswordAttemptCount);

                    if (user.Membership.FailedPasswordAttemptCount >= MAX_FAILED_ATTEMPS)
                    {
                        user.Membership.LastLockoutDate = DateTime.UtcNow;
                        user.Membership.IsLockedOut = true;
                        user.Membership.LockedOutReason = "Maximum failed password attemp reached.";

                        status = MembershipStatus.USER_LOCKED;

                        if(Logger.IsInfoEnabled)
                            Logger.InfoFormat("The user '{0}' was locked. Maximum failed password attemp reached: #{1}", email, user.Membership.FailedPasswordAttemptCount);
                    }
                } else
                {
                    user.Membership.FailedPasswordAttemptCount = 1;
                    user.Membership.FailedPasswordAttemptWindowStart = DateTime.UtcNow;

                    if(Logger.IsDebugEnabled)
                        Logger.DebugFormat("The user '{0}' was found and his password incorrect. First logon attempt.", email);
                }
                session.Update(user);
                tx.Commit();
                
                return status;
            }
        }

        public virtual void SignOut()
        {
            if(Logger.IsDebugEnabled) Logger.DebugFormat("Singing Out.");
            _formsAuthenticationStoreService.SingOut();
        }

        public MembershipStatus RecoverPassword(string email)
        {
            if(Logger.IsDebugEnabled) Logger.DebugFormat("Recovering the password for the user '{0}'.", email);

            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                var usuario = session.QueryOver<Usuario>().Where(u => u.Membership.Email == email).SingleOrDefault();

                var status = ValidateUser(email, usuario);
                if(status != MembershipStatus.USER_FOUND) return status;

                var token = DataGenerator.RandomString(10, 20, true, true, true, false);
                usuario.Membership.PasswordResetToken = token;
                usuario.Membership.PasswordResetTokenGeneratedOn = DateTime.UtcNow;

                session.Update(usuario);
                tx.Commit();

                _mailSenderService.SendPasswordResetEmail(usuario, token);
                return status;
            }
        }

        private MembershipStatus ValidateUser(string email, Usuario usuario) {
            if (usuario == default(Usuario))
            {
                if(Logger.IsInfoEnabled) Logger.InfoFormat("There isn't a user with email '{0}'", email);
                return MembershipStatus.USER_NOT_FOUND;
            }

            if(usuario.Membership.IsLockedOut)
            {
                if(Logger.IsWarnEnabled) Logger.WarnFormat("The user '{0}' is locked out.", email);
                return MembershipStatus.USER_LOCKED;
            }

            return MembershipStatus.USER_FOUND;
        }

        public MembershipStatus ChangePassword(string email, string givenToken, string newPassword)
        {
            if(Logger.IsDebugEnabled) Logger.DebugFormat("The user '{0}' is changing his password.", email);

            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                var usuario =
                    session.QueryOver<Usuario>().Where(
                        u => u.Membership.Email == email && u.Membership.PasswordResetToken == givenToken).
                        SingleOrDefault();

                var status = ValidateUser(email, usuario);
                if(status != MembershipStatus.USER_FOUND) return status;

                if (usuario.Membership.PasswordResetTokenGeneratedOn.HasValue
                    &&
                    usuario.Membership.PasswordResetTokenGeneratedOn.Value.AddMinutes(WINDOW_MINUTES) < DateTime.UtcNow)
                    return MembershipStatus.TOKEN_EXPIRED;

                var passwordHash = DataHashing.Compute(Algorithm.SHA1, newPassword + SHA1_SALT);

                usuario.Membership.Password = passwordHash;

                session.Update(usuario);
                tx.Commit();

                return MembershipStatus.USER_FOUND;
            }
        }

        public MembershipStatus UnlockUser(string email)
        {
            if(Logger.IsDebugEnabled) Logger.DebugFormat("Unlocking the user '{0}'.", email);

            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                var usuario = session.QueryOver<Usuario>().Where(u => u.Membership.Email == email).SingleOrDefault();

                if(usuario == default(Usuario))
                    return MembershipStatus.USER_NOT_FOUND;

                usuario.Membership.IsLockedOut = false;

                session.Update(usuario);
                tx.Commit();

                return MembershipStatus.USER_FOUND;
            }
        }

        public MembershipStatus LockUser(string email, string reason)
        {
            if(Logger.IsDebugEnabled) Logger.DebugFormat("Locking the user '{0}'.", email);

            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                var usuario = session.QueryOver<Usuario>().Where(u => u.Membership.Email == email).SingleOrDefault();

                if (usuario == default(Usuario))
                    return MembershipStatus.USER_NOT_FOUND;

                usuario.Membership.IsLockedOut = true;
                usuario.Membership.LockedOutReason = reason;
                usuario.Membership.LastLockoutDate = DateTime.UtcNow;

                session.Update(usuario);
                tx.Commit();

                return MembershipStatus.USER_FOUND;
            }
        }

        public MembershipStatus CreateUser(Usuario user, string email, string password)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password");

            if(Logger.IsDebugEnabled) Logger.DebugFormat("Creating the user '{0}'.", email);

            if (password.Length < MinPasswordLength)
                throw new ArgumentException(
                    "Password is shorter than the MinPasswordLength value (" + MinPasswordLength + ").", "password");

            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                var exists = session.QueryOver<Usuario>().Where(x => x.Membership.Email == email).RowCount() > 0;

                if(exists) return MembershipStatus.DUPLICATED_USER;

                user.Membership.FailedPasswordAttemptCount = 0;
                user.Membership.IsLockedOut = false;
                user.Membership.LockedOutReason = string.Empty;
                user.Membership.Email = email;

                var passwordHash = DataHashing.Compute(Algorithm.SHA1, password + SHA1_SALT);
                user.Membership.Password = passwordHash;

                user.Membership.PasswordResetToken = string.Empty;

                session.Save(user);
                tx.Commit();

                _mailSenderService.SendNewUserEmail(user);
            }

            return MembershipStatus.USER_CREATED;
        }

        public virtual Usuario GetCurrentUser()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return null;
            return (Usuario)HttpContext.Current.User;
        }

        public int MinPasswordLength
        {
            get { return 4; }
        }

        #endregion
    }
}