using System.Web.Routing;
using Combres;
using Sicemed.Web;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof (AppStart_Combres), "Start")]

namespace Sicemed.Web
{
    public static class AppStart_Combres
    {
        public static void Start()
        {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}