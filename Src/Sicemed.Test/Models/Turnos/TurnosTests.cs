using System;
using System.Linq;
using NUnit.Framework;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Tests.Models.Turnos
{
    [TestFixture]
    public class TurnosTests : InitializeNhibernate
    {
        [Test]
        public void PuedoOtorgarUnTurnoPersonalmente()
        {
            var paciente = ApplicationInstaller.PersonaPacientePablo.As<Paciente>();
            var profesional = ApplicationInstaller.PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>();
            var especialidad = ApplicationInstaller.EspecialidadClinico;
            var secretariaOtorgaTurno = ApplicationInstaller.PersonaSecretariaJuana.As<Secretaria>();
            var consultorio = ApplicationInstaller.ConsultorioA;
            var fechaTurno = new DateTime(2011, 02, 01);

            var session = SessionFactory.GetCurrentSession();
            var turno = Turno.Create(fechaTurno, paciente, profesional, especialidad, secretariaOtorgaTurno, consultorio);

            session.Save(turno);

            session.Flush();
            session.Evict(turno);

            var turnoDb = session.Get<Turno>(turno.Id);

            Assert.NotNull(turnoDb);
            Assert.AreEqual(turno.Id, turnoDb.Id);
            Assert.AreEqual(turno.Paciente, turnoDb.Paciente);
            Assert.AreEqual(turno.Profesional, turnoDb.Profesional);
            Assert.AreEqual(turno.Especialidad, turnoDb.Especialidad);
            Assert.AreEqual(turno.FechaTurno, turnoDb.FechaTurno);
            Assert.AreEqual(turno.SecretariaReservadoraTurno, turnoDb.SecretariaReservadoraTurno);
            Assert.IsNullOrEmpty(turnoDb.IpPaciente);
        }

        [Test]
        public void PuedoOtorgarUnTurnoViaWeb()
        {
            var paciente = ApplicationInstaller.PersonaPacientePablo.As<Paciente>();
            var profesional = ApplicationInstaller.PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>();
            var especialidad = ApplicationInstaller.EspecialidadClinico;
            var consultorio = ApplicationInstaller.ConsultorioA;
            var fechaTurno = new DateTime(2011, 02, 01);

            var session = SessionFactory.GetCurrentSession();
            var turno = Turno.Create(fechaTurno, paciente, profesional, especialidad, consultorio, "127.0.0.1");

            session.Save(turno);

            session.Flush();
            session.Evict(turno);

            var turnoDb = session.Get<Turno>(turno.Id);

            Assert.NotNull(turnoDb);
            Assert.AreEqual(turno.Id, turnoDb.Id);
            Assert.AreEqual(turno.Paciente, turnoDb.Paciente);
            Assert.AreEqual(turno.Profesional, turnoDb.Profesional);
            Assert.AreEqual(turno.Especialidad, turnoDb.Especialidad);
            Assert.AreEqual(turno.FechaTurno, turnoDb.FechaTurno);
            Assert.AreEqual(turno.IpPaciente, turnoDb.IpPaciente);
            Assert.IsNull(turnoDb.SecretariaReservadoraTurno);
        }

        [Test]
        public void NoPuedoOtorgarUnTurnoAUnaEspecialidadQueNoAtiendeElProfesional()
        {
            var paciente = ApplicationInstaller.PersonaPacientePablo.As<Paciente>();
            var profesional = ApplicationInstaller.PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>();
            var especialidad = ApplicationInstaller.EspecialidadPediatra;
            var consultorio = ApplicationInstaller.ConsultorioA;
            var fechaTurno = new DateTime(2011, 02, 01);

            Assert.Throws<ArgumentException>(() => Turno.Create(fechaTurno, paciente, profesional, especialidad, consultorio, "127.0.0.1"));
        }

        [Test]
        public void PuedoOtorgarUnTurnoViaWeb2()
        {
            var paciente = ApplicationInstaller.PersonaPacientePablo.As<Paciente>();
            var profesional = ApplicationInstaller.PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>();
            var especialidad = ApplicationInstaller.EspecialidadClinico;
            var consultorio = ApplicationInstaller.ConsultorioA;

            var fechaTurno = new DateTime(2011, 02, 01);

            profesional.AgregarAgenda(
                DayOfWeek.Tuesday,
                TimeSpan.FromMinutes(15),
                new DateTime(2000, 1, 1, 10, 0, 0),
                new DateTime(2000, 1, 1, 12, 0, 0),
                ApplicationInstaller.ConsultorioA,
                ApplicationInstaller.EspecialidadClinico
            );

            var session = SessionFactory.GetCurrentSession();

            session.Update(profesional);
            session.Flush();
            session.Evict(profesional);
            
            var turno = Turno.Create(fechaTurno, paciente, profesional, especialidad, consultorio, "127.0.0.1");

            session.Save(turno);

            session.Flush();
            session.Evict(turno);

            var turnoDb = session.Get<Turno>(turno.Id);

            Assert.NotNull(turnoDb);
            Assert.AreEqual(turno.Id, turnoDb.Id);
            Assert.AreEqual(turno.Paciente, turnoDb.Paciente);
            Assert.AreEqual(turno.Profesional, turnoDb.Profesional);
            Assert.AreEqual(turno.Especialidad, turnoDb.Especialidad);
            Assert.AreEqual(turno.FechaTurno, turnoDb.FechaTurno);
            Assert.AreEqual(turno.IpPaciente, turnoDb.IpPaciente);
            Assert.IsNull(turnoDb.SecretariaReservadoraTurno);
        }

        [Test]
        public void NoPuedoOtorgarUnTurnoViaWebConAgendaQueNoPerteneceAlProfesional()
        {
            var paciente = ApplicationInstaller.PersonaPacientePablo.As<Paciente>();
            var profesional = ApplicationInstaller.PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>();
            var especialidad = ApplicationInstaller.EspecialidadPediatra;
            var consultorio = ApplicationInstaller.ConsultorioA;
            var fechaTurno = new DateTime(2011, 02, 01);

            var agenda = new Agenda()
                             {
                                 Dia = DayOfWeek.Tuesday,
                                 DuracionTurno = TimeSpan.FromMinutes(15),
                                 HorarioDesde = new DateTime(2000, 1, 1, 10, 0, 0),
                                 HorarioHasta = new DateTime(2000, 1, 1, 12, 0, 0),
                                 Consultorio = ApplicationInstaller.ConsultorioA                                 
                             };
            agenda.AgregarEspecialidad(ApplicationInstaller.EspecialidadClinico);

            var session = SessionFactory.GetCurrentSession();

            session.Update(profesional);
            session.Flush();
            session.Evict(profesional);

            Assert.Throws<ArgumentException>(() => Turno.Create(fechaTurno, paciente, profesional, especialidad, consultorio, "127.0.0.1"));
        }

    }
}