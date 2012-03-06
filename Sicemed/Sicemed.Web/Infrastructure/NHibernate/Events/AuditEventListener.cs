using System;
using System.Web;
using NHibernate;
using NHibernate.Event;
using Newtonsoft.Json;
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
        }

        private string Serialize(object[] obj)
        {
            try
            {
                return "AA"; // JsonConvert.SerializeObject(obj, Formatting.Indented, _settings);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        private void AuditEvent(ISession session, string action, string entity, string before, string after)
        {
            var usuario = HttpContext.Current != null && HttpContext.Current.User != null 
                ? HttpContext.Current.User.Identity.Name : "*Unkonwn*";

            if (string.IsNullOrWhiteSpace(usuario)) usuario = "*Anonymous*";

            var auditRecord = new AuditLog()
            {
                Usuario = usuario,
                Accion = action,
                Entidad = entity,
                EntidadAntes = before,
                EntidadDespues = after,
                Fecha = DateTime.Now
            };

            if (Log.IsDebugEnabled)
                Log.DebugFormat(Serialize(new object[] { auditRecord }));
            
            session.Save(auditRecord);
            session.Flush();
        }

        public void OnPostDelete(PostDeleteEvent @event)
        {
            if (!(@event.Entity is Entity))
                return;

            AuditEvent(@event.Session.GetSession(EntityMode.Poco),
                "DELETE", @event.Entity.GetType().Name, Serialize(@event.DeletedState), string.Empty);
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            if (!(@event.Entity is Entity))
                return;

            AuditEvent(@event.Session.GetSession(EntityMode.Poco),
                "INSERT", @event.Entity.GetType().Name, string.Empty, Serialize(@event.State));
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            if (!(@event.Entity is Entity))
                return;

            AuditEvent(@event.Session.GetSession(EntityMode.Poco),
                "UPDATE", @event.Entity.GetType().Name, Serialize(@event.OldState), Serialize(@event.State));
        }
    }
}