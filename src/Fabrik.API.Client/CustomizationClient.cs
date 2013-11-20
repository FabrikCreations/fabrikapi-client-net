using Fabrik.API.Client.Core;
using Fabrik.API.Core;
using Fabrik.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class CustomizationClient : ICustomizationClient
    {
        private readonly ApiClient api;

        public CustomizationClient(ApiClient apiClient)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            api = apiClient;
        }
        
        public Task<IEnumerable<ThemeSummary>> GetThemesAsync(int siteId)
        {
            return api.GetAsync<IEnumerable<ThemeSummary>>(GetThemesPath(siteId));
        }

        public Task<Theme> GetThemeAsync(int siteId, int themeId)
        {
            return api.GetAsync<Theme>(GetThemesPath(siteId, themeId));
        }

        public async Task ApplyThemeAsync(int siteId, ApplyThemeCommand command)
        {
            await api.PostAsync(GetThemesPath(siteId), command);
        }

        public Task<ThemeConfiguration> GetThemeConfigurationAsync(int siteId, int themeId)
        {
            return api.GetAsync<ThemeConfiguration>(GetThemeConfigurationPath(siteId, themeId));
        }

        public async Task UpdateThemeConfigurationAsync(int siteId, int themeId, UpdateThemeConfigurationCommand command)
        {
            await api.PostAsync(GetThemeConfigurationPath(siteId, themeId), command);
        }

        public async Task DeleteThemeConfigurationAsync(int siteId, int themeId)
        {
            await api.DeleteAsync(GetThemeConfigurationPath(siteId, themeId));
        }

        private string GetThemesPath(int siteId, int? themeId = null)
        {
            var themesPath = "sites/{0}/themes".FormatWith(siteId);

            if (themeId.HasValue)
                themesPath += "/" + themeId;

            return themesPath;
        }

        private string GetThemeConfigurationPath(int siteId, int themeId)
        {
            return GetThemesPath(siteId, themeId) + "/configuration";
        }
    }
}
