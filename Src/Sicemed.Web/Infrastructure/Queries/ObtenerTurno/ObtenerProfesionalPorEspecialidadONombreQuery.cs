using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Util;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.Queries.ObtenerTurno
{
    public interface IObtenerProfesionalPorEspecialidadONombreQuery : IQuery<IEnumerable<Profesional>>
    {
        long EspecialidadId { get; set; }
        string Profesional { get; set; }
    }

    public class ObtenerProfesionalPorEspecialidadONombreQuery : Query<IEnumerable<Profesional>>, IObtenerProfesionalPorEspecialidadONombreQuery
    {
        public virtual long EspecialidadId { get; set; }
        public virtual string Profesional { get; set; }

        public override IEnumerable<Profesional> CoreExecute()
        {
            var query = SessionFactory.GetCurrentSession().QueryOver<Profesional>();

            //if (EspecialidadId != default(long))
            //query = query.JoinQueryOver<Especialidad>(x => x.Especialidades).Where(x=>x.Id ==EspecialidadId);

            if (!string.IsNullOrWhiteSpace(Profesional))
                return query.Where(
                    Restrictions.InsensitiveLike("Persona.Nombre", Profesional)
                    || Restrictions.InsensitiveLike("Persona.Apellido", Profesional)
                    || Restrictions.InsensitiveLike("Persona.SegundoNombre", Profesional))
                    .List();

            return query.List();
        }
    }
}