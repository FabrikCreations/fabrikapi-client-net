using Fabrik.API.Client.Core;
using Fabrik.API.Core;
using Fabrik.Common;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class PagesClient : IPagesClient
    {
        private readonly ApiClient api;
        private readonly int siteId;
        
        public PagesClient(ApiClient apiClient, int siteId)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            this.api = apiClient;
            this.siteId = siteId;
        }

        public Task<PagedResult<Page>> ListPagesAsync(int? pageSize = null, int? page = null, string slug = null, bool? includeUnpublishedPages = null)
        {
            return api.GetAsync<PagedResult<Page>>(GetPagesPath(), new { pageSize = pageSize, page = page, slug = slug, unpublished = includeUnpublishedPages });
        }

        public Task<Page> GetPageAsync(int pageId)
        {
            return api.GetAsync<Page>(GetPagesPath(pageId));
        }

        public Task<Page> AddPageAsync(AddPageCommand command)
        {
            return api.PostAsync<AddPageCommand, Page>(GetPagesPath(), command);
        }

        public async Task UpdatePageAsync(int pageId, UpdatePageCommand command)
        {
            await api.PutAsync(GetPagesPath(pageId), command);
        }

        public async Task DeletePageAsync(int pageId)
        {
            await api.DeleteAsync(GetPagesPath(pageId));
        }

        private string GetPagesPath(int? pageId = null)
        {
            var pagesPath = "sites/{0}/pages".FormatWith(siteId);

            if (pageId.HasValue)
                pagesPath += "/" + pageId;


            return pagesPath;
        }
    }
}
