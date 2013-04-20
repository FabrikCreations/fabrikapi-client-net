using Fabrik.CMS.API.Common;
using Fabrik.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class MenusClientExtensions
    {
        public static async Task<Menu> GetMenuByName(this IMenusClient client, int siteId, string name)
        {
            Ensure.Argument.NotNullOrEmpty(name, "name");
            var menus = await client.GetMenusAsync(siteId: siteId, name: name).ConfigureAwait(false);
            return menus.FirstOrDefault();
        }
    }
}
