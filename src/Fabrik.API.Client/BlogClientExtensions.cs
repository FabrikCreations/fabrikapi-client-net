using Fabrik.API.Core;
using Fabrik.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class BlogClientExtensions
    {
        public static async Task<Post> GetPostBySlugAsync(this IBlogClient client, string slug)
        {
            Ensure.Argument.NotNullOrEmpty(slug, "slug");
            var posts = await client.ListPostsAsync(slug: slug).ConfigureAwait(false);
            return posts.Items.FirstOrDefault();
        }

        public static async Task<TaggedResult<Post>> ListPostsByTagAsync(this IBlogClient client, string tag, int? pageSize = null, int? page = null)
        {
            Ensure.Argument.NotNullOrEmpty(tag, "tag");
            var taggedPosts = await client.ListPostsAsync(pageSize, page, tags: new[] { tag }).ConfigureAwait(false);
            return TaggedResult<Post>.Create(tag, taggedPosts);
        }
    }
}
