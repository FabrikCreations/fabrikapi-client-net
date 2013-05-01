using Fabrik.CMS.API.Common;
using Fabrik.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class BlogClientExtensions
    {
        public static async Task<Post> GetPostBySlugAsync(this IBlogClient client, int siteId, string slug)
        {
            Ensure.Argument.NotNullOrEmpty(slug, "slug");
            var posts = await client.GetPostsAsync(siteId, slug: slug).ConfigureAwait(false);
            return posts.Items.FirstOrDefault();
        }
    }
}
