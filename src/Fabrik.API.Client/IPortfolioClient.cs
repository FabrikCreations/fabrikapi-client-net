﻿using Fabrik.API.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IPortfolioClient
    {
        // Projects

        Task<PagedResult<Project>> ListProjectsAsync(int? pageSize = null, int? page = null, string slug = null, IEnumerable<string> tags = null, string term = null, int? portfolioId = null, bool? includeUnpublishedProjects = null);
        Task<Project> GetProjectAsync(int projectId);
        Task<Project> GetProjectBySlugAsync(string slug);
        Task<Project> AddProjectAsync(AddProjectCommand command);
        Task UpdateProjectAsync(int projectId, UpdateProjectCommand command);
        Task DeleteProjectAsync(int projectId);
        Task MoveProjectAsync(MoveProjectCommand command);
        Task MoveProjectAsync(int portfolioId, MoveProjectCommand command);

        // Project Media

        Task<IEnumerable<MediaItem>> AddProjectMediaAsync(int projectId, params AddMediaCommand[] commands);
        Task UpdateProjectMediaAsync(int projectId, int mediaItemId, UpdateMediaCommand command);
        Task PatchProjectMediaAsync(int projectId, PatchMediaCommand command);
        Task DeleteProjectMediaAsync(int projectId, int mediaItemId);

        // Project Tags

        Task<PagedResult<ProjectTagSummary>> ListTagsAsync(string term = null, int? pageSize = null, int? page = null);

        // Portfolios

        Task<PagedResult<Portfolio>> ListPortfoliosAsync(int? pageSize = null, int? page = null, int? parentPortfolioId = null, bool? ignoreHeirarchy = null, GetPortfoliosCommand.SortByOptions? sortBy = null, bool? includeHidden = null);
        Task<Portfolio> GetPortfolioAsync(int portfolioId, GetPortfolioCommand.SortByOptions? sortPortfoliosBy = null);
        Task<Portfolio> GetPortfolioBySlugAsync(string slug, GetPortfolioCommand.SortByOptions? sortPortfoliosBy = null);
        Task<IEnumerable<Portfolio>> GetPortfolioTreeAsync(GetPortfolioTreeCommand.SortByOptions? sortBy = null);
        Task<Portfolio> AddPortfolioAsync(AddPortfolioCommand command);
        Task UpdatePortfolioAsync(int portfolioId, UpdatePortfolioCommand command);
        Task DeletePortfolioAsync(int portfolioId);
        Task MovePortfolioAsync(MovePortfolioCommand command);
        Task MovePortfolioAsync(int portfolioId, MovePortfolioCommand command);
    }
}
