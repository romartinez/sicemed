using System.Web.Mvc;

namespace Sicemed.Web.Areas.Public
{
    public class PublicAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Public";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Public_As_default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Inicio", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Public_default",
                "Public/{controller}/{action}/{id}",
                new { controller= "Home", action = "Inicio", id = UrlParameter.Optional }
            );
        }
    }
}
