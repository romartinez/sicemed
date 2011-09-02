using NUnit.Framework;
using SharpTestsEx;
using Sicemed.Web.Models;

namespace Sicemed.Tests.Models.Paginas
{
    [TestFixture]
    public class PaginasTests : InitializeNhibernate
    {
         
        [Test]
        public void PuedoAgregarUnaPagina()
        {
            using(var tx = Session.BeginTransaction())
            {
                var pagina = new Pagina() {Nombre = "Prueba", Contenido = "HTML"};
                Session.Save(pagina);
                tx.Commit();
            }
        }

        [Test]
        public void PuedoAgregarUnaPaginaConHijos()
        {
            using (var tx = Session.BeginTransaction())
            {
                var pagina = new Pagina() { Nombre = "Prueba", Contenido = "HTML" };
                var pagina2 = new Pagina() { Nombre = "Prueba", Contenido = "HTML" };
                var pagina3 = new Pagina() { Nombre = "Prueba", Contenido = "HTML" };

                pagina.AgregarHijo(pagina2);
                pagina.AgregarHijo(pagina3);

                Session.Save(pagina);
                tx.Commit();
            }
        }
    
    }
}