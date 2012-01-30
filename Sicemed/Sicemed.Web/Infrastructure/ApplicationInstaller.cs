using System;
using System.Web;
using Castle.Core.Logging;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Exceptions;
using NHibernate.Tool.hbm2ddl;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations.Documentos;
using Sicemed.Web.Models.Roles;

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

        public static Persona PersonaAdmin;
        public static Persona PersonaPacientePablo;
        public static Persona PersonaPacientePedro;
        public static Persona PersonaSecretariaJuana;
        public static Persona PersonaProfesionalBernardoClinico;
        public static Persona PersonaProfesionalJoseClinicoYDermatologo;
        public static Persona PersonaAdminProfesionalWalter;

        #endregion

        public ApplicationInstaller()
        {
            Logger = NullLogger.Instance;
        }

        public ISessionFactory SessionFactory { get; set; }
        public ILogger Logger { get; set; }
        public IMembershipService MembershipService { get; set; }

        #region IApplicationInstaller Members

        public void Install(Configuration config)
        {            
            using (var importSession = SessionFactory.OpenSession())
            {
                ISession session = null;
                if(CurrentSessionContext.HasBind(SessionFactory))
                {
                    session = CurrentSessionContext.Unbind(SessionFactory);
                }
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
                
                CurrentSessionContext.Unbind(SessionFactory);   
                if(session != null)
                {
                    CurrentSessionContext.Bind(session);
                }
            }
        }

        #endregion

        private void Initialize(Configuration config, ISession session)
        {
            Logger.InfoFormat("Installing the application.");
            new SchemaExport(config).Execute(false, true, false);
            //No permite Tx anidadas
            CrearProvincias(session);
            CrearLocalidades(session);
            CrearEspecialidades(session);
            CrearConsultorios(session);
            CrearFeriados(session);
            CrearObrasSociales(session);
            CrearPlanes(session);
            CrearPaginas(session);
            CrearPersonas(session);
            CrearClinica(session);
            //Lo guardo al parametro nuevo.
            using (var tx = session.BeginTransaction())
            {
                var param = new Parametro(Parametro.Keys.APP_IS_INITIALIZED);
                param.Set(true);

                session.Save(param);
                tx.Commit();
            }

            Console.WriteLine(@"*************************************************");
            Console.WriteLine(@"*************************************************");
        }

        #region Entity Creation Methods
        private void CrearClinica(ISession session)
        {
            var clinica = new Clinica
                              {
                                  RazonSocial = "Centro Médico Integral Velez Sarsfield",
                                  DuracionTurnoPorDefecto = TimeSpan.FromMinutes(30),
                                  HorarioMatutinoDesde = new DateTime(2000, 1, 1, 8, 0, 0),
                                  HorarioMatutinoHasta = new DateTime(2000, 1, 1, 12, 0, 0),
                                  HorarioVespertinoDesde = new DateTime(2000, 1, 1, 15, 0, 0),
                                  HorarioVespertinoHasta = new DateTime(2000, 1, 1, 19, 0, 0),
                                  NumeroInasistenciasConsecutivasGeneranBloqueo = 5,
                                  Documento = new Documento
                                                  {
                                                      Numero = 13432423412,
                                                      TipoDocumento = TipoDocumento.Cuit
                                                  },
                                  Domicilio = new Domicilio
                                                  {
                                                      Direccion = "Av. Velez Sarsfield 453",
                                                      Localidad = LocalidadRosario
                                                  },
                                  Email = "contacto@sicemed.com.ar"
                              };
            clinica
                .AgregarTelefono(
                        new Telefono {Numero = "4487610", Prefijo = "0341"}
                ).AgregarTelefono(
                        new Telefono{Numero="4487610",Prefijo="0341"}
                );

            session.Save(clinica);
        }

        private void CrearPaginas(ISession session)
        {
            PaginaHome = new Pagina { Nombre = "Home", Contenido = "Hola a todos bienvenidos a SICEMED", Url = "", Orden = 0 };
            PaginaAboutUs = new Pagina { Nombre = "About Us", Contenido = "Somos una empresa en pleno crecimiento.", Url = "AboutUs", Orden = 0 };
            var paginaPadre = new Pagina { Nombre = "Con Hijos", Contenido = "Una pagina de prueba.", Url = "Padre", Orden = 0 };            
            for (var i = 0; i < 5; i++ )
            {
                var hijo = new Pagina()
                               {
                                   Nombre = "Hijo " + i,
                                   Contenido = "Una pagina de prueba.",
                                   Url = "Padre/Hijo-" + i,
                                   Orden = 0
                               };
                if(i == 0)
                {
                    hijo.AgregarHijo(new Pagina()
                    {
                        Nombre = "SubHijo",
                        Contenido = "Una pagina de prueba.",
                        Url = "Padre/Hijo-" + i + "/SubHijo",
                        Orden = 0
                    });
                }
                paginaPadre.AgregarHijo(hijo);
            }

            session.Save(paginaPadre);
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
                                         Direccion = "Bv. Oroño 345",
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
            FeriadoDiaIndependencia = new Feriado { Nombre = "Día De La Independencia", Fecha = new DateTime(2011, 07, 11) };

            session.Save(FeriadoCarnaval);
            session.Save(FeriadoDiaIndependencia);
        }

        private void CrearProvincias(ISession session)
        {
            ProvinciaSantaFe = new Provincia { Nombre = "Santa Fe" };
            ProvinciaBuenosAires = new Provincia { Nombre = "Buenos Aires" };
            ProvinciaCordoba = new Provincia { Nombre = "Córdoba" };

            session.Save(ProvinciaCordoba);
            session.Save(ProvinciaBuenosAires);
            session.Save(ProvinciaSantaFe);
        }

        private void CrearLocalidades(ISession session)
        {
            LocalidadCarlosPaz = new Localidad { Nombre = "Villa Carlos Paz", Provincia = ProvinciaCordoba };
            LocalidadMarcosJuarez = new Localidad { Nombre = "Marcos Juarez", Provincia = ProvinciaCordoba };
            LocalidadPergamino = new Localidad { Nombre = "Pergamino", Provincia = ProvinciaBuenosAires };
            LocalidadSanNicolas = new Localidad { Nombre = "San Nicolás De Los Arroyos", Provincia = ProvinciaBuenosAires };
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
            EspecialidadClinico = new Especialidad { Nombre = "Clínico" };
            EspecialidadDermatologo = new Especialidad { Nombre = "Dermatólogo" };
            EspecialidadPediatra = new Especialidad { Nombre = "Pediatra" };

            session.Save(EspecialidadClinico);
            session.Save(EspecialidadDermatologo);
            session.Save(EspecialidadPediatra);
        }

        private void CrearPersonas(ISession session)
        {
            PersonaAdmin = new Persona { Nombre = "Administrador", Apellido = "Administrador" };
            PersonaAdmin.AgregarRol(Administrador.Create());
            MembershipService.CreateUser(PersonaAdmin, "admin@gmail.com", "sicemedAdmin");
            session.Update(PersonaAdmin);

            PersonaAdminProfesionalWalter = new Persona
                                            {
                                                Nombre = "Walter",
                                                Apellido = "Blanch",
                                                SegundoNombre = "Gaston",
                                                FechaNacimiento = new DateTime(1985, 03, 03),
                                                Domicilio =
                                                    new Domicilio { Direccion = "Mendoza 1123 7°A", Localidad = LocalidadRosario },
                                                Documento =
                                                    new Documento { Numero = 31364468, TipoDocumento = TipoDocumento.Dni },
                                                Telefono = new Telefono { Prefijo = "0341", Numero = "153353273" }
                                            };
            PersonaAdminProfesionalWalter.AgregarRol(Administrador.Create());
            PersonaAdminProfesionalWalter.AgregarRol(Profesional.Create("46546546"));
            MembershipService.CreateUser(PersonaAdminProfesionalWalter, "walter@gmail.com", "sicemedWalter");

            session.Update(PersonaAdminProfesionalWalter);

            PersonaPacientePablo = new Persona
                                   {
                                       Nombre = "Pablo",
                                       Apellido = "Dominguez",
                                       FechaNacimiento = new DateTime(1987, 03, 07),
                                       Domicilio =
                                           new Domicilio { Direccion = "San Luis 323", Localidad = LocalidadRosario },
                                       Documento = new Documento { Numero = 1234568, TipoDocumento = TipoDocumento.Dni },
                                       Telefono = new Telefono { Prefijo = "0341", Numero = "1534665" }
                                   };
            PersonaPacientePablo.AgregarRol(Paciente.Create("9798798"));
            MembershipService.CreateUser(PersonaPacientePablo, "pablo@gmail.com", "sicemedPablo");
            session.Update(PersonaPacientePablo);

            PersonaPacientePedro = new Persona
                                   {
                                       Nombre = "Pedro",
                                       Apellido = "Hernandez",
                                       FechaNacimiento = new DateTime(1975, 6, 15),
                                       Domicilio =
                                           new Domicilio { Direccion = "Rioja 1344", Localidad = LocalidadRosario },
                                       Documento = new Documento { Numero = 11234343, TipoDocumento = TipoDocumento.Dni },
                                       Telefono = new Telefono { Prefijo = "0341", Numero = "156333456" }
                                   };
            PersonaPacientePedro.AgregarRol(Paciente.Create("9798798"));
            MembershipService.CreateUser(PersonaPacientePedro, "pedro@gmail.com", "sicemedPedro");
            session.Update(PersonaPacientePedro);

            PersonaSecretariaJuana = new Persona
                                     {
                                         Nombre = "Juana",
                                         Apellido = "Alvarez",
                                         FechaNacimiento = new DateTime(1990, 1, 27),
                                         Domicilio =
                                             new Domicilio { Direccion = "Ovideo Lagos 343", Localidad = LocalidadRosario },
                                         Documento =
                                             new Documento { Numero = 10112123, TipoDocumento = TipoDocumento.Dni },
                                         Telefono = new Telefono { Prefijo = "0341", Numero = "4481123" }
                                     };
            PersonaSecretariaJuana.AgregarRol(Secretaria.Create(DateTime.Now));
            MembershipService.CreateUser(PersonaSecretariaJuana, "juana@gmail.com", "sicemedJuana");
            session.Update(PersonaSecretariaJuana);

            PersonaProfesionalBernardoClinico = new Persona
                                         {
                                             Nombre = "Bernardo",
                                             Apellido = "Lattanzio",
                                             FechaNacimiento = new DateTime(1984, 6, 14),
                                             Domicilio =
                                                 new Domicilio { Direccion = "Italia 1243", Localidad = LocalidadRosario },
                                             Documento =
                                                 new Documento { Numero = 30123876, TipoDocumento = TipoDocumento.Dni },
                                             Telefono = new Telefono { Prefijo = "0341", Numero = "4471010" }
                                         };
            PersonaProfesionalBernardoClinico.AgregarRol(Profesional.Create("546546"));
            PersonaProfesionalBernardoClinico.As<Profesional>().AgregarEspecialidad(EspecialidadClinico);
            MembershipService.CreateUser(PersonaProfesionalBernardoClinico, "bernardo@gmail.com", "sicemedBernardo");
            session.Update(PersonaProfesionalBernardoClinico);
            session.Update(PersonaProfesionalBernardoClinico.As<Profesional>());

            PersonaProfesionalJoseClinicoYDermatologo = new Persona
                                     {
                                         Nombre = "Jose",
                                         Apellido = "Bernavá",
                                         SegundoNombre = "Ignacio",
                                         FechaNacimiento = new DateTime(1985, 7, 23),
                                         Domicilio =
                                             new Domicilio { Direccion = "Brown 123 7°a", Localidad = LocalidadRosario },
                                         Documento =
                                             new Documento { Numero = 31789502, TipoDocumento = TipoDocumento.Dni },
                                         Telefono = new Telefono { Prefijo = "0341", Numero = "1661234" }
                                     };
            PersonaProfesionalJoseClinicoYDermatologo.AgregarRol(Profesional.Create("546465489"));
            PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>().AgregarEspecialidad(EspecialidadClinico);
            PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>().AgregarEspecialidad(EspecialidadDermatologo);
            MembershipService.CreateUser(PersonaProfesionalJoseClinicoYDermatologo, "jose@gmail.com", "sicemedJose");
            session.Update(PersonaProfesionalJoseClinicoYDermatologo);
        }

        #endregion
    }
}