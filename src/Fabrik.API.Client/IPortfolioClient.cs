using Fabrik.API.Common;
using Fabrik.CMS.API.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IPortfolioClient
    {
        // Projects

        Task<PagedResult<Project>> GetProjectsAsync(int siteId, int? pageSize = null, int? page = null, string slug = null, IEnumerable<string> tags = null, string term = null, int? categoryId = null, string categorySlug = null, bool? includeUnpublishedProjects = null);
        Task<Project> GetProjectAsync(int siteId, int projectId);
        Task<Project> AddProjectAsync(int siteId, AddProjectCommand command);
        Task UpdateProjectAsync(int siteId, int projectId, UpdateProjectCommand command);
        Task DeleteProjectAsync(int siteId, int projectId);
        Task MoveProjectAsync(int siteId, MoveProjectCommand command);

        // Project Media

        Task<IEnumerable<MediaItem>> AddProjectMediaAsync(int siteId, int projectId, params AddMediaCommand[] commands);
        Task UpdateProjectMediaAsync(int siteId, int projectId, int mediaItemId, UpdateMediaCommand command);
        Task PatchProjectMediaAsync(int siteId, int projectId, PatchMediaCommand command);
        Task DeleteProjectMediaAsync(int siteId, int projectId, int mediaItemId);

        // Project Tags

        Task<PagedResult<ProjectTagSummary>> GetTagsAsync(int siteId, string term = null, int? pageSize = null, int? page = null);
        Task<TaggedResult<Project>> GetProjectsByTagAsync(int siteId, string tag, int? pageSize = null, int? page = null);

        // Project Categories

        Task<PagedResult<PortfolioCategory>> GetCategoriesAsync(int siteId, int? pageSize = null, int? page = null, int? parentCategoryId = null, string slug = null);
        Task<PortfolioCategory> GetCategoryAsync(int siteId, int categoryId);
        Task<PortfolioCategory> AddCategoryAsync(int siteId, AddPortfolioCategoryCommand command);
        Task UpdateCategoryAsync(int siteId, int categoryId, UpdatePortfolioCategoryCommand command);
        Task DeleteCategoryAsync(int siteId, int categoryId);
    }
}
