using System;

namespace Sicemed.Web.Plumbing
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DoNotMapAttribute : Attribute {}
}