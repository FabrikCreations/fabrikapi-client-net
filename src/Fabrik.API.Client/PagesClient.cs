using Fabrik.API.Client.Core;
using Fabrik.API.Common;
using Fabrik.API.Core;
using Fabrik.Common;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class PagesClient : IPagesClient
    {
        private readonly ApiClient api;
        
        public PagesClient(ApiClient apiClient)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            api = apiClient;
        }

        public Task<PagedResult<Page>> GetPagesAsync(int siteId, int? pageSize = null, int? page = null, string slug = null, bool? includeUnpublishedPages = null)
        {
            return api.GetAsync<PagedResult<Page>>(GetPagesPath(siteId), new { pageSize = pageSize, page = page, slug = slug, unpublished = includeUnpublishedPages });
        }

        public Task<Page> GetPageAsync(int siteId, int pageId)
        {
            return api.GetAsync<Page>(GetPagesPath(siteId, pageId));
        }

        public Task<Page> AddPageAsync(int siteId, AddPageCommand command)
        {
            return api.PostAsync<AddPageCommand, Page>(GetPagesPath(siteId), command);
        }

        public async Task UpdatePageAsync(int siteId, int pageId, UpdatePageCommand command)
        {
            await api.PutAsync(GetPagesPath(siteId, pageId), command);
        }

        public async Task DeletePageAsync(int siteId, int pageId)
        {
            await api.DeleteAsync(GetPagesPath(siteId, pageId));
        }

        private string GetPagesPath(int siteId, int? pageId = null)
        {
            var pagesPath = "sites/{0}/pages".FormatWith(siteId);

            if (pageId.HasValue)
                pagesPath += "/" + pageId;


            return pagesPath;
        }
    }
}
