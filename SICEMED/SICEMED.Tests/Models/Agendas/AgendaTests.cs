using System;
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
            var profesional = ApplicationInstaller.PersonaProfesionalBernardoClinico.As<Profesional>();

            profesional.AgregarAgenda(DayOfWeek.Tuesday, TimeSpan.FromMinutes(15), new DateTime(2000, 1,1,10,0,0), new DateTime(2000,1,1,12,0,0), ApplicationInstaller.ConsultorioA, ApplicationInstaller.EspecialidadClinico);
        }
    }
}