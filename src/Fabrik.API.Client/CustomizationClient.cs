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
        private readonly int siteId;

        public CustomizationClient(ApiClient apiClient, int siteId)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            this.api = apiClient;
            this.siteId = siteId;
        }
        
        public Task<IEnumerable<ThemeSummary>> ListThemesAsync()
        {
            return api.GetAsync<IEnumerable<ThemeSummary>>(GetThemesPath());
        }

        public Task<Theme> GetThemeAsync(int themeId)
        {
            return api.GetAsync<Theme>(GetThemesPath(themeId));
        }

        public async Task ApplyThemeAsync(ApplyThemeCommand command)
        {
            await api.PostAsync(GetThemesPath(), command);
        }

        public Task<ThemeConfiguration> GetThemeConfigurationAsync(int themeId)
        {
            return api.GetAsync<ThemeConfiguration>(GetThemeConfigurationPath(themeId));
        }

        public async Task UpdateThemeConfigurationAsync(int themeId, UpdateThemeConfigurationCommand command)
        {
            await api.PostAsync(GetThemeConfigurationPath(themeId), command);
        }

        public async Task DeleteThemeConfigurationAsync(int themeId)
        {
            await api.DeleteAsync(GetThemeConfigurationPath(themeId));
        }

        private string GetThemesPath(int? themeId = null)
        {
            var themesPath = "sites/{0}/themes".FormatWith(siteId);

            if (themeId.HasValue)
                themesPath += "/" + themeId;

            return themesPath;
        }

        private string GetThemeConfigurationPath(int themeId)
        {
            return GetThemesPath(themeId) + "/configuration";
        }
    }
}
