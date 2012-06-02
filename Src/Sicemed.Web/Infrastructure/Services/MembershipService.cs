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
        MembershipStatus Login(string email, string password, out Persona persona, bool rememberMe = false);
        void SignOut();
        MembershipStatus RecoverPassword(string email);
        MembershipStatus ChangePassword(string email, string givenToken, string newPassword);
        MembershipStatus UnlockUser(string email);
        MembershipStatus LockUser(string email, string reason);
        MembershipStatus CreateUser(Persona user, string email, string password);
        Persona GetCurrentUser();
    }

    public class MembershipService : IMembershipService
    {
    	public const int MIN_REQUIRED_PASSWORD_LENGTH = 6;
        private const string SHA1_SALT = "!-3453dfg4";
        private const int WINDOW_MINUTES = 30;
        private const int MAX_FAILED_ATTEMPS = 3;
        private readonly IFormAuthenticationStoreService _formsAuthenticationStoreService;
        private readonly IMailSenderService _mailSenderService;
        private readonly ISessionFactory _sessionFactory;
        private ILogger _logger = NullLogger.Instance;

        public MembershipService(ISessionFactory sessionFactory,
                                 IMailSenderService mailSenderService,
                                 IFormAuthenticationStoreService formsAuthenticationStoreService)
        {
            _sessionFactory = sessionFactory;
            _mailSenderService = mailSenderService;
            _formsAuthenticationStoreService = formsAuthenticationStoreService;
        }

        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        #region IMembershipService Members

        public MembershipStatus Login(string email, string password, out Persona persona, bool rememberMe = false)
        {
            if (Logger.IsDebugEnabled) Logger.DebugFormat("La persona '{0}' se esta logueando.", email);

            var passwordHash = DataHashing.Compute(Algorithm.SHA1, password + SHA1_SALT);
            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                persona = session.QueryOver<Persona>().Where(u => u.Membership.Email == email).SingleOrDefault();

                var status = ValidateUser(email, persona);
                if (status != MembershipStatus.USER_FOUND) return status;

                if (persona.Membership.Password == passwordHash)
                {
                    if (Logger.IsDebugEnabled)
                        Logger.DebugFormat(
                            "La persona '{0}' was found and his password correct. Updating his LastLoginDate.", email);

                    persona.Membership.LastLoginDate = DateTime.Now;
                    session.Update(persona);

                    _formsAuthenticationStoreService.CreateLogInCookie(persona, rememberMe);

                    tx.Commit();
                    return status;
                }

                status = MembershipStatus.BAD_PASSWORD;

                if (persona.Membership.FailedPasswordAttemptWindowStart.HasValue
                    &&
                    persona.Membership.FailedPasswordAttemptWindowStart.Value.AddMinutes(WINDOW_MINUTES) >= DateTime.Now)
                {
                    persona.Membership.FailedPasswordAttemptCount++;
                    if (Logger.IsDebugEnabled)
                        Logger.DebugFormat(
                            "The persona '{0}' was found and his password incorrect. Incorrect attempt #{1}.", email,
                            persona.Membership.FailedPasswordAttemptCount);

                    if (persona.Membership.FailedPasswordAttemptCount >= MAX_FAILED_ATTEMPS)
                    {
                        persona.Membership.LastLockoutDate = DateTime.UtcNow;
                        persona.Membership.IsLockedOut = true;
                        persona.Membership.LockedOutReason = "Maximum failed password attemp reached.";

                        status = MembershipStatus.USER_LOCKED;

                        if (Logger.IsInfoEnabled)
                            Logger.InfoFormat(
                                "The persona '{0}' was locked. Maximum failed password attemp reached: #{1}", email,
                                persona.Membership.FailedPasswordAttemptCount);
                    }
                } else
                {
                    persona.Membership.FailedPasswordAttemptCount = 1;
                    persona.Membership.FailedPasswordAttemptWindowStart = DateTime.UtcNow;

                    if (Logger.IsDebugEnabled)
                        Logger.DebugFormat(
                            "The persona '{0}' was found and his password incorrect. First logon attempt.", email);
                }
                session.Update(persona);
                tx.Commit();

                return status;
            }
        }

        public virtual void SignOut()
        {
            if (Logger.IsDebugEnabled) Logger.DebugFormat("Singing Out.");
            _formsAuthenticationStoreService.SingOut();
        }

        public MembershipStatus RecoverPassword(string email)
        {
            if (Logger.IsDebugEnabled) Logger.DebugFormat("Recovering the password for the persona '{0}'.", email);

            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                var persona = session.QueryOver<Persona>().Where(u => u.Membership.Email == email).SingleOrDefault();

                var status = ValidateUser(email, persona);
                if (status != MembershipStatus.USER_FOUND) return status;

                var token = DataGenerator.RandomString(10, 20, true, true, true, false);
                persona.Membership.PasswordResetToken = token;
                persona.Membership.PasswordResetTokenGeneratedOn = DateTime.UtcNow;

                session.Update(persona);
                tx.Commit();

                _mailSenderService.SendPasswordResetEmail(persona, token);
                return status;
            }
        }

        public MembershipStatus ChangePassword(string email, string givenToken, string newPassword)
        {
            if (Logger.IsDebugEnabled) Logger.DebugFormat("The persona '{0}' is changing his password.", email);

            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                var usuario =
                    session.QueryOver<Persona>().Where(
                        u => u.Membership.Email == email && u.Membership.PasswordResetToken == givenToken).
                        SingleOrDefault();

                var status = ValidateUser(email, usuario);
                if (status != MembershipStatus.USER_FOUND) return status;

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
            if (Logger.IsDebugEnabled) Logger.DebugFormat("Unlocking the persona '{0}'.", email);

            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                var usuario = session.QueryOver<Persona>().Where(u => u.Membership.Email == email).SingleOrDefault();

                if (usuario == default(Persona))
                    return MembershipStatus.USER_NOT_FOUND;

                usuario.Membership.IsLockedOut = false;

                session.Update(usuario);
                tx.Commit();

                return MembershipStatus.USER_FOUND;
            }
        }

        public MembershipStatus LockUser(string email, string reason)
        {
            if (Logger.IsDebugEnabled) Logger.DebugFormat("Locking the persona '{0}'.", email);

            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                var usuario = session.QueryOver<Persona>().Where(u => u.Membership.Email == email).SingleOrDefault();

                if (usuario == default(Persona))
                    return MembershipStatus.USER_NOT_FOUND;

                usuario.Membership.IsLockedOut = true;
                usuario.Membership.LockedOutReason = reason;
                usuario.Membership.LastLockoutDate = DateTime.UtcNow;

                session.Update(usuario);
                tx.Commit();

                return MembershipStatus.USER_FOUND;
            }
        }

        public MembershipStatus CreateUser(Persona user, string email, string password)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password");

            if (Logger.IsDebugEnabled) Logger.DebugFormat("Creating the persona '{0}'.", email);

            if (password.Length < MinPasswordLength)
                throw new ArgumentException(
                    "Password is shorter than the MinPasswordLength value (" + MinPasswordLength + ").", "password");

            var session = _sessionFactory.GetCurrentSession();
            using (var tx = session.BeginTransaction())
            {
                var exists = session.QueryOver<Persona>().Where(x => x.Membership.Email == email).RowCount() > 0;

                if (exists) return MembershipStatus.DUPLICATED_USER;

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

        public virtual Persona GetCurrentUser()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return null;
            return (Persona) HttpContext.Current.User;
        }

        public int MinPasswordLength
        {
            get { return 4; }
        }

        #endregion

        private MembershipStatus ValidateUser(string email, Persona persona)
        {
            if (persona == default(Persona))
            {
                if (Logger.IsInfoEnabled) Logger.InfoFormat("There isn't a persona with email '{0}'", email);
                return MembershipStatus.USER_NOT_FOUND;
            }

            if (persona.Membership.IsLockedOut)
            {
                if (Logger.IsWarnEnabled) Logger.WarnFormat("The persona '{0}' is locked out.", email);
                return MembershipStatus.USER_LOCKED;
            }

            return MembershipStatus.USER_FOUND;
        }
    }
}