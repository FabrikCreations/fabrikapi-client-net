using Fabrik.API.Common;
using Fabrik.CMS.API.Common;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IPagesClient
    {
        Task<PagedResult<Page>> GetPagesAsync(int siteId, int? pageSize = null, int? page = null, string slug = null, bool? includeUnpublishedPages = null);
        Task<Page> GetPageAsync(int siteId, int pageId);
        Task<Page> AddPageAsync(int siteId, AddPageCommand command);
        Task UpdatePageAsync(int siteId, int pageId, UpdatePageCommand command);
        Task DeletePageAsync(int siteId, int pageId);
    }
}
