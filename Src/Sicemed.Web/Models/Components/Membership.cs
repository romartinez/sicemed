using System;

namespace Sicemed.Web.Models.Components
{
    public class Membership
    {
        private readonly DateTime _createDate;

        public Membership()
        {
            _createDate = DateTime.UtcNow;
        }

        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordResetToken { get; set; }
        public virtual DateTime? PasswordResetTokenGeneratedOn { get; set; }
        public virtual bool IsLockedOut { get; set; }
        public virtual string LockedOutReason { get; set; }

        public virtual DateTime CreateDate
        {
            get { return _createDate; }
        }

        public virtual DateTime? LastLoginDate { get; set; }
        public virtual DateTime? LastPasswordChangedDate { get; set; }
        public virtual DateTime? LastLockoutDate { get; set; }
        public virtual int FailedPasswordAttemptCount { get; set; }
        public virtual DateTime? FailedPasswordAttemptWindowStart { get; set; }

        public override string ToString()
        {
            return Email;
        }
    }
}