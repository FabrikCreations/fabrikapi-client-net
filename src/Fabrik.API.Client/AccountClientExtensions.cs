using Fabrik.API.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class AccountClientExtensions
    {
        public static async Task<Site> GetDefaultSiteAsync(this IAccountClient client)
        {
            var sites = await client.ListSitesAsync();
            return sites.FirstOrDefault();
        }
    }
}
