using System.Web.Mvc;
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

        public ActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
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
                    return RedirectToAction("Index", "Content");
                }

                ModelState.AddModelError("", status.Get());
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Salir()
        {
            _membershipService.SignOut();

            return RedirectToAction("Index", "Content");
        }

        public ActionResult Registro()
        {
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Registro(RegistroPersonaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var user = new Persona { Nombre = model.Nombre, Apellido = model.Apellido };
                _membershipService.CreateUser(user, model.Email, model.Password);
                var status = _membershipService.Login(model.Email, model.Password, out user);
                if (status == MembershipStatus.USER_CREATED)
                    return RedirectToAction("Index", "Content");
                ModelState.AddModelError("", status.Get());
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(CambiarPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var status = _membershipService.ChangePassword(User.Identity.Name, viewModel.PasswordActual, viewModel.PasswordNuevo);
                if (status == MembershipStatus.USER_FOUND)
                    return RedirectToAction("CambioDePasswordExitoso");
                ModelState.AddModelError("", status.Get());
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View(viewModel);
        }

        public ActionResult CambioDePasswordExitoso()
        {
            return View();
        }
    }
}