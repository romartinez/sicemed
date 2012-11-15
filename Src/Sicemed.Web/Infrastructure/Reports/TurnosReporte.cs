using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Transform;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components;
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
            set { 
                _pacienteId = value;
                _paciente = SessionFactory.GetCurrentSession().Load<Paciente>(value);
            }
        }

        public override string Title { get { return string.Format("Ficha Paciente: {0}", _paciente.Persona.NombreCompleto); } }

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

            var turnos = query.Future();

            return turnos.Select(Convert);
        }

        private FichaPacienteReportModel Convert(Turno turno)
        {
            var model = new FichaPacienteReportModel
            {
                NombrePaciente = turno.Paciente.Persona.NombreCompleto,
                Email = turno.Paciente.Persona.Membership.Email,

                FechaTurno = turno.FechaTurno,
                Profesional = turno.Profesional.Persona.NombreCompleto,
                Consultorio = turno.Consultorio != null ? turno.Consultorio.Nombre : string.Empty,
                Especialidad = turno.Especialidad != null ? turno.Especialidad.Descripcion : string.Empty,
                Nota = turno.Nota
            };

            if (turno.Paciente.Persona.Documento != null)
            {
                model.DocumentoTipo = turno.Paciente.Persona.Documento.TipoDocumento.DisplayName;
                model.DocumentoNumero = turno.Paciente.Persona.Documento.Numero.ToString();
            }

            if (turno.Paciente.Persona.Domicilio != null)
            {
                model.Domicilio = turno.Paciente.Persona.Domicilio.Direccion;
                if (turno.Paciente.Persona.Domicilio.Localidad != null)
                {
                    model.Domicilio = string.Format("{0}, {1} ({2}), {3}", model.Domicilio,
                                                    turno.Paciente.Persona.Domicilio.Localidad.Nombre,
                                                    turno.Paciente.Persona.Domicilio.Localidad.CodigoPostal,
                                                    turno.Paciente.Persona.Domicilio.Localidad.Provincia.Nombre);
                }
            }

            if (turno.Paciente.Persona.Telefono != null)
            {
                model.Telefono = string.Format("({0}) - {1}", turno.Paciente.Persona.Telefono.Prefijo,
                                               turno.Paciente.Persona.Telefono.Numero);
            }

            if (turno.Paciente.Persona.FechaNacimiento.HasValue)
                model.FechaNacimiento = turno.Paciente.Persona.FechaNacimiento.Value.ToShortDateString();

            if (turno.Paciente.Persona.Edad.HasValue)
                model.Edad = turno.Paciente.Persona.Edad.Value.ToString();

            if (turno.Paciente.Persona.Peso.HasValue)
                model.Peso = turno.Paciente.Persona.Peso.Value.ToString();

            if (turno.Paciente.Persona.Altura.HasValue)
                model.Altura = turno.Paciente.Persona.Altura.Value.ToString();

            model.NumeroAfiliado = turno.Paciente.NumeroAfiliado;

            if (turno.Paciente.Plan != null)
            {
                model.Plan = turno.Paciente.Plan.Nombre;
                model.ObraSocial = turno.Paciente.Plan.ObraSocial.RazonSocial;
            }

            return model;
        }
    }
}