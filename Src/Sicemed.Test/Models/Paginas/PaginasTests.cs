﻿using System;
using System.Linq;
using NUnit.Framework;
using SharpTestsEx;
using Sicemed.Web.Infrastructure.Queries.Paginas;
using Sicemed.Web.Models;

namespace Sicemed.Tests.Models.Paginas
{
    [TestFixture]
    public class PaginasTests : InitializeNhibernate
    {
        [Test]
        public void PuedoAgregarUnArbolDeHijos()
        {
            Pagina padre = null;
            using (var tx = Session.BeginTransaction())
            {
                padre = new Pagina { Nombre = "Prueba Padre", Contenido = "HTML", Url="/1" };
                var pagina2 = new Pagina { Nombre = "Prueba Hijo 1", Contenido = "HTML", Url = "/2" };
                var pagina3 = new Pagina { Nombre = "Prueba Hijo 2", Contenido = "HTML", Url = "/3" };

                pagina2.AgregarHijo(pagina3);
                padre.AgregarHijo(pagina2);

                Session.Save(padre);
                tx.Commit();
            }

            Session.Flush();
            Session.Evict(padre);

            var pagina = Session.Get<Pagina>(padre.Id);

            Assert.NotNull(pagina);
            pagina.Hijos.Count().Should().Be(1);
            pagina.Hijos.First().Hijos.Count().Should().Be(1);
        }

        [Test]
        public void PuedoAgregarUnaPagina()
        {
            Pagina pagina;
            using (var tx = Session.BeginTransaction())
            {
                pagina = new Pagina { Nombre = "Prueba", Contenido = "HTML", Url = "/" };
                Session.Save(pagina);
                tx.Commit();
            }

            Session.Flush();
            Session.Evict(pagina);

            var db = Session.Get<Pagina>(pagina.Id);
            db.Nombre.Should().Be("Prueba");
        }

        [Test]
        public void PuedoAgregarUnaPaginaConHijos()
        {
            Pagina padre = null;
            using (var tx = Session.BeginTransaction())
            {
                padre = new Pagina { Nombre = "Prueba Padre", Contenido = "HTML", Url = "/1" };
                var pagina2 = new Pagina { Nombre = "Prueba Hijo 1", Contenido = "HTML", Url = "/2" };
                var pagina3 = new Pagina { Nombre = "Prueba Hijo 2", Contenido = "HTML", Url = "/3" };

                padre.AgregarHijo(pagina2);
                padre.AgregarHijo(pagina3);

                Session.Save(padre);
                tx.Commit();
            }

            Session.Flush();
            Session.Evict(padre);

            var pagina = Session.Get<Pagina>(padre.Id);

            Assert.NotNull(pagina);
            pagina.Hijos.Count().Should().Be(2);
        }

        [Test]
        public void PuedoConsultarLasPaginasRaiz()
        {
            using (var tx = Session.BeginTransaction())
            {
                var padre = new Pagina { Nombre = "Prueba Padre", Contenido = "HTML", Url = "/1" };
                var pagina2 = new Pagina { Nombre = "Prueba Hijo 1", Contenido = "HTML", Url = "/2" };
                var pagina3 = new Pagina { Nombre = "Prueba Hijo 2", Contenido = "HTML", Url = "/3" };

                padre.AgregarHijo(pagina2);

                Session.Save(padre);
                Session.Save(pagina3);
                tx.Commit();
            }

            Session.Flush();

            var roots = new ObtenerPaginasRaizQuery { SessionFactory = SessionFactory }.Execute();

            roots.Count().Should().Be(5); //Dos ya existen en el app initializer
        }
    }
}