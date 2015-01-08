using Fabrik.API.Core;
using Fabrik.Common;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class PortfolioClientExtensions
    {
        public static async Task<TaggedResult<Project>> ListProjectsByTagAsync(this IPortfolioClient client, string tag, int? pageSize = null, int? page = null)
        {
            Ensure.Argument.NotNullOrEmpty(tag, "tag");
            var taggedProjects = await client.ListProjectsAsync(pageSize, page, tags: new[] { tag }).ConfigureAwait(false);
            return TaggedResult<Project>.Create(tag, taggedProjects);
        }
    }
}
