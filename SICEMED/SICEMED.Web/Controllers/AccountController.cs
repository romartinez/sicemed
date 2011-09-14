using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Enums;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Account;

namespace Sicemed.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IMembershipService _membershipService;

        public AccountController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public void Index()
        {

        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                Usuario user;
                var status = _membershipService.Login(model.Email, model.Password, out user);
                if(status == MembershipStatus.USER_FOUND)
                {
                    if(Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("Index", "Content");
                }
                
                ModelState.AddModelError("", status.Get());
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************
        public ActionResult LogOff()
        {
            _membershipService.SignOut();

            return RedirectToAction("Index", "Content");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************
        public ActionResult Register()
        {
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                // Attempt to register the user
                var user = new Usuario { Nombre = model.UserName };
                _membershipService.CreateUser(user, model.Email, model.Password);
                var status = _membershipService.Login(model.Email, model.Password, out user);
                if(status == MembershipStatus.USER_CREATED)
                    return RedirectToAction("Index", "Content");
                ModelState.AddModelError("", status.Get());
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if(ModelState.IsValid)
            {                
                var status = _membershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                if(status == MembershipStatus.USER_FOUND)
                    return RedirectToAction("ChangePasswordSuccess");
                ModelState.AddModelError("", status.Get());
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View(model);
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