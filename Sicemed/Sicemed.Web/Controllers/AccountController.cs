using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels;
using Sicemed.Web.Models.ViewModels.Cuenta;
using Sicemed.Web.Plumbing;
using Sicemed.Web.Plumbing.Helpers;
using Sicemed.Web.Services.ApplicationServices.Cuenta;

namespace Sicemed.Web.Controllers
{
    public class AccountController : BaseController
    {
        public IFormsAuthenticationApplicationService FormsService { get; set; }
        public IMembershipApplicationService MembershipApplicationService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null)
            {
                FormsService = new FormsAuthenticationApplicationService();
            }
            if (MembershipApplicationService == null)
            {
                MembershipApplicationService = new AccountMembershipApplicationService();
            }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipApplicationService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    } else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                } else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewBag.PasswordLength = MembershipApplicationService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipApplicationService.CreateUser(model.UserName, model.Password,
                                                                                   model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                } else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipApplicationService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipApplicationService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(CambiarPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (MembershipApplicationService.ChangePassword(User.Identity.Name, viewModel.OldPassword, viewModel.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                } else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipApplicationService.MinPasswordLength;
            return View(viewModel);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}