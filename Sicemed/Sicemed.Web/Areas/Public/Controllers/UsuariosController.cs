﻿using System.Web.Mvc;
using System.Web.Security;
using Resources;
using Sicemed.Web.Areas.Public.Models.Cuenta;
using Sicemed.Web.Plumbing;
using Sicemed.Web.Plumbing.Helpers;
using Sicemed.Web.Services.ApplicationServices.Cuenta;

namespace Sicemed.Web.Areas.Public.Controllers
{
    public class UsuariosController : BaseController
    {
        private readonly IFormsAuthenticationApplicationService _formsApplicationService;
        private readonly IMembershipApplicationService _membershipApplicationService;
        
        public UsuariosController(IFormsAuthenticationApplicationService formsApplicationService, IMembershipApplicationService membershipApplicationService)
        {
            _formsApplicationService = formsApplicationService;
            _membershipApplicationService = membershipApplicationService;
        }

        public ActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(IniciarSesionViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_membershipApplicationService.ValidateUser(model.Username, model.Password))
                {
                    _formsApplicationService.SignIn(model.Username, model.Recordarme);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    } else
                    {
                        return RedirectToAction("Inicio", "Home");
                    }
                } else
                {
                    ModelState.AddModelError(string.Empty, Recursos.USUARIOS_USUARIO_O_PASSWORD_INCORRECTO);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult CerrarSesion()
        {
            _formsApplicationService.SignOut();

            return RedirectToAction("Inicio", "Home");
        }


        public ActionResult Registrar()
        {
            ViewBag.PasswordLength = _membershipApplicationService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = _membershipApplicationService.CreateUser(model.Username, model.Password,
                                                                                   model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    _formsApplicationService.SignIn(model.Username, false /* createPersistentCookie */);
                    return RedirectToAction("Inicio", "Home");
                } else
                {
                    ModelState.AddModelError(string.Empty, AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipApplicationService.MinPasswordLength;
            return View(model);
        }

        [Authorize]
        public ActionResult CambiarPassword()
        {
            ViewBag.PasswordLength = _membershipApplicationService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CambiarPassword(CambiarPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (_membershipApplicationService.ChangePassword(User.Identity.Name, viewModel.PasswordViejo, viewModel.PasswordNuevo))
                {
                    return RedirectToAction("CambioPasswordExito");
                } else
                {
                    ModelState.AddModelError(string.Empty, Recursos.USUARIOS_PASSWORD_INVALIDO_O_NUEVO_INCORRECTO);
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipApplicationService.MinPasswordLength;
            return View(viewModel);
        }

        public ActionResult CambioPasswordExito()
        {
            return View();
        }
    }
}