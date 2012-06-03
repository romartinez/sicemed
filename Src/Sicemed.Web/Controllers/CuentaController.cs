using System;
using System.Web.Mvc;
using AutoMapper;
using Sicemed.Web.Areas.Admin.Models.Clinicas;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Enums;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels.Cuenta;

namespace Sicemed.Web.Controllers
{
    public class CuentaController : NHibernateController
    {
        private readonly IMembershipService _membershipService;

        public CuentaController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public ActionResult Salir()
        {
            _membershipService.SignOut();

            ShowMessages(ResponseMessage.Success("Ha cerrado su sesión."));

            return RedirectToAction("Index", "Content");
        }

        #region Iniciar Sesion
        public ActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IniciarSesion(InciarSesionViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Persona user;
                var status = _membershipService.Login(model.Email, model.Password, out user);
                if (status == MembershipStatus.USER_FOUND)
                {
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    ShowMessages(ResponseMessage.Success("Ha iniciado sesión como: {0}", user.NombreCompleto));

                    return RedirectToAction("Index", "Content");
                }

                ModelState.AddModelError("", status.Get());
            }

            return View(model);
        }

        #endregion

        #region Registro
        public ActionResult Registro()
        {
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            var viewModel = new RegistroPersonaViewModel();
            AppendLists(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(RegistroPersonaViewModel viewModel)
        {
            AppendLists(viewModel);

            if (ModelState.IsValid)
            {
                var session = SessionFactory.GetCurrentSession();
                // Attempt to register the user                
                var model = Mapper.Map<Persona>(viewModel);
                //Update not automapped properties
                model.Documento = new Documento
                                      {
                                          Numero = viewModel.DocumentoNumero,
                                          TipoDocumento = Enumeration.FromValue<TipoDocumento>(viewModel.DocumentoTipoDocumentoValue)
                                      };
                model.Domicilio = new Domicilio
                                      {
                                          Direccion = viewModel.DomicilioDireccion,
                                          Localidad =  session.Load<Localidad>(viewModel.DomicilioLocalidadId)
                                      };
                //TODO: Falta meterle las obras sociales y los planes para poder hacer esto
                //User.AgregarRol(Paciente.Create(viewModel.NumeroAfiliado));
                var status = _membershipService.CreateUser(model, viewModel.Email, viewModel.Password);                
                if (status == MembershipStatus.USER_CREATED)
                {
                    _membershipService.Login(viewModel.Email, viewModel.Password, out model);
                    ShowMessages(ResponseMessage.Success("Bienvenido a SICEMED {0}.", model.NombreCompleto));
                    return RedirectToAction("Index", "Content");
                }
                ModelState.AddModelError("", status.Get());
            }

            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View(viewModel);
        }
        #endregion

        #region CambiarPassword

        public ActionResult CambiarPassword()
        {
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarPassword(CambiarPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var status = _membershipService.ChangePassword(User.Identity.Name, viewModel.PasswordActual,
                                                               viewModel.PasswordNuevo);
                if (status == MembershipStatus.USER_FOUND)
                {
                    ShowMessages(ResponseMessage.Success("Se ha cambiado con éxito su password."));
                    return RedirectToAction("Index", "Content");
                }
                ModelState.AddModelError("", status.Get());
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View(viewModel);
        }
        #endregion

        #region Olvide Password
        public ActionResult RecuperarPassword()
        {
            //TODO:
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarPassword(string pass)
        {
            //TODO:
            throw new NotImplementedException();
        }
        #endregion

        private void AppendLists(RegistroPersonaViewModel viewModel)
        {
            viewModel.TiposDocumentosHabilitados = DomainExtensions.GetTiposDocumentos(viewModel.DocumentoTipoDocumentoValue);
            viewModel.ProvinciasHabilitadas = DomainExtensions.GetProvincias(SessionFactory, viewModel.DomicilioLocalidadProvinciaId);

            if (viewModel.DomicilioLocalidadProvinciaId.HasValue)
            {
                viewModel.LocalidadesHabilitadas =
                    DomainExtensions.GetLocalidades(SessionFactory, viewModel.DomicilioLocalidadProvinciaId.Value, viewModel.DomicilioLocalidadId);
            }
        }
    }
}