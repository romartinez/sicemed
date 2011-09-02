using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Web.Mvc;
using System.Web.Services.Description;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public interface ISecurityService
    {
        void Validate(Usuario user, ActionDescriptor actionDescriptor);
        bool Can(Usuario user, ActionDescriptor actionDescriptor);
        void InitializeSecurity(params Type[] types);
        void InitializeSecurity(params Assembly[] assemblies);
    }

    public class SecurityService : ISecurityService
    {
        private readonly Dictionary<string, Operation> _actions;

        public SecurityService()
        {
            _actions = new Dictionary<string, Operation>();
        }

        #region ISecurityService Members

        public virtual ConcurrentDictionary<string, Operation> Actions
        {
            get
            {
                //return a copy
                return new ConcurrentDictionary<string, Operation>(_actions);
            }
        }

        public virtual void Validate(Usuario user, ActionDescriptor actionDescriptor)
        {
            Validate(user, GetMethodName(actionDescriptor));
        }

        private void Validate(Usuario user, string methodInfo)
        {
            if (user == null) throw new SecurityException("The user is not Authenticated.");
            if (string.IsNullOrWhiteSpace(methodInfo)) throw new ArgumentNullException("methodInfo");

            if (!Can(user, methodInfo))
                throw new SecurityException(
                    string.Format("The User '{0}' doesn't have the permissions to execute '{1}'.", user.Membership.Email, methodInfo));
        }

        public virtual bool Can(Usuario user, ActionDescriptor actionDescriptor)
        {
            return Can(user, GetMethodName(actionDescriptor));
        }

        private bool Can(Usuario user, string methodInfo)
        {
            if (user == null) return false;//throw new ArgumentNullException("usuario");
            if (string.IsNullOrWhiteSpace(methodInfo)) return false; //throw new ArgumentNullException("methodInfo");

            if (!_actions.ContainsKey(methodInfo))
                return false; //throw new SecurityException("El método no tiene definidos permisos. Por defecto los métodos son seguros, la seguridad debe ser explpícita.");

            throw new NotImplementedException();
            //return user.AllowedOperations.Contains(_actions[methodInfo]);
        }

        public virtual void InitializeSecurity(params Type[] types)
        {
            if (types == null) throw new ArgumentNullException("types");

            foreach (var type in types)
            {
                var attribute =
                    type.GetCustomAttributes(typeof(AuthorizeItAttribute), false).Cast<AuthorizeItAttribute>().FirstOrDefault();
                if (attribute != default(AuthorizeItAttribute) && attribute.IsActionSpecific)
                {
                    //Usado a nivel de clase
                    var typeMethods =
                        type.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                    var accionInstance = (Operation)Activator.CreateInstance(attribute.ActionType);
                    foreach (var methodInfo in typeMethods)
                    {
                        AddMethod(methodInfo, accionInstance);
                    }
                } 
                //Usado a nivel de metodo
                var methods =
                    type.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                foreach (var methodInfo in methods)
                {
                    AddMethod(methodInfo);
                }
            }
        }

        private void AddMethod(MethodInfo methodInfo, Operation action = null)
        {
            var methodAttribute =
                methodInfo.GetCustomAttributes(typeof(AuthorizeItAttribute), false).Cast<AuthorizeItAttribute>().
                    FirstOrDefault();

            var key = GetMethodName(methodInfo);

            if (methodAttribute != default(AuthorizeItAttribute) && methodAttribute.IsActionSpecific)
            {
                var accionInstance = (Operation)Activator.CreateInstance(methodAttribute.ActionType);
                if (_actions.ContainsKey(key)) _actions.Remove(key);

                _actions.Add(key, accionInstance);
            }
            else if(action != null)
            {
                _actions.Add(key, action);                
            }
        }

        public virtual void InitializeSecurity(params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException("assemblies");

            foreach (var assembly in assemblies)
            {
                InitializeSecurity(assembly.GetTypes());
            }
        }

        #endregion

        protected virtual string GetMethodName(ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null) throw new ArgumentNullException("actionDescriptor");

            return actionDescriptor.ControllerDescriptor.ControllerType.FullName + "."
                   + actionDescriptor.ActionName + "_" + string.Join("|", actionDescriptor.GetParameters().Select(p=>p.ParameterType + ":" + p.ParameterName));
        }

        protected virtual string GetMethodName(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException("method");

            return method.ReflectedType.FullName + "." + method.Name + "_" + string.Join("|", method.GetParameters().Select(p => p.ParameterType + ":" + p.Name));
        }
    }
}