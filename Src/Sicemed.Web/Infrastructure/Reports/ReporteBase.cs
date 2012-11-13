using System;
using System.Collections.Generic;
using AutoMapper;
using Castle.Core.Logging;
using NHibernate;
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
        public ISessionFactory SessionFactory { get; set; }

        public abstract IEnumerable<T> Execute();
        public abstract string Name { get; }
        public abstract string Title { get; }
        public string DataSource { get { return Name; } }

        private Dictionary<string, string> _parameters;
        public Dictionary<string, string> Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new Dictionary<string, string>
                    {
                        {"User", User.NombreCompleto},
                        {"Date", DateTime.Now.ToString("dd/MM/yyyy HH:mm")},
                        {"Title", Title}
                    };
                }

                return _parameters;
            }
        }

        protected virtual Persona User
        {
            get { return MembershipService.GetCurrentUser(); }
        }
    }
}