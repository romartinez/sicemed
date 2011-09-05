using System;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.ViewModels.UserManagement;

namespace Sicemed.Web.Controllers
{
    public class UserManagementController : NHibernateController
    {
        public ActionResult Index()
        {
            var session = SessionFactory.GetCurrentSession();            
            return View(session.QueryOver<Usuario>().List());
        }
        public ActionResult Create()
        {
            var vm = new RegisterUserViewModel();
            FillRoles(vm);
            return View(vm);
        }

        private void FillRoles(RegisterUserViewModel vm) {
            var session = SessionFactory.GetCurrentSession();
            var roles = session.QueryOver<Rol>().Future();
            vm.AllRoles = roles.ToList();
        }

        [HttpPost]
        public ActionResult Create(RegisterUserViewModel model)
        {
            FillRoles(model);
            try
            {
                if(ModelState.IsValid)
                {
                    var user = new Usuario()
                               {
                                   Nombre = model.FullName
                               };

                    foreach(var selectedRole in model.SelectedRoles)
                    {
                        var role = model.AllRoles.Single(r => r.Value == selectedRole);
                        user.AgregarRol(role);
                    }

                    MembershipService.CreateUser(user, model.Email, model.Password);
                    
                    return RedirectToAction("Index");   
                }
                return View(model);
            }
            catch(Exception ex)
            {
                Logger.Fatal("There was an error creating the user.", ex);
                ModelState.AddModelError("", "There was an error creating the user.");
                return View(model);
            }
        }

        public ActionResult Edit(long id)
        {
            var session = SessionFactory.GetCurrentSession();
            var user = session.Get<Usuario>(id);
            var vm = new RegisterUserViewModel(user);
            FillRoles(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(long id, RegisterUserViewModel model)
        {
            FillRoles(model);
            try
            {
                if (ModelState.IsValidField(ReflectionHelper.GetProperty<RegisterUserViewModel>(x=>x.FullName).Name))
                {
                    var session = SessionFactory.GetCurrentSession();
                    var user = session.Get<Usuario>(id);
                    user.Nombre = model.FullName;
                    
                    //Roles
                    var rolesToDelete = user.Roles.Where(r => !model.SelectedRoles.Contains(r.Rol.Value)).ToList();
                    foreach(var roleToDelete in rolesToDelete)
                    {
                        user.QuitarRol(roleToDelete.Rol);
                    }

                    var rolesIdToAdd = model.SelectedRoles.Where(r => !user.Roles.Any(role => role.Rol.Value == r)).ToList();
                    foreach (var roleIdToAdd in rolesIdToAdd)
                    {
                        var roleToAdd = model.AllRoles.Single(r => r.Value == roleIdToAdd);
                        user.AgregarRol(roleToAdd);
                    }

                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch(Exception ex)
            {
                Logger.Fatal("There was an error modifying the user.", ex);
                ModelState.AddModelError("", "There was an error modifying the user.");
                return View(model);
            }
        }

        //
        // GET: /Admin/UserManagement/Delete/5

        public ActionResult Delete(long id)
        {
            var session = SessionFactory.GetCurrentSession();
            session.Delete<Usuario>(id);
            return View();
        }
    }
}
