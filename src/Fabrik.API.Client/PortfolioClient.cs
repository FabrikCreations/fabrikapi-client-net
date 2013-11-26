using Fabrik.API.Client.Core;
using Fabrik.API.Core;
using Fabrik.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class PortfolioClient : IPortfolioClient
    {
        private readonly ApiClient api;
        
        public PortfolioClient(ApiClient apiClient)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            api = apiClient;
        }
        
        public Task<PagedResult<Project>> GetProjectsAsync(int siteId, int? pageSize = null, int? page = null, string slug = null, IEnumerable<string> tags = null, string term = null, int? categoryId = null, string categorySlug = null, bool? includeUnpublishedProjects = null)
        {
            var tagString = tags.JoinOrDefault(";");
            return api.GetAsync<PagedResult<Project>>(GetProjectsPath(siteId), new { 
                pageSize = pageSize, 
                page = page, 
                slug = slug, 
                tags = tagString, 
                categoryId = categoryId, 
                categorySlug = categorySlug,
                term = term, 
                unpublished = includeUnpublishedProjects 
            });
        }

        public Task<Project> GetProjectAsync(int siteId, int projectId)
        {
            return api.GetAsync<Project>(GetProjectsPath(siteId, projectId));
        }

        public Task<Project> AddProjectAsync(int siteId, AddProjectCommand command)
        {
            return api.PostAsync<AddProjectCommand, Project>(GetProjectsPath(siteId), command);
        }

        public async Task UpdateProjectAsync(int siteId, int projectId, UpdateProjectCommand command)
        {
            await api.PutAsync(GetProjectsPath(siteId, projectId), command);
        }

        public async Task DeleteProjectAsync(int siteId, int projectId)
        {
            await api.DeleteAsync(GetProjectsPath(siteId, projectId));
        }

        public async Task MoveProjectAsync(int siteId, MoveProjectCommand command)
        {
            await api.PutAsync(GetPortfolioPath(siteId), command);
        }

        public Task<IEnumerable<MediaItem>> AddProjectMediaAsync(int siteId, int projectId, params AddMediaCommand[] commands)
        {
            return api.PostAsync<IEnumerable<AddMediaCommand>, IEnumerable<MediaItem>>(GetProjectMediaPath(siteId, projectId), commands);
        }

        public async Task UpdateProjectMediaAsync(int siteId, int projectId, int mediaItemId, UpdateMediaCommand command)
        {
            await api.PutAsync(GetProjectMediaPath(siteId, projectId, mediaItemId), command);
        }

        public async Task PatchProjectMediaAsync(int siteId, int projectId, PatchMediaCommand command)
        {
            await api.PatchAsync(GetProjectMediaPath(siteId, projectId), command);
        }

        public async Task DeleteProjectMediaAsync(int siteId, int projectId, int mediaItemId)
        {
            await api.DeleteAsync(GetProjectMediaPath(siteId, projectId, mediaItemId));
        }

        public Task<PagedResult<ProjectTagSummary>> GetTagsAsync(int siteId, string term = null, int? pageSize = null, int? page = null)
        {
            return api.GetAsync<PagedResult<ProjectTagSummary>>(GetProjectTagsPath(siteId), new { term = term, pageSize = pageSize, page = page });
        }

        public async Task<TaggedResult<Project>> GetProjectsByTagAsync(int siteId, string tag, int? pageSize = null, int? page = null)
        {
            Ensure.Argument.NotNullOrEmpty(tag, "tag");
            var taggedProjects = await GetProjectsAsync(siteId, pageSize, page, tags: new[] { tag }).ConfigureAwait(false);
            return TaggedResult<Project>.Create(tag, taggedProjects);
        }

        public Task<PagedResult<PortfolioCategory>> GetCategoriesAsync(int siteId, int? pageSize = null, int? page = null, int? parentCategoryId = null, string slug = null)
        {
            return api.GetAsync<PagedResult<PortfolioCategory>>(GetCategoriesPath(siteId), new { pageSize = pageSize, page = page, parentCategoryId = parentCategoryId, slug = slug });
        }

        public Task<PortfolioCategory> GetCategoryAsync(int siteId, int categoryId)
        {
            return api.GetAsync<PortfolioCategory>(GetCategoriesPath(siteId, categoryId));
        }

        public Task<PortfolioCategory> AddCategoryAsync(int siteId, AddPortfolioCategoryCommand command)
        {
            return api.PostAsync<AddPortfolioCategoryCommand, PortfolioCategory>(GetCategoriesPath(siteId), command);
        }

        public async Task UpdateCategoryAsync(int siteId, int categoryId, UpdatePortfolioCategoryCommand command)
        {
            await api.PutAsync(GetCategoriesPath(siteId, categoryId), command);
        }

        public async Task DeleteCategoryAsync(int siteId, int categoryId)
        {
            await api.DeleteAsync(GetCategoriesPath(siteId, categoryId));
        }

        private string GetPortfolioPath(int siteId)
        {
            return "sites/{0}/portfolio".FormatWith(siteId);
        }

        private string GetProjectsPath(int siteId, int? projectId = null)
        {
            var projectsPath = "{0}/projects".FormatWith(GetPortfolioPath(siteId));

            if (projectId.HasValue)
                projectsPath += "/" + projectId;

            return projectsPath;
        }

        private string GetProjectMediaPath(int siteId, int projectId, int? mediaItemId = null)
        {
            var projectMediaPath = "{0}/media".FormatWith(GetProjectsPath(siteId, projectId));

            if (mediaItemId.HasValue)
                projectMediaPath += "/" + mediaItemId;

            return projectMediaPath;
        }

        private string GetCategoriesPath(int siteId, int? categoryId = null)
        {
            var categoriesPath = "{0}/categories".FormatWith(GetPortfolioPath(siteId));

            if (categoryId.HasValue)
                categoriesPath += "/" + categoryId;

            return categoriesPath;
        }

        private string GetProjectTagsPath(int siteId)
        {
            return "{0}/tags".FormatWith(GetPortfolioPath(siteId));
        }
    }
}
