using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class MembershipMap : ComponentMapping<Membership>
    {
        public MembershipMap()
        {
            Property(x => x.Password, map => map.NotNullable(true));
            Property(x => x.Email, map => map.NotNullable(true));
            Property(x => x.PasswordResetToken);
            Property(x => x.PasswordResetTokenGeneratedOn);
            Property(x => x.IsLockedOut, map => map.NotNullable(true));
            Property(x => x.LockedOutReason);
            Property(x => x.CreateDate, map =>
            {
                map.Access(Accessor.NoSetter);
                map.NotNullable(true);
            });
            Property(x => x.LastLoginDate);
            Property(x => x.LastPasswordChangedDate);
            Property(x => x.LastLockoutDate);
            Property(x => x.FailedPasswordAttemptCount, map => map.NotNullable(true));
            Property(x => x.FailedPasswordAttemptWindowStart);
        }
    }
}