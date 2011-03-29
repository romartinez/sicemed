using System.Web.Mvc;
using System.Web.Security;
using Resources;
using Sicemed.Web.Areas.Public.Models.Cuenta;
using Sicemed.Web.Plumbing;
using Sicemed.Web.Plumbing.Helpers;
using Sicemed.Web.Services.ApplicationServices.Cuenta;

namespace Sicemed.Web.Areas.Public.Controllers
{
    public partial class UsuariosController : BaseController
    {
        private readonly IFormsAuthenticationApplicationService _formsApplicationService;
        private readonly IMembershipApplicationService _membershipApplicationService;
        
        public UsuariosController(IFormsAuthenticationApplicationService formsApplicationService, IMembershipApplicationService membershipApplicationService)
        {
            _formsApplicationService = formsApplicationService;
            _membershipApplicationService = membershipApplicationService;
        }

        public virtual ActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult IniciarSesion(IniciarSesionViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_membershipApplicationService.ValidarUsuario(model.Username, model.Password))
                {
                    _formsApplicationService.IniciarSesion(model.Username, model.Recordarme);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    } else
                    {
                        return RedirectToAction(MVC.Public.Home.Inicio());
                    }
                } else
                {
                    ModelState.AddModelError(string.Empty, Recursos.USUARIOS_USUARIO_O_PASSWORD_INCORRECTO);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual ActionResult CerrarSesion()
        {
            _formsApplicationService.CerrarSesion();

            return RedirectToAction(MVC.Public.Home.Inicio());
        }


        public virtual ActionResult Registrar()
        {
            ViewBag.PasswordLength = _membershipApplicationService.LargoMinimoPassword;
            return View();
        }

        [HttpPost]
        public virtual ActionResult Registrar(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = _membershipApplicationService.CrearUsuario(model.Username, model.Password,
                                                                                   model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    _formsApplicationService.IniciarSesion(model.Username, false /* createPersistentCookie */);
                    return RedirectToAction(MVC.Public.Home.Inicio());
                } else
                {
                    ModelState.AddModelError(string.Empty, AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipApplicationService.LargoMinimoPassword;
            return View(model);
        }

        [Authorize]
        public virtual ActionResult CambiarPassword()
        {
            ViewBag.PasswordLength = _membershipApplicationService.LargoMinimoPassword;
            return View();
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult CambiarPassword(CambiarPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (_membershipApplicationService.CambiarPassword(User.Identity.Name, viewModel.PasswordViejo, viewModel.PasswordNuevo))
                {
                    return RedirectToAction(this.CambioPasswordExito());
                } else
                {
                    ModelState.AddModelError(string.Empty, Recursos.USUARIOS_PASSWORD_INVALIDO_O_NUEVO_INCORRECTO);
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipApplicationService.LargoMinimoPassword;
            return View(viewModel);
        }

        public virtual ActionResult CambioPasswordExito()
        {
            return View();
        }
    }
}