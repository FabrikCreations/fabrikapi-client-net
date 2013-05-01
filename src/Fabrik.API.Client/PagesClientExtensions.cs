using Fabrik.CMS.API.Common;
using Fabrik.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class PagesClientExtensions
    {
        public static async Task<Page> GetPageBySlugAsync(this IPagesClient client, int siteId, string slug)
        {
            Ensure.Argument.NotNullOrEmpty(slug, "slug");
            var pages = await client.GetPagesAsync(siteId, slug: slug).ConfigureAwait(false);
            return pages.Items.FirstOrDefault();
        }
    }
}
