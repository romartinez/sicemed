using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Castle.Core.Logging;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Exceptions;
using NHibernate.Tool.hbm2ddl;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.BI;
using Sicemed.Web.Models.BI.Enumerations;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations.Documentos;
using Sicemed.Web.Models.Roles;
using Dapper;

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
        public static ObraSocial ObraSocialParticular;

        public static Plan PlanOsdeGold;
        public static Plan PlanOsdeSilver;
        public static Plan PlanOsdeNeo;
        public static Plan PlanSwissNbsf;
        public static Plan PlanSwissSb64;
        public static Plan PlanParticular;

        public static Persona PersonaAdmin;
        public static Persona PersonaPacientePablo;
        public static Persona PersonaPacientePedro;
        public static Persona PersonaSecretariaJuana;
        public static Persona PersonaProfesionalBernardoClinico;
        public static Persona PersonaProfesionalJoseClinicoYDermatologo;
        public static Persona PersonaAdminProfesionalWalter;

        public static CategoriaIndicador CategoriaIndicadorOperativo;
        public static CategoriaIndicador CategoriaIndicadorGerencial;

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
                if (CurrentSessionContext.HasBind(SessionFactory))
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
                if (session != null)
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
            CrearCategoriasBI(session);
            CrearIndicadoresBI(session);
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

            //Create Additional Tables and Scripts
            var sqlPath = HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data") :
                @"D:\Documents\Projects\sicemed\Src\Sicemed.Web\App_Data";
            var files = Directory.GetFiles(sqlPath, "*.sql").OrderBy(x => x);

            foreach (var file in files)
            {
                Logger.DebugFormat("Executing custom sql: {0}", file);
                try
                {
                    session.Connection.Execute(File.ReadAllText(file));
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("Error al ejecutar el SQL: {0}. Exc: {1}", file, ex);
                }
            }
        }

        private void CrearIndicadoresBI(ISession session)
        {
            var indicadorTurnos = new Indicador
                {
                    Categoria = CategoriaIndicadorGerencial,
                    Nombre = "Turnos Por Mes",
                    Descripcion = "Este indicador es para ver los turnos dados en el mes.",
                    Habilitado = false,
                    DenominadorSql = "SELECT 1",
                    NumeradorSql = "SELECT COUNT(*) FROM Turnos WHERE MONTH(FechaTurno) = :Mes AND YEAR(FechaTurno) = :Anio",
                    TipoOperador = TipoOperadorIndicador.Mayor
                };
            for(var i = 1; i < 12; i++)
            {
                var objetivo = new ObjetivoIndicador
                    {
                        Anio = 2012,
                        Mes = i,
                        Indicador = indicadorTurnos,
                        ValorMinimo = 10,
                        ValorMaximo = 10 + i*2
                    };
                indicadorTurnos.AgregarObjetivo(objetivo);
            }
            session.Save(indicadorTurnos);

            var ratioDeTurnosPresentadosVsAusencias = new Indicador
                {
                    Categoria = CategoriaIndicadorGerencial,
                    Nombre = "Ratio Turnos Otorgados vs Presentados",
                    Descripcion = "Este indicador sirve para ver si se estan otorgando muchos turnos pero pocos se presentan.",
                    Habilitado = false,
                    DenominadorSql =    "SELECT COUNT(*) FROM Turnos WHERE MONTH(FechaTurno) = :Mes AND YEAR(FechaTurno) = :Anio AND FechaAtencion IS NULL",
                    NumeradorSql =      "SELECT COUNT(*) FROM Turnos WHERE MONTH(FechaTurno) = :Mes AND YEAR(FechaTurno) = :Anio AND FechaAtencion IS NOT NULL",
                    TipoOperador = TipoOperadorIndicador.Mayor
                };
            for(var i = 1; i < 12; i++)
            {
                var objetivo = new ObjetivoIndicador
                    {
                        Anio = 2012,
                        Mes = i,
                        Indicador = ratioDeTurnosPresentadosVsAusencias,
                        ValorMinimo = 0.9m,
                        ValorMaximo = 1000
                    };
                ratioDeTurnosPresentadosVsAusencias.AgregarObjetivo(objetivo);
            }
            session.Save(ratioDeTurnosPresentadosVsAusencias);
        }

        private void CrearCategoriasBI(ISession session)
        {
            CategoriaIndicadorOperativo = new CategoriaIndicador { Nombre = "Operativo" };
            CategoriaIndicadorGerencial = new CategoriaIndicador { Nombre = "Gerencial" };

            session.Save(CategoriaIndicadorOperativo);
            session.Save(CategoriaIndicadorGerencial);
        }



        #region Entity Creation Methods
        private void CrearClinica(ISession session)
        {
            var clinica = new Clinica
                              {
                                  RazonSocial = "Centro Médico Integral Velez Sarsfield",
                                  DuracionTurnoPorDefecto = TimeSpan.FromMinutes(30),
                                  HorarioMatutinoDesde = new TimeSpan(8, 0, 0),
                                  HorarioMatutinoHasta = new TimeSpan(12, 0, 0),
                                  HorarioVespertinoDesde = new TimeSpan(15, 0, 0),
                                  HorarioVespertinoHasta = new TimeSpan(19, 0, 0),
                                  NumeroInasistenciasConsecutivasGeneranBloqueo = 5,
                                  Documento = new Documento
                                                  {
                                                      Numero = 30690562622,
                                                      TipoDocumento = TipoDocumento.Cuit
                                                  },
                                  Domicilio = new Domicilio
                                                  {
                                                      Direccion = "Velez Sarsfield 825",
                                                      Localidad = LocalidadRosario,
                                                      Latitud = -32.92501,
                                                      Longitud = -60.67679
                                                  },
                                  Email = "contacto@sicemed.com.ar",
                                  GoogleMapsKey = "AIzaSyBKt3eKqEI-zr5bqGJG3mCCZJK0asao5-0"
                              };
            clinica
                .AgregarTelefono(
                        new Telefono { Numero = "4382370", Prefijo = "0341" }
                ).AgregarTelefono(
                        new Telefono { Numero = "4487610", Prefijo = "0341" }
                );

            clinica.AgregarDiaHabilitado(DayOfWeek.Monday);
            clinica.AgregarDiaHabilitado(DayOfWeek.Tuesday);
            clinica.AgregarDiaHabilitado(DayOfWeek.Wednesday);
            clinica.AgregarDiaHabilitado(DayOfWeek.Thursday);
            clinica.AgregarDiaHabilitado(DayOfWeek.Friday);

            session.Save(clinica);
        }

        private void CrearPaginas(ISession session)
        {
            PaginaHome = new Pagina { Nombre = "Home", Contenido = "p<>.El Centro Médico Integral Velez Sarsfield es un centro médico de atención ambulatoria" + System.Environment.NewLine + "Establecido hace ya más de 30 años en la zona Norte de la ciudad de Rosario, brinda servicios de alta calidad en las diferentes especialidades con las que cuenta." + System.Environment.NewLine + "p=.!/public/images/theme/frente_cemi.png(CeMI)!", Url = "", Orden = 0 };
            PaginaAboutUs = new Pagina { Nombre = "About Us", Contenido = "Somos una empresa en pleno crecimiento.", Url = "AboutUs", Orden = 999 };
            var paginaPadre = new Pagina { Nombre = "Con Hijos", Contenido = "Una pagina de prueba.", Url = "Padre", Orden = 50 };
            for (var i = 0; i < 5; i++)
            {
                var hijo = new Pagina()
                               {
                                   Nombre = "Hijo " + i,
                                   Contenido = "Una pagina de prueba.",
                                   Url = "Padre/Hijo-" + i,
                                   Orden = i * 10
                               };
                if (i == 0)
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
            ObraSocialParticular = new ObraSocial
            {
                RazonSocial = "Consulta Particular",
                Documento = new Documento
                {
                    Numero = 30690562622,
                    TipoDocumento = TipoDocumento.Cuit
                },
                Telefono = new Telefono
                {
                    Prefijo = "0341",
                    Numero = "4382370"
                },
                Domicilio =
                    new Domicilio
                    {
                        Direccion = "Velez Sarsfield 825",
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
            session.Save(ObraSocialParticular);
            session.Save(ObraSocialSwissMedical);
        }

        private void CrearPlanes(ISession session)
        {
            PlanOsdeGold = new Plan { Nombre = "Gold", ObraSocial = ObraSocialOsde, Coseguro=0 };
            PlanOsdeSilver = new Plan { Nombre = "Silver", ObraSocial = ObraSocialOsde, Coseguro = 10 };
            PlanOsdeNeo = new Plan { Nombre = "Neo", ObraSocial = ObraSocialOsde, Coseguro = 30 };
            PlanParticular = new Plan { Nombre = "Consulta Particular", ObraSocial = ObraSocialParticular, Coseguro = 0 };
            PlanSwissNbsf = new Plan { Nombre = "Convenio Nuevo Banco De Santa Fe",Descripcion="Consulta Particular - NO MODIFICABLE", ObraSocial = ObraSocialSwissMedical, Coseguro = 60 };
            PlanSwissSb64 = new Plan { Nombre = "SB64", ObraSocial = ObraSocialSwissMedical, Coseguro = 35 };

            session.Save(PlanOsdeGold);
            session.Save(PlanOsdeSilver);
            session.Save(PlanOsdeNeo);
            session.Save(PlanParticular);
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
                                                Nombre = "los",
                                                Apellido = "Todos",
                                                SegundoNombre = "Roles",
                                                FechaNacimiento = new DateTime(1960, 03, 03),
                                                Domicilio =
                                                    new Domicilio { Direccion = "Mendoza 1123 7°A", Localidad = LocalidadRosario },
                                                Documento =
                                                    new Documento { Numero = 18364468, TipoDocumento = TipoDocumento.Dni },
                                                Telefono = new Telefono { Prefijo = "0341", Numero = "153353273" }
                                            };

            PersonaAdminProfesionalWalter.AgregarRol(Administrador.Create());
            PersonaAdminProfesionalWalter.AgregarRol(Paciente.Create("79887987"));
            PersonaAdminProfesionalWalter.AgregarRol(Profesional.Create("8888888"));
            PersonaAdminProfesionalWalter.AgregarRol(Secretaria.Create(DateTime.Now));
            PersonaAdminProfesionalWalter.As<Paciente>().Plan = PlanOsdeGold;

            var hInicio = new TimeSpan(10, 00, 00);
            var hfin = new TimeSpan(20, 00, 00);

            PersonaAdminProfesionalWalter.As<Profesional>().AgregarEspecialidad(EspecialidadClinico);
            PersonaAdminProfesionalWalter.As<Profesional>().AgregarAgenda(DayOfWeek.Monday, TimeSpan.FromMinutes(30), hInicio, hfin, ConsultorioA, EspecialidadClinico);
            PersonaAdminProfesionalWalter.As<Profesional>().AgregarAgenda(DayOfWeek.Tuesday, TimeSpan.FromMinutes(30), hInicio, hfin, ConsultorioA, EspecialidadClinico);
            PersonaAdminProfesionalWalter.As<Profesional>().AgregarAgenda(DayOfWeek.Wednesday, TimeSpan.FromMinutes(30), hInicio, hfin, ConsultorioA, EspecialidadClinico);
            PersonaAdminProfesionalWalter.As<Profesional>().AgregarAgenda(DayOfWeek.Thursday, TimeSpan.FromMinutes(30), hInicio, hfin, ConsultorioA, EspecialidadClinico);
            PersonaAdminProfesionalWalter.As<Profesional>().AgregarAgenda(DayOfWeek.Friday, TimeSpan.FromMinutes(30), hInicio, hfin, ConsultorioA, EspecialidadClinico);

            MembershipService.CreateUser(PersonaAdminProfesionalWalter, "Roles.Full@gmail.com", "sicemedRolesfull");
            session.Update(PersonaAdminProfesionalWalter);

            //Turno Ausente
            session.Save(Turno.Create(DateTime.Now.AddDays(-1).SetTimeWith(hInicio), TimeSpan.FromMinutes(15), PersonaAdminProfesionalWalter.As<Paciente>(),
                         PersonaAdminProfesionalWalter.As<Profesional>(), EspecialidadClinico, ConsultorioA, "127.0.0.1")
                         .MarcarAusente());

            //Turno Pendiente de Presentarse
            session.Save(Turno.Create(DateTime.Now.SetTimeWith(hInicio), TimeSpan.FromMinutes(15), PersonaAdminProfesionalWalter.As<Paciente>(),
                         PersonaAdminProfesionalWalter.As<Profesional>(), EspecialidadClinico, ConsultorioA, "127.0.0.1"));

            //Turno Presentado
            session.Save(Turno.Create(DateTime.Now.SetTimeWith(hInicio).AddMinutes(30), TimeSpan.FromMinutes(15), PersonaAdminProfesionalWalter.As<Paciente>(),
                         PersonaAdminProfesionalWalter.As<Profesional>(), EspecialidadClinico, ConsultorioA, "127.0.0.1")
                         .RegistrarIngreso(PersonaAdminProfesionalWalter.As<Secretaria>()));

            //Turno Atendido
            session.Save(Turno.Create(DateTime.Now.SetTimeWith(hInicio).AddMinutes(60), TimeSpan.FromMinutes(15), PersonaAdminProfesionalWalter.As<Paciente>(),
                         PersonaAdminProfesionalWalter.As<Profesional>(), EspecialidadClinico, ConsultorioA, "127.0.0.1")
                         .RegistrarIngreso(PersonaAdminProfesionalWalter.As<Secretaria>())
                         .RegistrarAtencion(PersonaAdminProfesionalWalter.As<Profesional>(), "El paciente se presentó con dolor de cabeza. \n Se le recetaron 2mg de Ibuprofeno cada 8hs."));

            //Turno Cancelado antes de presentarse
            session.Save(Turno.Create(DateTime.Now.SetTimeWith(hInicio).AddMinutes(90), TimeSpan.FromMinutes(15), PersonaAdminProfesionalWalter.As<Paciente>(),
                         PersonaAdminProfesionalWalter.As<Profesional>(), EspecialidadClinico, ConsultorioA, "127.0.0.1")
                         .CancelarTurno(PersonaAdminProfesionalWalter, "No me puedo presentar"));

            //Turno Cancelado luego de ingresar a la sala de espera
            session.Save(Turno.Create(DateTime.Now.SetTimeWith(hInicio).AddMinutes(120), TimeSpan.FromMinutes(15), PersonaAdminProfesionalWalter.As<Paciente>(),
                         PersonaAdminProfesionalWalter.As<Profesional>(), EspecialidadClinico, ConsultorioA, "127.0.0.1")
                         .RegistrarIngreso(PersonaAdminProfesionalWalter.As<Secretaria>())
                         .CancelarTurno(PersonaAdminProfesionalWalter, "Me tuve que ir\n Saludos."));


            PersonaPacientePablo = new Persona
                                   {
                                       Nombre = "Paciente",
                                       Apellido = "Uno",
                                       FechaNacimiento = new DateTime(1987, 03, 07),
                                       Domicilio =
                                           new Domicilio { Direccion = "San Luis 323", Localidad = LocalidadRosario },
                                       Documento = new Documento { Numero = 1234568, TipoDocumento = TipoDocumento.Dni },
                                       Telefono = new Telefono { Prefijo = "0341", Numero = "1534665" }
                                   };
            PersonaPacientePablo.AgregarRol(Paciente.Create("9798798"));
            MembershipService.CreateUser(PersonaPacientePablo, "paciente.uno@gmail.com", "sicemedPaciente");
            session.Update(PersonaPacientePablo);

            PersonaPacientePedro = new Persona
                                   {
                                       Nombre = "Paciente",
                                       Apellido = "Dos",
                                       FechaNacimiento = new DateTime(1975, 6, 15),
                                       Domicilio =
                                           new Domicilio { Direccion = "Rioja 1344", Localidad = LocalidadRosario },
                                       Documento = new Documento { Numero = 11234343, TipoDocumento = TipoDocumento.Dni },
                                       Telefono = new Telefono { Prefijo = "0341", Numero = "156333456" }
                                   };
            PersonaPacientePedro.AgregarRol(Paciente.Create("9798798"));
            MembershipService.CreateUser(PersonaPacientePedro, "paciente.dos@gmail.com", "sicemedPaciente");
            session.Update(PersonaPacientePedro);

            PersonaSecretariaJuana = new Persona
                                     {
                                         Nombre = "Secretaria",
                                         Apellido = "Matutina",
                                         FechaNacimiento = new DateTime(1990, 1, 27),
                                         Domicilio =
                                             new Domicilio { Direccion = "Ovideo Lagos 343", Localidad = LocalidadRosario },
                                         Documento =
                                             new Documento { Numero = 10112123, TipoDocumento = TipoDocumento.Dni },
                                         Telefono = new Telefono { Prefijo = "0341", Numero = "4481123" }
                                     };
            PersonaSecretariaJuana.AgregarRol(Secretaria.Create(DateTime.Now));
            MembershipService.CreateUser(PersonaSecretariaJuana, "Secretaria.Matutina@gmail.com", "sicemedSecretaria");
            session.Update(PersonaSecretariaJuana);

            PersonaProfesionalBernardoClinico = new Persona
                                         {
                                             Nombre = "Gregory",
                                             Apellido = "House",
                                             FechaNacimiento = new DateTime(1957, 6, 14),
                                             Domicilio =
                                                 new Domicilio { Direccion = "Italia 1243", Localidad = LocalidadRosario },
                                             Documento =
                                                 new Documento { Numero = 18123876, TipoDocumento = TipoDocumento.Dni },
                                             Telefono = new Telefono { Prefijo = "0341", Numero = "4471010" }
                                         };
            PersonaProfesionalBernardoClinico.AgregarRol(Profesional.Create("546546"));
            PersonaProfesionalBernardoClinico.As<Profesional>().AgregarEspecialidad(EspecialidadClinico);
            var horarioComienzo = new TimeSpan(10, 00, 00);
            var horarioFin = new TimeSpan(20, 00, 00);
            PersonaProfesionalBernardoClinico.As<Profesional>().AgregarAgenda(DayOfWeek.Monday, TimeSpan.FromMinutes(30), horarioComienzo, horarioFin, ConsultorioA, EspecialidadClinico);
            PersonaProfesionalBernardoClinico.As<Profesional>().AgregarAgenda(DayOfWeek.Wednesday, TimeSpan.FromMinutes(30), horarioComienzo, horarioFin, ConsultorioA, EspecialidadClinico);
            PersonaProfesionalBernardoClinico.As<Profesional>().AgregarAgenda(DayOfWeek.Friday, TimeSpan.FromMinutes(30), horarioComienzo, horarioFin, ConsultorioA, EspecialidadClinico);
            MembershipService.CreateUser(PersonaProfesionalBernardoClinico, "dr.house@gmail.com", "sicemedProfesional");
            session.Update(PersonaProfesionalBernardoClinico);
            session.Update(PersonaProfesionalBernardoClinico.As<Profesional>());

            PersonaProfesionalJoseClinicoYDermatologo = new Persona
                                     {
                                         Nombre = "Mark",
                                         Apellido = "Green",
                                         SegundoNombre = "",
                                         FechaNacimiento = new DateTime(1985, 7, 23),
                                         Domicilio =
                                             new Domicilio { Direccion = "Brown 123 7°a", Localidad = LocalidadRosario },
                                         Documento =
                                             new Documento { Numero = 31789502, TipoDocumento = TipoDocumento.Dni },
                                         Telefono = new Telefono { Prefijo = "0341", Numero = "1661234" }
                                     };
            PersonaProfesionalJoseClinicoYDermatologo.AgregarRol(Profesional.Create("546465489"));
            var horarioComienzo2 = new TimeSpan(11, 00, 00);
            var horarioFin2 = new TimeSpan(16, 00, 00);
            PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>().AgregarEspecialidad(EspecialidadClinico);
            PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>().AgregarAgenda(DayOfWeek.Tuesday, TimeSpan.FromMinutes(15), horarioComienzo2, horarioFin2, ConsultorioB, EspecialidadClinico);
            PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>().AgregarAgenda(DayOfWeek.Thursday, TimeSpan.FromMinutes(15), horarioComienzo2, horarioFin2, ConsultorioB, EspecialidadClinico);

            PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>().AgregarEspecialidad(EspecialidadDermatologo);
            PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>().AgregarAgenda(DayOfWeek.Monday, TimeSpan.FromMinutes(10), horarioComienzo2, horarioFin2, ConsultorioB, EspecialidadDermatologo);
            PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>().AgregarAgenda(DayOfWeek.Wednesday, TimeSpan.FromMinutes(10), horarioComienzo2, horarioFin2, ConsultorioB, EspecialidadDermatologo);
            PersonaProfesionalJoseClinicoYDermatologo.As<Profesional>().AgregarAgenda(DayOfWeek.Friday, TimeSpan.FromMinutes(10), horarioComienzo2, horarioFin2, ConsultorioB, EspecialidadDermatologo);
            MembershipService.CreateUser(PersonaProfesionalJoseClinicoYDermatologo, "dr.green@gmail.com", "sicemed.Profesional");
            session.Update(PersonaProfesionalJoseClinicoYDermatologo);
        }

        #endregion
    }
}