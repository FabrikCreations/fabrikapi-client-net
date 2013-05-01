using Fabrik.API.Common;
using Fabrik.CMS.API.Common;
using Fabrik.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class PortfolioClientExtensions
    {
        public static async Task<Project> GetProjectBySlugAsync(this IPortfolioClient client, int siteId, string slug)
        {
            Ensure.Argument.NotNull(client, "client");
            Ensure.Argument.NotNullOrEmpty(slug, "slug");
            var projects = await client.GetProjectsAsync(siteId, slug: slug).ConfigureAwait(false);
            return projects.Items.FirstOrDefault();
        }

        public static Task<PagedResult<Project>> GetProjectsByCategoryAsync(this IPortfolioClient client, int siteId, int categoryId, IEnumerable<string> tags = null, int? pageSize = null, int? page = null)
        {
            Ensure.Argument.NotNull(client, "client");
            return client.GetProjectsAsync(siteId, categoryId: categoryId, tags: tags, pageSize: pageSize, page: page);
        }

        public static Task<PagedResult<Project>> GetProjectsByCategoryAsync(this IPortfolioClient client, int siteId, string categorySlug, IEnumerable<string> tags = null, int? pageSize = null, int? page = null)
        {
            Ensure.Argument.NotNull(client, "client");
            Ensure.Argument.NotNull(categorySlug);

            return client.GetProjectsAsync(
                siteId: siteId,
                categorySlug: categorySlug,
                tags: tags,
                page: page,
                pageSize: pageSize);
        }

        public static async Task<PortfolioCategory> GetCategoryBySlugAsync(this IPortfolioClient client, int siteId, string slug)
        {
            Ensure.Argument.NotNull(client, "client");
            Ensure.Argument.NotNull(slug, "slug");
            var categories = await client.GetCategoriesAsync(siteId, slug: slug).ConfigureAwait(false);
            return categories.Items.FirstOrDefault();
        }
    }
}
