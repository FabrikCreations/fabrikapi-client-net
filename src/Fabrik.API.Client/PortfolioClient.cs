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
        private readonly int siteId;
        
        public PortfolioClient(ApiClient apiClient, int siteId)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            this.api = apiClient;
            this.siteId = siteId;
        }
        
        public Task<PagedResult<Project>> ListProjectsAsync(int? pageSize = null, int? page = null, string slug = null, IEnumerable<string> tags = null, string term = null, int? categoryId = null, string categorySlug = null, bool? includeUnpublishedProjects = null)
        {
            var tagString = tags.JoinOrDefault(";");
            return api.GetAsync<PagedResult<Project>>(GetProjectsPath(), new { 
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

        public Task<Project> GetProjectAsync(int projectId)
        {
            return api.GetAsync<Project>(GetProjectsPath(projectId));
        }

        public Task<Project> AddProjectAsync(AddProjectCommand command)
        {
            return api.PostAsync<AddProjectCommand, Project>(GetProjectsPath(), command);
        }

        public async Task UpdateProjectAsync(int projectId, UpdateProjectCommand command)
        {
            await api.PutAsync(GetProjectsPath(projectId), command);
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            await api.DeleteAsync(GetProjectsPath(projectId));
        }

        public async Task MoveProjectAsync(MoveProjectCommand command)
        {
            await api.PutAsync(GetRootPortfolioPath(), command);
        }

        public Task<IEnumerable<MediaItem>> AddProjectMediaAsync(int projectId, params AddMediaCommand[] commands)
        {
            return api.PostAsync<IEnumerable<AddMediaCommand>, IEnumerable<MediaItem>>(GetProjectMediaPath(projectId), commands);
        }

        public async Task UpdateProjectMediaAsync(int projectId, int mediaItemId, UpdateMediaCommand command)
        {
            await api.PutAsync(GetProjectMediaPath(projectId, mediaItemId), command);
        }

        public async Task PatchProjectMediaAsync(int projectId, PatchMediaCommand command)
        {
            await api.PatchAsync(GetProjectMediaPath(projectId), command);
        }

        public async Task DeleteProjectMediaAsync(int projectId, int mediaItemId)
        {
            await api.DeleteAsync(GetProjectMediaPath(projectId, mediaItemId));
        }

        public Task<PagedResult<ProjectTagSummary>> ListTagsAsync(string term = null, int? pageSize = null, int? page = null)
        {
            return api.GetAsync<PagedResult<ProjectTagSummary>>(GetProjectTagsPath(), new { term = term, pageSize = pageSize, page = page });
        }

        public Task<PagedResult<PortfolioCategory>> ListCategoriesAsync(int? pageSize = null, int? page = null, int? parentCategoryId = null, string slug = null)
        {
            return api.GetAsync<PagedResult<PortfolioCategory>>(GetCategoriesPath(), new { pageSize = pageSize, page = page, parentCategoryId = parentCategoryId, slug = slug });
        }

        public Task<PortfolioCategory> GetCategoryAsync(int categoryId)
        {
            return api.GetAsync<PortfolioCategory>(GetCategoriesPath(categoryId));
        }

        public Task<PortfolioCategory> AddCategoryAsync(AddPortfolioCategoryCommand command)
        {
            return api.PostAsync<AddPortfolioCategoryCommand, PortfolioCategory>(GetCategoriesPath(), command);
        }

        public async Task UpdateCategoryAsync(int categoryId, UpdatePortfolioCategoryCommand command)
        {
            await api.PutAsync(GetCategoriesPath(categoryId), command);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await api.DeleteAsync(GetCategoriesPath(categoryId));
        }

        public Task<PagedResult<Portfolio>> ListPortfoliosAsync(int? pageSize = null, int? page = null, int? parentPortfolioId = null)
        {
            return api.GetAsync<PagedResult<Portfolio>>(GetPortfoliosPath(), new { pageSize = pageSize, page = page, parentPortfolioId = parentPortfolioId });
        }

        public Task<Portfolio> GetPortfolioAsync(int portfolioId)
        {
            return api.GetAsync<Portfolio>(GetPortfoliosPath(portfolioId));
        }

        public Task<Portfolio> GetPortfolioBySlugAsync(string slug)
        {
            return api.GetAsync<Portfolio>("{0}/byslug/{1}".FormatWith(GetPortfoliosPath(), slug));
        }

        public Task<Portfolio> AddPortfolioAsync(AddPortfolioCommand command)
        {
            return api.PostAsync<AddPortfolioCommand, Portfolio>(GetPortfoliosPath(), command);
        }

        public async Task UpdatePortfolioAsync(int portfolioId, UpdatePortfolioCommand command)
        {
            await api.PutAsync(GetPortfoliosPath(portfolioId), command);
        }

        public async Task DeletePortfolioAsync(int portfolioId)
        {
            await api.DeleteAsync(GetPortfoliosPath(portfolioId));
        }

        private string GetRootPortfolioPath()
        {
            return "sites/{0}/portfolio".FormatWith(siteId);
        }

        private string GetProjectsPath(int? projectId = null)
        {
            var projectsPath = "{0}/projects".FormatWith(GetRootPortfolioPath());

            if (projectId.HasValue)
                projectsPath += "/" + projectId;

            return projectsPath;
        }

        private string GetProjectMediaPath(int projectId, int? mediaItemId = null)
        {
            var projectMediaPath = "{0}/media".FormatWith(GetProjectsPath(projectId));

            if (mediaItemId.HasValue)
                projectMediaPath += "/" + mediaItemId;

            return projectMediaPath;
        }

        private string GetCategoriesPath(int? categoryId = null)
        {
            var categoriesPath = "{0}/categories".FormatWith(GetRootPortfolioPath());

            if (categoryId.HasValue)
                categoriesPath += "/" + categoryId;

            return categoriesPath;
        }

        private string GetPortfoliosPath(int? portfolioId = null)
        {
            var portfoliosPath = "sites/{0}/portfolios".FormatWith(siteId);

            if (portfolioId.HasValue)
                portfoliosPath += "/" + portfolioId;

            return portfoliosPath;
        }

        private string GetProjectTagsPath()
        {
            return "{0}/tags".FormatWith(GetRootPortfolioPath());
        }
    }
}
