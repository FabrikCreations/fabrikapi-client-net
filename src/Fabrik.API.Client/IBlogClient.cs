using Fabrik.API.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IBlogClient
    {
        // Posts
        
        Task<PagedResult<Post>> ListPostsAsync(int? pageSize = null, int? page = null, string slug = null, IEnumerable<string> tags = null, string term = null, 
            bool? includeFuturePosts = null, bool? includeUnpublishedPosts = null);
        Task<Post> GetPostAsync(int postId);
        Task<Post> AddPostAsync(AddPostCommand command);
        Task UpdatePostAsync(int postId, UpdatePostCommand command);
        Task DeletePostAsync(int postId);

        // Post Media

        Task<MediaItem> AddPostMediaAsync(int postId, AddMediaCommand command);
        Task UpdatePostMediaAsync(int postId, int mediaItemId, UpdateMediaCommand command);
        Task PatchPostMediaAsync(int postId, PatchMediaCommand command);
        Task DeletePostMediaAsync(int postId, int mediaItemId);

        // Post Archives

        Task<PagedResult<PostArchive>> ListArchivesAsync(int? pageSize = null, int? page = null);
        Task<PagedResult<Post>> GetArchivePostsAsync(int year, int month, int? pageSize = null, int? page = null);

        // Post Tags

        Task<PagedResult<PostTagSummary>> ListTagsAsync(string term = null, int? pageSize = null, int? page = null);
    }
}
