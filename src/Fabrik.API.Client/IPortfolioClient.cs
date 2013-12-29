using Fabrik.API.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IPortfolioClient
    {
        // Projects

        Task<PagedResult<Project>> GetProjectsAsync(int? pageSize = null, int? page = null, string slug = null, IEnumerable<string> tags = null, string term = null, int? categoryId = null, string categorySlug = null, bool? includeUnpublishedProjects = null);
        Task<Project> GetProjectAsync(int projectId);
        Task<Project> AddProjectAsync(AddProjectCommand command);
        Task UpdateProjectAsync(int projectId, UpdateProjectCommand command);
        Task DeleteProjectAsync(int projectId);
        Task MoveProjectAsync(MoveProjectCommand command);

        // Project Media

        Task<IEnumerable<MediaItem>> AddProjectMediaAsync(int projectId, params AddMediaCommand[] commands);
        Task UpdateProjectMediaAsync(int projectId, int mediaItemId, UpdateMediaCommand command);
        Task PatchProjectMediaAsync(int projectId, PatchMediaCommand command);
        Task DeleteProjectMediaAsync(int projectId, int mediaItemId);

        // Project Tags

        Task<PagedResult<ProjectTagSummary>> GetTagsAsync(string term = null, int? pageSize = null, int? page = null);
        Task<TaggedResult<Project>> GetProjectsByTagAsync(string tag, int? pageSize = null, int? page = null);

        // Project Categories

        Task<PagedResult<PortfolioCategory>> GetCategoriesAsync(int? pageSize = null, int? page = null, int? parentCategoryId = null, string slug = null);
        Task<PortfolioCategory> GetCategoryAsync(int categoryId);
        Task<PortfolioCategory> AddCategoryAsync(AddPortfolioCategoryCommand command);
        Task UpdateCategoryAsync(int categoryId, UpdatePortfolioCategoryCommand command);
        Task DeleteCategoryAsync(int categoryId);
    }
}
