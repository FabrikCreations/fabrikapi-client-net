using Fabrik.CMS.API.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface ICustomizationClient
    {
        Task<IEnumerable<ThemeSummary>> GetThemesAsync(int siteId);
        Task<Theme> GetThemeAsync(int siteId, int themeId);
        Task ApplyThemeAsync(int siteId, ApplyThemeCommand command);
        Task<ThemeConfiguration> GetThemeConfigurationAsync(int siteId, int themeId);
        Task UpdateThemeConfigurationAsync(int siteId, int themeId, UpdateThemeConfigurationCommand command);
        Task DeleteThemeConfigurationAsync(int siteId, int themeId);
    }
}
