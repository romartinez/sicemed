using System;
using System.Web;
using Castle.Core.Logging;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Exceptions;
using NHibernate.Tool.hbm2ddl;
using Sicemed.Web.Infrastructure.Providers.Session;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations.Documentos;
using Sicemed.Web.Models.Enumerations.Roles;

namespace Sicemed.Web.Infrastructure
{
    public interface IApplicationInstaller
    {
        void Install(Configuration config);
    }

    public sealed class ApplicationInstaller : IApplicationInstaller
    {
        #region Public Static DB Entities
        public static Provincia ProvinciaSantaFe;
        public static Provincia ProvinciaBuenosAires;
        public static Provincia ProvinciaCordoba;

        public static Localidad LocalidadPergamino;
        public static Localidad LocalidadSanNicolas;
        public static Localidad LocalidadRosario;
        public static Localidad LocalidadSantaFe;
        public static Localidad LocalidadCarlosPaz;
        public static Localidad LocalidadMarcosJuarez;

        public static Consultorio ConsultorioA;
        public static Consultorio ConsultorioB;

        public static Especialidad EspecialidadPediatra;
        public static Especialidad EspecialidadClinico;
        public static Especialidad EspecialidadDermatologo;

        public static Pagina PaginaHome;
        public static Pagina PaginaAboutUs;

        public static Feriado FeriadoCarnaval;
        public static Feriado FeriadoDiaIndependencia;

        public static ObraSocial ObraSocialOsde;
        public static ObraSocial ObraSocialSwissMedical;

        public static Plan PlanOsdeGold;
        public static Plan PlanOsdeSilver;
        public static Plan PlanOsdeNeo;
        public static Plan PlanSwissNbsf;
        public static Plan PlanSwissSb64;

        public static Usuario UsuarioAdmin;
        public static Usuario UsuarioPacientePablo;
        public static Usuario UsuarioPacientePedro;
        public static Usuario UsuarioSecretariaJuana;
        public static Usuario UsuarioProfesionalBernardo;
        public static Usuario UsuarioProfesionalJose;
        public static Usuario UsuarioAdminProfesionalWalter;
        #endregion

        public ISessionFactory SessionFactory { get; set; }
        public ILogger Logger { get; set; }
        public IMembershipService MembershipService { get; set; }

        public ApplicationInstaller()
        {
            Logger = NullLogger.Instance;
        }

        public void Install(Configuration config)
        {
            var session = SessionFactory.GetCurrentSession() ?? SessionFactory.OpenSession();
            using (var importSession = SessionFactory.OpenSession(session.Connection))
            {
                if (HttpContext.Current != null)
                    LazySessionContext.Bind(new Lazy<ISession>(() => importSession), SessionFactory);
                else
                    CurrentSessionContext.Bind(importSession);

                Logger.InfoFormat("Checking if the application is installed.");
                try
                {
                    var param = importSession.Get<Parametro>(Parametro.Keys.APP_IS_INITIALIZED);
                    var isInitialized = param == null || param.Get<bool>();
                    Logger.DebugFormat("The parameter for the DB Installed is: {0}", isInitialized);
                    if (!isInitialized)
                    {
                        Initialize(config, importSession);
                    }
                }
                catch (GenericADOException)
                {
                    Logger.WarnFormat("The DB isn't initialized, generating it.");
                    //Check if the DB is created
                    Initialize(config, importSession);
                }
                if (HttpContext.Current != null)
                    LazySessionContext.UnBind(SessionFactory);
                else
                    CurrentSessionContext.Unbind(SessionFactory);
            }
        }

        private void Initialize(Configuration config, ISession session)
        {
            Logger.InfoFormat("Installing the application.");
            new SchemaExport(config).Execute(false, true, false, session.Connection, null);
            //No permite Tx anidadas
            CrearProvincias(session);
            CrearLocalidades(session);
            CrearEspecialidades(session);
            CrearConsultorios(session);
            CrearFeriados(session);
            CrearObrasSociales(session);
            CrearPlanes(session);
            CrearPaginas(session);
            CrearUsuarios();
            //Lo guardo al parametro nuevo.
            using (var tx = session.BeginTransaction())
            {
                var param = new Parametro(Parametro.Keys.APP_IS_INITIALIZED);
                param.Set(true);

                session.Save(param);
                tx.Commit();
            }
        }

        #region Entity Creation Methods

        private void CrearPaginas(ISession session)
        {
            PaginaHome = new Pagina { Nombre = "Home", Contenido = "Hola a todos bienvenidos a SICEMED" };
            PaginaAboutUs = new Pagina { Nombre = "About Us", Contenido = "Somos una empresa en pleno crecimiento." };

            session.Save(PaginaAboutUs);
            session.Save(PaginaHome);
        }

