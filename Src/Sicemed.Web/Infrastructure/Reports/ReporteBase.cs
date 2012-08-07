using System.Collections.Generic;
using AutoMapper;
using Castle.Core.Logging;
using Sicemed.Web.Infrastructure.Providers.Cache;
using Sicemed.Web.Infrastructure.Queries;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Reports
{
    public abstract class ReporteBase<T> : IReport<T>
    {
        private ILogger _logger;

        public ILogger Logger
        {
            get { return _logger ?? (_logger = NullLogger.Instance); }
            set { _logger = value; }
        }

        public IMembershipService MembershipService { get; set; }
        public IQueryFactory QueryFactory { get; set; }
        public IMappingEngine MappingEngine { get; set; }
        public ICacheProvider Cache { get; set; }

        public abstract IEnumerable<T> Execute();

        protected virtual Persona User
        {
            get { return MembershipService.GetCurrentUser(); }
        }
    }
}