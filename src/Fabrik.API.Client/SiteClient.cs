using Fabrik.API.Client.Core;
using Fabrik.API.Core;
using Fabrik.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class SiteClient : ISiteClient
    {
        private readonly ApiClient api;

        public SiteClient(ApiClient apiClient)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            api = apiClient;
        }

        public Task<IEnumerable<Site>> GetSitesAsync()
        {
            return api.GetAsync<IEnumerable<Site>>(GetSitesPath());
        }

        public Task<Site> GetSiteAsync(int siteId)
        {
            return api.GetAsync<Site>(GetSitesPath(siteId));
        }

        public Task<UrlMapping> MapUrlAsync(int siteId, MapUrlCommand command)
        {
            return api.PostAsync<MapUrlCommand, UrlMapping>(GetSiteUrlsPath(siteId), command);
        }

        public async Task MakeUrlPrimaryAsync(int siteId, MakeUrlPrimaryCommand command)
        {
            await api.PatchAsync(GetSiteUrlsPath(siteId), command);
        }

        public async Task RemoveUrlAsync(int siteId, int urlMappingId)
        {
            await api.DeleteAsync(GetSiteUrlsPath(siteId, urlMappingId));
        }


        public Task<SiteSettings> GetSiteSettingsAsync(int siteId)
        {
            return api.GetAsync<SiteSettings>(GetSiteSettingsPath(siteId));
        }

        public Task UpdateSiteSettingsAsync(int siteId, UpdateSiteSettingsCommand command)
        {
            return api.PutAsync(GetSiteSettingsPath(siteId), command);
        }

        private string GetSitesPath(int? siteId = null)
        {
            var sitesPath = "sites";
            
            if (siteId.HasValue)
                sitesPath += "/" + siteId;


            return sitesPath;
        }

        private string GetSiteUrlsPath(int siteId, int? urlMappingId = null)
        {
            var urlsPath = string.Concat(GetSitesPath(siteId), "/urls");

            if (urlMappingId.HasValue)
                urlsPath += "/" + urlMappingId;

            return urlsPath;
        }

        private string GetSiteSettingsPath(int siteId)
        {
            return string.Concat(GetSitesPath(siteId), "/settings");
        }
    }
}
