using System;
using System.Collections.Generic;
using System.Web;
using NHibernate;
using NHibernate.Event;
using NHibernate.Event.Default;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sicemed.Web.Models;
using log4net;

namespace Sicemed.Web.Infrastructure.NHibernate.Events
{
    public class AuditEventListener : IPostDeleteEventListener, IPostInsertEventListener, IPostUpdateEventListener
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuditEventListener));
        private readonly JsonSerializerSettings _settings;

        public AuditEventListener()
        {
            _settings = new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                Error = (sender, args) =>
                                            {
                                                args.ErrorContext.Handled = true;
                                            }
                            };
            _settings.Converters.Add(new IsoDateTimeConverter());
        }

        private string Serialize(string[] propertyNames, object[] obj)
        {
            try
            {
                var values = new Dictionary<string, object>();
                if(propertyNames.Length != obj.Length) 
                    throw new Exception("The length of the properties is different from the values.");

                for (var i = 0; i < propertyNames.Length; i++)
                {
                    values.Add(propertyNames[i], obj[i]);
                }

                return JsonConvert.SerializeObject(values, Formatting.Indented, _settings);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void AuditEvent(ISession session, object entityId, string action, string entity, string before, string after)
        {
            var usuario = HttpContext.Current != null && HttpContext.Current.User != null
                ? HttpContext.Current.User.Identity.Name : "*Unkonwn*";

            if (string.IsNullOrWhiteSpace(usuario)) usuario = "*Anonymous*";

            var auditRecord = new AuditLog()
            {
                Usuario = usuario,
                Accion = action,
                Entidad = entity,
                EntidadId = Convert.ToInt64(entityId),
                EntidadAntes = before,
                EntidadDespues = after,
                Fecha = DateTime.Now
            };

            session.Save(auditRecord);
            session.Flush();
        }

        public void OnPostDelete(PostDeleteEvent @event)
        {
            if (!(@event.Entity is Entity))
                return;

            AuditEvent(@event.Session.GetSession(EntityMode.Poco), @event.Id,
                "DELETE", @event.Entity.GetType().Name, Serialize(@event.Persister.EntityMetamodel.PropertyNames, Resolver.ResolveArray(@event.DeletedState, @event.Session)), string.Empty);
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            if (!(@event.Entity is Entity))
                return;

            AuditEvent(@event.Session.GetSession(EntityMode.Poco), @event.Id,
                "INSERT", @event.Entity.GetType().Name, string.Empty, Serialize(@event.Persister.EntityMetamodel.PropertyNames, Resolver.ResolveArray(@event.State, @event.Session)));
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            if (!(@event.Entity is Entity))
                return;

            AuditEvent(@event.Session.GetSession(EntityMode.Poco), @event.Id,
                "UPDATE", @event.Entity.GetType().Name, Serialize(@event.Persister.EntityMetamodel.PropertyNames, Resolver.ResolveArray(@event.OldState, @event.Session)), Serialize(@event.Persister.EntityMetamodel.PropertyNames, Resolver.ResolveArray(@event.State, @event.Session)));
        }
    }
}