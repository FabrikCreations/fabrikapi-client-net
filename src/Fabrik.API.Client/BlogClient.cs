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
        private readonly int siteId;
        
        public BlogClient(ApiClient apiClient, int siteId)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            this.api = apiClient;
            this.siteId = siteId;
        }

        public Task<PagedResult<Post>> GetPostsAsync(int? pageSize = null, int? page = null, string slug = null, IEnumerable<string> tags = null, string term = null,
            bool? includeFuturePosts = null, bool? includeUnpublishedPosts = null)
        {
            var tagString = tags.JoinOrDefault(";");

            return api.GetAsync<PagedResult<Post>>(GetPostsPath(),
                new { pageSize = pageSize, page = page, slug = slug, tags = tagString, term = term, futureposts = includeFuturePosts, unpublished = includeUnpublishedPosts });
        }

        public Task<Post> GetPostAsync(int postId)
        {
            return api.GetAsync<Post>(GetPostsPath(postId));
        }

        public Task<Post> AddPostAsync(AddPostCommand command)
        {
            return api.PostAsync<AddPostCommand, Post>(GetPostsPath(), command);
        }

        public async Task UpdatePostAsync(int postId, UpdatePostCommand command)
        {
            await api.PutAsync(GetPostsPath(postId), command);
        }

        public async Task DeletePostAsync(int postId)
        {
            await api.DeleteAsync(GetPostsPath(postId));
        }

        public Task<MediaItem> AddPostMediaAsync(int postId, AddMediaCommand command)
        {
            return api.PostAsync<AddMediaCommand, MediaItem>(GetPostMediaPath(postId), command);
        }

        public async Task UpdatePostMediaAsync(int postId, int mediaItemId, UpdateMediaCommand command)
        {
            await api.PutAsync(GetPostMediaPath(postId, mediaItemId), command);
        }

        public async Task PatchPostMediaAsync(int postId, PatchMediaCommand command)
        {
            await api.PatchAsync(GetPostMediaPath(postId), command);
        }

        public async Task DeletePostMediaAsync(int postId, int mediaItemId)
        {
            await api.DeleteAsync(GetPostMediaPath(postId, mediaItemId));
        }

        public Task<PagedResult<PostArchive>> GetArchivesAsync(int? pageSize = null, int? page = null)
        {
            return api.GetAsync<PagedResult<PostArchive>>(GetArchivesPath(), new { pageSize = pageSize, page = page });
        }

        public Task<PagedResult<Post>> GetArchivePostsAsync(int year, int month, int? pageSize = null, int? page = null)
        {
            return api.GetAsync<PagedResult<Post>>(GetArchivesPath(year, month), new { pageSize = pageSize, page = page });
        }

        public Task<PagedResult<PostTagSummary>> GetTagsAsync(string term = null, int? pageSize = null, int? page = null)
        {
            return api.GetAsync<PagedResult<PostTagSummary>>(GetPostTagsPath(), new { term = term, pageSize = pageSize, page = page });
        }

        public async Task<TaggedResult<Post>> GetPostsByTagAsync(string tag, int? pageSize = null, int? page = null)
        {
            Ensure.Argument.NotNullOrEmpty(tag, "tag");
            var taggedPosts = await GetPostsAsync(pageSize, page, tags: new[] { tag }).ConfigureAwait(false);
            return TaggedResult<Post>.Create(tag, taggedPosts);
        }

        private string GetBlogPath()
        {
            return "sites/{0}/blog".FormatWith(siteId);
        }

        private string GetPostsPath(int? postId = null)
        {
            var postsPath = "{0}/posts".FormatWith(GetBlogPath());

            if (postId.HasValue)
                postsPath += "/" + postId;

            return postsPath;
        }

        private string GetPostMediaPath(int postId, int? mediaItemId = null)
        {
            var postMediaPath = "{0}/media".FormatWith(GetPostsPath(postId));

            if (mediaItemId.HasValue)
                postMediaPath += "/" + mediaItemId;

            return postMediaPath;
        }

        private string GetArchivesPath(int? year = null, int? month = null)
        {
            var archivesPath = "{0}/archives".FormatWith(GetBlogPath());

            if (year.HasValue && month.HasValue)
                archivesPath += "/{1}/{2}".FormatWith(year.Value, month.Value);

            return archivesPath;
        }

        private string GetPostTagsPath()
        {
            return "{0}/tags".FormatWith(GetBlogPath());
        }
    }
}
