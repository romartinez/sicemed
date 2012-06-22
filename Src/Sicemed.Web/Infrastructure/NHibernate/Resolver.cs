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
            // create a resolved entities list for peace and sharing
            var resolvedEntities = new List<Object>();
            // loop over elements
            if (entityArray == null) return null;

            for (var i = 0; i < entityArray.Length; i++)
                entityArray[i] = Resolve(entityArray[i], session, resolvedEntities);

            // done
            return entityArray;
        }

        private static object Resolve(object entity, ISession session, ICollection<object> resolvedEntities)
        {
            // CHECKS //
            // Do not loop over included intities
            if (entity is Entity) return entity.ToString();

            // if the entity is null, just skip it
            if (entity == null)
                return null;
            // if we have already resolved it, return that
            if (resolvedEntities.Contains(entity))
                return entity;

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
            // add entity to the list of resolved entities
            resolvedEntities.Add(resolvedEntity);

            // GET TYPE INFO //

            IClassMetadata entityMetadata = null;
            // get the entity type
            var entityType = entity.GetType();
            // get the entity meta data from the type
            try
            {
                entityMetadata = session.SessionFactory.GetClassMetadata(entityType);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return null;
            }

            // PERFORM PROPERTY DIVE //

            // get properties for this object
            var propertyInfos = entityType.GetProperties();
            // loop over source properties & compare
            foreach (var propertyInfo in propertyInfos)
            {
                try
                {
                    // get property name
                    var propertyName = propertyInfo.Name;
                    // get property type
                    IType entityPropertyType = null;
                    try { entityPropertyType = entityMetadata.GetPropertyType(propertyName); }
                    catch (Exception) { continue; }
                    // get property value
                    var propertyValue = propertyInfo.GetValue(entity, null);
                    // these are not the good kind of bags :P
                    if (entityPropertyType.IsCollectionType)
                    {
                        // first get the property list's internal type
                        var propertyListInternalType = propertyInfo.PropertyType.GetGenericArguments()[0];
                        // create new array type based on the internal type
                        var propertyListType = typeof(List<>).MakeGenericType(propertyListInternalType);
                        // create a new property list of the internal type
                        var propertyList = (IList)Activator.CreateInstance(propertyListType);
                        // set the property list in the resolved object
                        propertyInfo.SetValue(resolvedEntity, propertyList, null);
                        // get the enumerator for this property value
                        var enumerator = ((IEnumerable)propertyValue).GetEnumerator();
                        // loop over items to also perform resolution
                        while (enumerator.MoveNext())
                            propertyList.Add(Resolve(enumerator.Current, session, resolvedEntities));
                    }
                    // destroy hibernate proxies
                    else if (entityPropertyType.IsEntityType)
                    {
                        // set the property of the resolved entity to the child beneath us
                        propertyInfo.SetValue(resolvedEntity, Resolve(propertyValue, session, resolvedEntities), null);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            }

            // return the resolved entity :)
            return resolvedEntity;
        }
    }
}