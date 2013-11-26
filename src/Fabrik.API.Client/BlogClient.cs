using Fabrik.API.Client.Core;
using Fabrik.API.Core;
using Fabrik.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class BlogClient : IBlogClient
    {
        private readonly ApiClient api;
        
        public BlogClient(ApiClient apiClient)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            api = apiClient;
        }

        public Task<PagedResult<Post>> GetPostsAsync(int siteId, int? pageSize = null, int? page = null, string slug = null, IEnumerable<string> tags = null, string term = null,
            bool? includeFuturePosts = null, bool? includeUnpublishedPosts = null)
        {
            var tagString = tags.JoinOrDefault(";");

            return api.GetAsync<PagedResult<Post>>(GetPostsPath(siteId),
                new { pageSize = pageSize, page = page, slug = slug, tags = tagString, term = term, futureposts = includeFuturePosts, unpublished = includeUnpublishedPosts });
        }

        public Task<Post> GetPostAsync(int siteId, int postId)
        {
            return api.GetAsync<Post>(GetPostsPath(siteId, postId));
        }

        public Task<Post> AddPostAsync(int siteId, AddPostCommand command)
        {
            return api.PostAsync<AddPostCommand, Post>(GetPostsPath(siteId), command);
        }

        public async Task UpdatePostAsync(int siteId, int postId, UpdatePostCommand command)
        {
            await api.PutAsync(GetPostsPath(siteId, postId), command);
        }

        public async Task DeletePostAsync(int siteId, int postId)
        {
            await api.DeleteAsync(GetPostsPath(siteId, postId));
        }

        public Task<MediaItem> AddPostMediaAsync(int siteId, int postId, AddMediaCommand command)
        {
            return api.PostAsync<AddMediaCommand, MediaItem>(GetPostMediaPath(siteId, postId), command);
        }

        public async Task UpdatePostMediaAsync(int siteId, int postId, int mediaItemId, UpdateMediaCommand command)
        {
            await api.PutAsync(GetPostMediaPath(siteId, postId, mediaItemId), command);
        }

        public async Task PatchPostMediaAsync(int siteId, int postId, PatchMediaCommand command)
        {
            await api.PatchAsync(GetPostMediaPath(siteId, postId), command);
        }

        public async Task DeletePostMediaAsync(int siteId, int postId, int mediaItemId)
        {
            await api.DeleteAsync(GetPostMediaPath(siteId, postId, mediaItemId));
        }

        public Task<PagedResult<PostArchive>> GetArchivesAsync(int siteId, int? pageSize = null, int? page = null)
        {
            return api.GetAsync<PagedResult<PostArchive>>(GetArchivesPath(siteId), new { pageSize = pageSize, page = page });
        }

        public Task<PagedResult<Post>> GetArchivePostsAsync(int siteId, int year, int month, int? pageSize = null, int? page = null)
        {
            return api.GetAsync<PagedResult<Post>>(GetArchivesPath(siteId, year, month), new { pageSize = pageSize, page = page });
        }

        public Task<PagedResult<PostTagSummary>> GetTagsAsync(int siteId, string term = null, int? pageSize = null, int? page = null)
        {
            return api.GetAsync<PagedResult<PostTagSummary>>(GetPostTagsPath(siteId), new { term = term, pageSize = pageSize, page = page });
        }

        public async Task<TaggedResult<Post>> GetPostsByTagAsync(int siteId, string tag, int? pageSize = null, int? page = null)
        {
            Ensure.Argument.NotNullOrEmpty(tag, "tag");
            var taggedPosts = await GetPostsAsync(siteId, pageSize, page, tags: new[] { tag }).ConfigureAwait(false);
            return TaggedResult<Post>.Create(tag, taggedPosts);
        }

        private string GetBlogPath(int siteId)
        {
            return "sites/{0}/blog".FormatWith(siteId);
        }

        private string GetPostsPath(int siteId, int? postId = null)
        {
            var postsPath = "{0}/posts".FormatWith(GetBlogPath(siteId));

            if (postId.HasValue)
                postsPath += "/" + postId;

            return postsPath;
        }

        private string GetPostMediaPath(int siteId, int postId, int? mediaItemId = null)
        {
            var postMediaPath = "{0}/media".FormatWith(GetPostsPath(siteId, postId));

            if (mediaItemId.HasValue)
                postMediaPath += "/" + mediaItemId;

            return postMediaPath;
        }

        private string GetArchivesPath(int siteId, int? year = null, int? month = null)
        {
            var archivesPath = "{0}/archives".FormatWith(GetBlogPath(siteId));

            if (year.HasValue && month.HasValue)
                archivesPath += "/{1}/{2}".FormatWith(year.Value, month.Value);

            return archivesPath;
        }

        private string GetPostTagsPath(int siteId)
        {
            return "{0}/tags".FormatWith(GetBlogPath(siteId));
        }
    }
}
