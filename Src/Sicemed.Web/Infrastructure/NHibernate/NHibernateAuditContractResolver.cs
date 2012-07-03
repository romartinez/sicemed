using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Sicemed.Web.Infrastructure.NHibernate
{
    public class NHibernateAuditContractResolver : DefaultContractResolver
    {
        protected override JsonArrayContract CreateArrayContract(Type objectType)
        {
            return null;
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            return new List<MemberInfo>();
        }
    }
}