        private void CrearObrasSociales(ISession session)
        {
            ObraSocialOsde = new ObraSocial
                                 {
                RazonSocial = "OSDE",
                Documento = new Documento
                                {
                                    Numero = 301231234,
                                    TipoDocumento = TipoDocumento.Cuit
                                },
                Telefono = new Telefono
                               {
                                Prefijo = "0341",
                                Numero = "4481010"
                            },
                Domicilio =
                    new Domicilio
                        {
                            Direccion = "Bv. Oro�o 345",
                            Localidad = LocalidadRosario
                        }
            };
            ObraSocialSwissMedical = new ObraSocial
                                         {
                RazonSocial = "Swiss Medical Group",
                Documento = new Documento
                                {
                    Numero = 301456938,
                    TipoDocumento = TipoDocumento.Cuit
                },
                Telefono = new Telefono
                               {
                    Prefijo = "0341",
                    Numero = "4473023"
                },
                Domicilio =
                    new Domicilio
                        {
                        Direccion = "Rioja 1289",
                        Localidad = LocalidadRosario
                    }
            };

            session.Save(ObraSocialOsde);
            session.Save(ObraSocialSwissMedical);
        }

        private void CrearPlanes(ISession session)
        {
            PlanOsdeGold = new Plan { Nombre = "Gold", ObraSocial = ObraSocialOsde };
            PlanOsdeSilver = new Plan { Nombre = "Silver", ObraSocial = ObraSocialOsde };
            PlanOsdeNeo = new Plan { Nombre = "Neo", ObraSocial = ObraSocialOsde };

            PlanSwissNbsf = new Plan { Nombre = "Convenio Nuevo Banco De Santa Fe", ObraSocial = ObraSocialSwissMedical };
            PlanSwissSb64 = new Plan { Nombre = "SB64", ObraSocial = ObraSocialSwissMedical };

            session.Save(PlanOsdeGold);
            session.Save(PlanOsdeSilver);
            session.Save(PlanOsdeNeo);
            session.Save(PlanSwissNbsf);
            session.Save(PlanSwissSb64);
        }

        private void CrearFeriados(ISession session)
        {
            FeriadoCarnaval = new Feriado { Nombre = "Carnaval", Fecha = new DateTime(2011, 03, 07) };
            FeriadoDiaIndependencia = new Feriado { Nombre = "D�a De La Independencia", Fecha = new DateTime(2011, 07, 11) };

            session.Save(FeriadoCarnaval);
            session.Save(FeriadoDiaIndependencia);
        }

        private void CrearProvincias(ISession session)
        {
            ProvinciaSantaFe = new Provincia { Nombre = "Santa Fe" };
            ProvinciaBuenosAires = new Provincia { Nombre = "Buenos Aires" };
            ProvinciaCordoba = new Provincia { Nombre = "C�rdoba" };

            session.Save(ProvinciaCordoba);
            session.Save(ProvinciaBuenosAires);
            session.Save(ProvinciaSantaFe);
        }

        private void CrearLocalidades(ISession session)
        {
            LocalidadCarlosPaz = new Localidad { Nombre = "Villa Carlos Paz", Provincia = ProvinciaCordoba };
            LocalidadMarcosJuarez = new Localidad { Nombre = "Marcos Juarez", Provincia = ProvinciaCordoba };
            LocalidadPergamino = new Localidad { Nombre = "Pergamino", Provincia = ProvinciaBuenosAires };
            LocalidadSanNicolas = new Localidad { Nombre = "San Nicol�s De Los Arroyos", Provincia = ProvinciaBuenosAires };
            LocalidadRosario = new Localidad { Nombre = "Rosario", Provincia = ProvinciaSantaFe };
            LocalidadSantaFe = new Localidad { Nombre = "Santa Fe", Provincia = ProvinciaSantaFe };


            session.Save(LocalidadCarlosPaz);
            session.Save(LocalidadMarcosJuarez);
            session.Save(LocalidadPergamino);
            session.Save(LocalidadSanNicolas);
            session.Save(LocalidadRosario);
            session.Save(LocalidadSantaFe);
        }

        private void CrearConsultorios(ISession session)
        {
            ConsultorioA = new Consultorio { Nombre = "Consultorio A" };
            ConsultorioB = new Consultorio { Nombre = "Consultorio B" };

            session.Save(ConsultorioA);
            session.Save(ConsultorioB);
        }

        private void CrearEspecialidades(ISession session)
        {
            EspecialidadClinico = new Especialidad { Nombre = "Cl�nico" };
            EspecialidadDermatologo = new Especialidad { Nombre = "Dermat�logo" };
            EspecialidadPediatra = new Especialidad { Nombre = "Pediatra" };

            session.Save(EspecialidadClinico);
            session.Save(EspecialidadDermatologo);
            session.Save(EspecialidadPediatra);
        }

