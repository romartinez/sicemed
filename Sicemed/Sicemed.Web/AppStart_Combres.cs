[assembly: WebActivator.PreApplicationStartMethod(typeof(Sicemed.Web.AppStart_Combres), "Start")]
namespace Sicemed.Web {
	using System.Web.Routing;
	using Combres;
	
    public static class AppStart_Combres {
        public static void Start() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}