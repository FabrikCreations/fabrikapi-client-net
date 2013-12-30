using Fabrik.API.Core;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IPagesClient
    {
        Task<PagedResult<Page>> ListPagesAsync(int? pageSize = null, int? page = null, string slug = null, bool? includeUnpublishedPages = null);
        Task<Page> GetPageAsync(int pageId);
        Task<Page> AddPageAsync(AddPageCommand command);
        Task UpdatePageAsync(int pageId, UpdatePageCommand command);
        Task DeletePageAsync(int pageId);
    }
}
