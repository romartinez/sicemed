using System;
using Agatha.Common;
using NHibernate;
using Sicemed.Services.RR;

namespace Sicemed.Services.Handlers {
    public abstract class NHibernateBaseRequestHandler<TRequest, TResponse>
        : BaseRequestHandler<TRequest, TResponse>
        where TRequest : BaseRequest
        where TResponse : BaseResponse {

        public virtual ISession Session { get; set; }
        public virtual ISessionFactory SessionFactory { get; set; }

        public override Response Handle(Request request) {
            if (!Session.Transaction.IsActive) {
                if (Logger.IsDebugEnabled) Logger.DebugFormat("Initializing a new ADO transaction.");
                Session.BeginTransaction();
            }
            try {
                var response = base.Handle(request);
                if (Session.Transaction.IsActive) {
                    if (Logger.IsDebugEnabled) Logger.DebugFormat("Commiting ADO transaction.");
                    Session.Transaction.Commit();
                }
                return response;
            }catch(Exception) {
                if (Session.Transaction.IsActive) {
                    if (Logger.IsWarnEnabled) Logger.WarnFormat("Rolling back ADO transaction.");
                    Session.Transaction.Rollback();
                }
                throw;
            }
        }
    }
}
