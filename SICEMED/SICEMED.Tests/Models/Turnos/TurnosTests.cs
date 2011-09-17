using System;
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
        public void PuedoOtorgarUnTurnoPersonalmenteSinAgenda()
        {
            var paciente = ApplicationInstaller.PersonaPacientePablo.As<Paciente>();
            var profesional = ApplicationInstaller.PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>();
            var especialidad = ApplicationInstaller.EspecialidadClinico;
            var secretariaOtorgaTurno = ApplicationInstaller.PersonaSecretariaJuana.As<Secretaria>();
            var fechaTurno = new DateTime(2011, 02, 01);

            var session = SessionFactory.GetCurrentSession();
            var turno = Turno.Create(fechaTurno, paciente, profesional, especialidad, secretariaOtorgaTurno);

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
        public void PuedoOtorgarUnTurnoViaWebSinAgenda()
        {
            var paciente = ApplicationInstaller.PersonaPacientePablo.As<Paciente>();
            var profesional = ApplicationInstaller.PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>();
            var especialidad = ApplicationInstaller.EspecialidadClinico;
            var fechaTurno = new DateTime(2011, 02, 01);

            var session = SessionFactory.GetCurrentSession();
            var turno = Turno.Create(fechaTurno, paciente, profesional, especialidad, "127.0.0.1");

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
            var fechaTurno = new DateTime(2011, 02, 01);

            Assert.Throws<ArgumentException>(() => Turno.Create(fechaTurno, paciente, profesional, especialidad, "127.0.0.1"));
        }
    }
}