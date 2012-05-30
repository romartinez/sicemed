using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Proxy;
using Newtonsoft.Json.Serialization;

namespace Sicemed.Web.Infrastructure.NHibernate
{
    public class NHibernateContractResolver : DefaultContractResolver
    {
        protected override JsonArrayContract CreateArrayContract(Type objectType)
        {
            if (!typeof(INHibernateProxy).IsAssignableFrom(objectType))
            {
                return base.CreateArrayContract(objectType);
            }
            return null;
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            if (!typeof(INHibernateProxy).IsAssignableFrom(objectType))
            {
                return base.GetSerializableMembers(objectType);
            }
            return new List<MemberInfo>();
        }
    }
}