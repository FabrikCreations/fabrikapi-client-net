using Fabrik.API.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface ICustomizationClient
    {
        Task<IEnumerable<ThemeSummary>> GetThemesAsync();
        Task<Theme> GetThemeAsync(int themeId);
        Task ApplyThemeAsync(ApplyThemeCommand command);
        Task<ThemeConfiguration> GetThemeConfigurationAsync(int themeId);
        Task UpdateThemeConfigurationAsync(int themeId, UpdateThemeConfigurationCommand command);
        Task DeleteThemeConfigurationAsync(int themeId);
    }
}
