using Fabrik.API.Common;
using Fabrik.API.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IBlogClient
    {
        // Posts
        
        Task<PagedResult<Post>> GetPostsAsync(int siteId, int? pageSize = null, int? page = null, string slug = null, IEnumerable<string> tags = null, string term = null, 
            bool? includeFuturePosts = null, bool? includeUnpublishedPosts = null);
        Task<Post> GetPostAsync(int siteId, int postId);
        Task<Post> AddPostAsync(int siteId, AddPostCommand command);
        Task UpdatePostAsync(int siteId, int postId, UpdatePostCommand command);
        Task DeletePostAsync(int siteId, int postId);

        // Post Media

        Task<MediaItem> AddPostMediaAsync(int siteId, int postId, AddMediaCommand command);
        Task UpdatePostMediaAsync(int siteId, int postId, int mediaItemId, UpdateMediaCommand command);
        Task PatchPostMediaAsync(int siteId, int postId, PatchMediaCommand command);
        Task DeletePostMediaAsync(int siteId, int postId, int mediaItemId);

        // Post Archives

        Task<PagedResult<PostArchive>> GetArchivesAsync(int siteId, int? pageSize = null, int? page = null);
        Task<PagedResult<Post>> GetArchivePostsAsync(int siteId, int year, int month, int? pageSize = null, int? page = null);

        // Post Tags

        Task<PagedResult<PostTagSummary>> GetTagsAsync(int siteId, string term = null, int? pageSize = null, int? page = null);
        Task<TaggedResult<Post>> GetPostsByTagAsync(int siteId, string tag, int? pageSize = null, int? page = null);
    }
}
