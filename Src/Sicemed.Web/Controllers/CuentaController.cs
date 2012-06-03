using System;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Enums;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Cuenta;

namespace Sicemed.Web.Controllers
{
    public class CuentaController : BaseController
    {
        private readonly IMembershipService _membershipService;

        public CuentaController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
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

        public ActionResult Salir()
        {
            _membershipService.SignOut();

            ShowMessages(ResponseMessage.Success("Ha cerrado su sesión."));

            return RedirectToAction("Index", "Content");
        }

        #region Registro
        public ActionResult Registro()
        {
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(RegistroPersonaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var user = new Persona {Nombre = model.Nombre, Apellido = model.Apellido};
                _membershipService.CreateUser(user, model.Email, model.Password);
                var status = _membershipService.Login(model.Email, model.Password, out user);
                if (status == MembershipStatus.USER_CREATED)
                {
                    ShowMessages(ResponseMessage.Success("Bienvenido a SICEMED {0}.", user.NombreCompleto));
                    return RedirectToAction("Index", "Content");
                }
                ModelState.AddModelError("", status.Get());
            }
            
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View(model);
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
    }
}