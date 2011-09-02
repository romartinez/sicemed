using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class MembershipMap : ComponentMapping<Membership>
    {
         public MembershipMap()
         {
             Property(x => x.Password);
             Property(x => x.Email);
             Property(x => x.PasswordResetToken);
             Property(x => x.PasswordResetTokenGeneratedOn);
             Property(x => x.IsLockedOut);
             Property(x => x.LockedOutReason);
             Property(x => x.CreateDate, map => map.Access(Accessor.Field));
             Property(x => x.LastLoginDate);
             Property(x => x.LastPasswordChangedDate);
             Property(x => x.LastLockoutDate);
             Property(x => x.FailedPasswordAttemptCount);
             Property(x => x.FailedPasswordAttemptWindowStart);
         }
    }
}