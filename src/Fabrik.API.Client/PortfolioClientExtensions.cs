using Fabrik.API.Core;
using Fabrik.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class PortfolioClientExtensions
    {
        public static async Task<Project> GetProjectBySlugAsync(this IPortfolioClient client, string slug)
        {
            Ensure.Argument.NotNullOrEmpty(slug, "slug");
            var projects = await client.ListProjectsAsync(slug: slug).ConfigureAwait(false);
            return projects.Items.FirstOrDefault();
        }

        public static Task<PagedResult<Project>> ListProjectsByCategoryAsync(this IPortfolioClient client, int categoryId, IEnumerable<string> tags = null, int? pageSize = null, int? page = null)
        {
            return client.ListProjectsAsync(categoryId: categoryId, tags: tags, pageSize: pageSize, page: page);
        }

        public static Task<PagedResult<Project>> ListProjectsByCategoryAsync(this IPortfolioClient client, string categorySlug, IEnumerable<string> tags = null, int? pageSize = null, int? page = null)
        {
            Ensure.Argument.NotNull(categorySlug);

            return client.ListProjectsAsync(
                categorySlug: categorySlug,
                tags: tags,
                page: page,
                pageSize: pageSize);
        }

        public static async Task<PortfolioCategory> GetCategoryBySlugAsync(this IPortfolioClient client, string slug)
        {
            Ensure.Argument.NotNull(slug, "slug");
            var categories = await client.ListCategoriesAsync(slug: slug).ConfigureAwait(false);
            return categories.Items.FirstOrDefault();
        }

        public static async Task<TaggedResult<Project>> ListProjectsByTagAsync(this IPortfolioClient client, string tag, int? pageSize = null, int? page = null)
        {
            Ensure.Argument.NotNullOrEmpty(tag, "tag");
            var taggedProjects = await client.ListProjectsAsync(pageSize, page, tags: new[] { tag }).ConfigureAwait(false);
            return TaggedResult<Project>.Create(tag, taggedProjects);
        }
    }
}
