using System.Web;
using NHibernate.Event;
using Newtonsoft.Json;
using Sicemed.Web.Infrastructure.Helpers;
using log4net;

namespace Sicemed.Web.Infrastructure.NHibernate.Events
{
    public class SaveOrUpdateEventListener : ISaveOrUpdateEventListener
    {
        private ILog _log = LogManager.GetLogger(typeof(SaveOrUpdateEventListener));

        public void OnSaveOrUpdate(SaveOrUpdateEvent @event)
        {
            _log.Fatal("Save or update");

            _log.FatalFormat("[{0}] - {1} - {2} - {3} - {4}->{5}",
                HttpContext.Current.User, @event.EntityName, @event.Entry != null ? @event.Entry.Status.ToString() : string.Empty, @event.Session.SessionId, Json.SerializeObject(@event.Entity), Json.SerializeObject(@event.ResultEntity));            
        }
    }
}