using System.Web;

namespace Sicemed.Web.Plumbing
{
    public class SicemedSiteMapProvider : StaticSiteMapProvider
    {
        protected override SiteMapNode GetRootNodeCore()
        {
            return new SiteMapNode(this, "Wally");
        }

        public override SiteMapNode BuildSiteMap()
        {
            return new SiteMapNode(this, "Wally2");
        }
    }
}