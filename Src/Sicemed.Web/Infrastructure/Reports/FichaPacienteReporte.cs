using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Transform;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Reports;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.Reports
{
    public interface IFichaPacienteReporte : IReport<FichaPacienteReportModel>
    {
        DateTime FechaDesde { get; set; }
        DateTime FechaHasta { get; set; }
        string Filtro { get; set; }
        long? PacienteId { get; set; }
    }

    public class FichaPacienteReporte : ReporteBase<FichaPacienteReportModel>, IFichaPacienteReporte
    {
        private Paciente _paciente;
        private long? _pacienteId;

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Filtro { get; set; }
        public long? PacienteId
        {
            get { return _pacienteId; }
            set
            {
                _pacienteId = value;
                _paciente = SessionFactory.GetCurrentSession().Load<Paciente>(value);
            }
        }

        public override string Title { get { return string.Format("Resumen de Atenciones\nFechas: {0:dd/MM/yyyy} hasta {1:dd/MM/yyyy}\nFiltro: {2}", FechaDesde, FechaHasta, string.IsNullOrWhiteSpace(Filtro) ? "*Sin Filtro*" : Filtro); } }

        public override string Name { get { return "FichaPacienteReporte"; } }

        public override IEnumerable<FichaPacienteReportModel> Execute()
        {
            var session = SessionFactory.GetCurrentSession();
            var query = session.QueryOver<Turno>()
                .Fetch(t => t.Paciente).Eager
                .Fetch(t => t.Paciente.Persona).Eager
                .Fetch(t => t.Profesional).Eager
                .Fetch(t => t.Profesional.Persona).Eager
                .Fetch(t => t.Consultorio).Eager
                .Fetch(t => t.Especialidad).Eager
                .Where(t => t.FechaTurno >= FechaDesde)
                .Where(t => t.FechaTurno <= FechaHasta)
                .Where(t => t.Paciente == _paciente)
                //Solo los que estan atendidos
                .Where(t => t.Estado == EstadoTurno.Atendido)
                .OrderBy(t => t.FechaTurno).Desc;

            if (!string.IsNullOrWhiteSpace(Filtro))
            {
                Models.Roles.Profesional profesional = null;
                Persona persona = null;

                query.Left.JoinAlias(x => x.Profesional, () => profesional)
                    .Left.JoinAlias(x => profesional.Persona, () => persona)
                .Where(
                    Restrictions.On<Turno>(x => x.Nota).IsInsensitiveLike(Filtro, MatchMode.Anywhere)
                        || Restrictions.On(() => persona.Nombre).IsInsensitiveLike(Filtro, MatchMode.Start)
                        || Restrictions.On(() => persona.SegundoNombre).IsInsensitiveLike(Filtro, MatchMode.Start)
                        || Restrictions.On(() => persona.Apellido).IsInsensitiveLike(Filtro, MatchMode.Start)
                );
            }

            query.JoinQueryOver<CambioEstadoTurno>(x => x.CambiosDeEstado).JoinQueryOver(x => x.Responsable)
                .TransformUsing(new DistinctRootEntityResultTransformer());

            var turnos = query.List();

            if (turnos.Any()) return turnos.Select(Convert);

            return new List<FichaPacienteReportModel> { AppendPacienteInfo(new FichaPacienteReportModel()) };
        }

        private FichaPacienteReportModel Convert(Turno turno)
        {
            var model = new FichaPacienteReportModel
            {
                FechaTurno = turno.FechaTurno,
                Profesional = turno.Profesional.Persona.NombreCompleto,
                Consultorio = turno.Consultorio != null ? turno.Consultorio.Nombre : string.Empty,
                Especialidad = turno.Especialidad != null ? turno.Especialidad.Nombre : string.Empty,
                Nota = turno.Nota
            };
            return AppendPacienteInfo(model);
        }

        private FichaPacienteReportModel AppendPacienteInfo(FichaPacienteReportModel model)
        {
            model.NombrePaciente = _paciente.Persona.NombreCompleto;
            model.Email = _paciente.Persona.Membership.Email;

            if (_paciente.Persona.Documento != null)
            {
                model.DocumentoTipo = _paciente.Persona.Documento.TipoDocumento.DisplayName;
                model.DocumentoNumero = _paciente.Persona.Documento.Numero.ToString();
            }

            if (_paciente.Persona.Domicilio != null)
            {
                model.Domicilio = _paciente.Persona.Domicilio.Direccion;
                if (_paciente.Persona.Domicilio.Localidad != null)
                {
                    model.Domicilio = string.Format("{0}, {1} ({2}), {3}", model.Domicilio,
                                                    _paciente.Persona.Domicilio.Localidad.Nombre,
                                                    _paciente.Persona.Domicilio.Localidad.CodigoPostal,
                                                    _paciente.Persona.Domicilio.Localidad.Provincia.Nombre);
                }
            }

            if (_paciente.Persona.Telefono != null)
            {
                model.Telefono = string.Format("({0}) - {1}", _paciente.Persona.Telefono.Prefijo,
                                               _paciente.Persona.Telefono.Numero);
            }

            if (_paciente.Persona.FechaNacimiento.HasValue)
                model.FechaNacimiento = _paciente.Persona.FechaNacimiento.Value.ToShortDateString();

            if (_paciente.Persona.Edad.HasValue)
                model.Edad = _paciente.Persona.Edad.Value.ToString();

            if (_paciente.Persona.Peso.HasValue)
                model.Peso = _paciente.Persona.Peso.Value.ToString();

            if (_paciente.Persona.Altura.HasValue)
                model.Altura = _paciente.Persona.Altura.Value.ToString();

            model.NumeroAfiliado = _paciente.NumeroAfiliado;

            if (_paciente.Plan != null)
            {
                model.Plan = _paciente.Plan.Nombre;
                model.ObraSocial = _paciente.Plan.ObraSocial.RazonSocial;
            }
            return model;
        }
    }
}