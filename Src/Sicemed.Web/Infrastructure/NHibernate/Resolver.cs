using System;
using System.Collections.Generic;
using System.Collections;
using NHibernate;
using NHibernate.Metadata;
using NHibernate.Type;
using Sicemed.Web.Models;
using log4net;

namespace Sicemed.Web.Infrastructure.NHibernate
{
    public static class Resolver
    {
        private static ILog _log = LogManager.GetLogger(typeof(Resolver));

        public static object[] ResolveArray(object[] entityArray, ISession session)
        {
            // loop over elements
            if (entityArray == null) return null;

            for (var i = 0; i < entityArray.Length; i++)
                entityArray[i] = Resolve(entityArray[i], session);

            // done
            return entityArray;
        }

        private static object Resolve(object entity, ISession session)
        {
            // CHECKS //
            // if the entity is null, just skip it
            if (entity == null)
                return null;
            // Do not loop over included intities
            if (entity is Entity) return entity.ToString();

            // RESOLVE ENTITY //

            object resolvedEntity;
            // now lets go ahead and make sure everything is unproxied
            try
            {
                resolvedEntity = session.GetSessionImplementation().PersistenceContext.Unproxy(entity);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return null;
            }

            // return the resolved entity :)
            return resolvedEntity.ToString();
        }
    }
}