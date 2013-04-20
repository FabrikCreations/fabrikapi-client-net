using Fabrik.CMS.API.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class SiteClientExtensions
    {
        public static async Task<Site> GetDefaultSite(this ISiteClient client)
        {
            var sites = await client.GetSitesAsync();
            return sites.FirstOrDefault();
        }
    }
}
