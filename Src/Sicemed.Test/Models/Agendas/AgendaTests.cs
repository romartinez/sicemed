using System;
using System.Linq;
using NUnit.Framework;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Tests.Models.Agendas
{
    [TestFixture]
    public class AgendaTests : InitializeNhibernate
    {

        [Test]
        public void PuedoAsignarUnaAgendaAUnProfesional()
        {
            var session = SessionFactory.GetCurrentSession();
            var profesional = session.Get<Persona>(ApplicationInstaller.PersonaProfesionalBernardoClinico.Id).As<Profesional>();
            var consultorio = session.Get<Consultorio>(ApplicationInstaller.ConsultorioA.Id);
            var clinico = session.Get<Especialidad>(ApplicationInstaller.EspecialidadClinico.Id);


            profesional.AgregarAgenda(
                DayOfWeek.Tuesday,
                TimeSpan.FromMinutes(15),
                new DateTime(2000, 1, 1, 10, 0, 0),
                new DateTime(2000, 1, 1, 12, 0, 0),
                consultorio,
                clinico
            );

            session.Update(profesional);

            session.Flush();
            session.Evict(profesional);

            var personaDb = session.Get<Persona>(profesional.Persona.Id);
            Assert.IsNotNull(personaDb);

            var profesionalDb = personaDb.As<Profesional>();

            Assert.AreEqual(1, profesionalDb.Agendas.Count(x => x.Dia == DayOfWeek.Tuesday && x.DuracionTurno == TimeSpan.FromMinutes(15)));
        }

        [Test]
        public void NoPuedoAsignarUnaAgendaAUnProfesionalConUnaEspecialidadQueNoPosee()
        {
            var profesional = ApplicationInstaller.PersonaProfesionalBernardoClinico.As<Profesional>();

            Assert.Throws<ArgumentException>(() =>
                profesional.AgregarAgenda(
                    DayOfWeek.Tuesday,
                    TimeSpan.FromMinutes(15),
                    new DateTime(2000, 1, 1, 10, 0, 0),
                    new DateTime(2000, 1, 1, 12, 0, 0),
                    ApplicationInstaller.ConsultorioA,
                    ApplicationInstaller.EspecialidadDermatologo
                )
            );
        }

        [Test]
        public void NoPuedoAsignarUnaAgendaAUnProfesionalConUnHorarioHastaMenorAlHorarioDesde()
        {
            var profesional = ApplicationInstaller.PersonaProfesionalBernardoClinico.As<Profesional>();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                             profesional.AgregarAgenda(
                                                 DayOfWeek.Tuesday,
                                                 TimeSpan.FromMinutes(15),
                                                 new DateTime(2000, 1, 1, 10, 0, 0),
                                                 new DateTime(2000, 1, 1, 9, 0, 0),
                                                 ApplicationInstaller.ConsultorioA,
                                                 ApplicationInstaller.EspecialidadClinico
                                                 )
                );
        }
    }
}