        private void CrearUsuarios()
        {
            UsuarioAdmin = new Usuario { Nombre = "Administrador", Apellido = "Administrador" };
            UsuarioAdmin.AgregarRol(Rol.Administrador);
            MembershipService.CreateUser(UsuarioAdmin, "admin@gmail.com", "sicemedAdmin");

            UsuarioAdminProfesionalWalter = new Usuario
                                                {
                Nombre = "Walter",
                Apellido = "Blanch",
                SegundoNombre = "Gaston",
                FechaNacimiento = new DateTime(1985, 03, 03),
                EstaHabilitadoTurnosWeb = true,
                Domicilio = new Domicilio { Direccion = "Mendoza 1123 7�A", Localidad = LocalidadRosario },
                Documento = new Documento { Numero = 31364468, TipoDocumento = TipoDocumento.Dni },
                Telefono = new Telefono { Prefijo = "0341", Numero = "153353273" }
            };
            UsuarioAdminProfesionalWalter.AgregarRol(Rol.Administrador);
            UsuarioAdminProfesionalWalter.AgregarRol(Rol.Profesional);
            MembershipService.CreateUser(UsuarioAdminProfesionalWalter, "walter@gmail.com", "sicemedWalter");

            UsuarioPacientePablo = new Usuario
                                       {
                Nombre = "Pablo",
                Apellido = "Dominguez",
                FechaNacimiento = new DateTime(1987, 03, 07),
                EstaHabilitadoTurnosWeb = true,
                Domicilio = new Domicilio { Direccion = "San Luis 323", Localidad = LocalidadRosario },
                Documento = new Documento { Numero = 1234568, TipoDocumento = TipoDocumento.Dni },
                Telefono = new Telefono { Prefijo = "0341", Numero = "1534665" }
            };
            MembershipService.CreateUser(UsuarioPacientePablo, "pablo@gmail.com", "sicemedPablo");

            UsuarioPacientePedro = new Usuario
                                       {
                Nombre = "Pedro",
                Apellido = "Hernandez",
                FechaNacimiento = new DateTime(1975, 6, 15),
                EstaHabilitadoTurnosWeb = true,
                Domicilio = new Domicilio { Direccion = "Rioja 1344", Localidad = LocalidadRosario },
                Documento = new Documento { Numero = 11234343, TipoDocumento = TipoDocumento.Dni },
                Telefono = new Telefono { Prefijo = "0341", Numero = "156333456" }
            };
            MembershipService.CreateUser(UsuarioPacientePedro, "pedro@gmail.com", "sicemedPedro");

            UsuarioSecretariaJuana = new Usuario
                                         {
                Nombre = "Juana",
                Apellido = "Alvarez",
                FechaNacimiento = new DateTime(1990, 1, 27),
                EstaHabilitadoTurnosWeb = true,
                Domicilio = new Domicilio { Direccion = "Ovideo Lagos 343", Localidad = LocalidadRosario },
                Documento = new Documento { Numero = 10112123, TipoDocumento = TipoDocumento.Dni },
                Telefono = new Telefono { Prefijo = "0341", Numero = "4481123" }
            };
            UsuarioSecretariaJuana.AgregarRol(Rol.Secretaria);
            MembershipService.CreateUser(UsuarioSecretariaJuana, "juana@gmail.com", "sicemedJuana");

            UsuarioProfesionalBernardo = new Usuario
                                             {
                Nombre = "Bernardo",
                Apellido = "Lattanzio",
                FechaNacimiento = new DateTime(1984, 6, 14),
                EstaHabilitadoTurnosWeb = true,
                Domicilio = new Domicilio { Direccion = "Italia 1243", Localidad = LocalidadRosario },
                Documento = new Documento { Numero = 30123876, TipoDocumento = TipoDocumento.Dni },
                Telefono = new Telefono { Prefijo = "0341", Numero = "4471010" }
            };
            UsuarioProfesionalBernardo.AgregarRol(Rol.Profesional);
            MembershipService.CreateUser(UsuarioProfesionalBernardo, "bernardo@gmail.com", "sicemedBernardo");

            UsuarioProfesionalJose = new Usuario
                                         {
                Nombre = "Jose",
                Apellido = "Bernav�",
                SegundoNombre = "Ignacio",
                FechaNacimiento = new DateTime(1985, 7, 23),
                EstaHabilitadoTurnosWeb = true,
                Domicilio = new Domicilio { Direccion = "Brown 123 7�a", Localidad = LocalidadRosario },
                Documento = new Documento { Numero = 31789502, TipoDocumento = TipoDocumento.Dni },
                Telefono = new Telefono { Prefijo = "0341", Numero = "1661234" }
            };
            UsuarioProfesionalJose.AgregarRol(Rol.Profesional);
            MembershipService.CreateUser(UsuarioProfesionalJose, "jose@gmail.com", "sicemedJose");
        }
        #endregion
    }
}