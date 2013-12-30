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
        private readonly int siteId;

        public SiteClient(ApiClient apiClient, int siteId)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            this.api = apiClient;
            this.siteId = siteId;
        }

        public Task<Site> GetSiteAsync()
        {
            return api.GetAsync<Site>(GetSitePath());
        }

        // Site URLs

        public Task<UrlMapping> MapUrlAsync(MapUrlCommand command)
        {
            return api.PostAsync<MapUrlCommand, UrlMapping>(GetSiteUrlsPath(), command);
        }

        public async Task MakeUrlPrimaryAsync(MakeUrlPrimaryCommand command)
        {
            await api.PatchAsync(GetSiteUrlsPath(), command);
        }

        public async Task RemoveUrlAsync(int urlMappingId)
        {
            await api.DeleteAsync(GetSiteUrlsPath(urlMappingId));
        }

        // Site Settings

        public Task<SiteSettings> GetSiteSettingsAsync()
        {
            return api.GetAsync<SiteSettings>(GetSiteSettingsPath());
        }

        public Task UpdateSiteSettingsAsync(UpdateSiteSettingsCommand command)
        {
            return api.PutAsync(GetSiteSettingsPath(), command);
        }

        // Site Redirect Rules

        public Task<IEnumerable<RedirectRule>> ListRedirectsAsync()
        {
            return api.GetAsync<IEnumerable<RedirectRule>>(GetSiteRedirectsPath());
        }

        public Task<RedirectRule> GetRedirectAsync(int redirectId)
        {
            return api.GetAsync<RedirectRule>(GetSiteRedirectsPath(redirectId));
        }

        public Task<RedirectRule> AddRedirectAsync(AddRedirectRuleCommand command)
        {
            return api.PostAsync<AddRedirectRuleCommand, RedirectRule>(GetSiteRedirectsPath(), command);
        }

        public async Task DeleteRedirectAsync(int redirectId)
        {
            await api.DeleteAsync(GetSiteRedirectsPath(redirectId));
        }

        private string GetSitePath()
        {
            return "sites/" + siteId;
        }

        private string GetSiteUrlsPath(int? urlMappingId = null)
        {
            var urlsPath = string.Concat(GetSitePath(), "/urls");

            if (urlMappingId.HasValue)
            {
                urlsPath += "/" + urlMappingId;
            }

            return urlsPath;
        }

        private string GetSiteRedirectsPath(int? redirectId = null)
        {
            var redirectsPath = string.Concat(GetSitePath(), "/redirects");

            if (redirectId.HasValue)
            {
                redirectsPath += "/" + redirectsPath;
            }

            return redirectsPath;
        }

        private string GetSiteSettingsPath()
        {
            return string.Concat(GetSitePath(), "/settings");
        }
    }
}
