using Fabrik.API.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface ISiteClient
    {
        Task<Site> GetSiteAsync();
        
        // Url Mappings        
        Task<UrlMapping> MapUrlAsync(MapUrlCommand command);
        Task MakeUrlPrimaryAsync(MakeUrlPrimaryCommand command);
        Task RemoveUrlAsync(int urlMappingId);

        // Settings
        Task<SiteSettings> GetSiteSettingsAsync();
        Task UpdateSiteSettingsAsync(UpdateSiteSettingsCommand command);

        // Redirects
        Task<IEnumerable<RedirectRule>> ListRedirectsAsync();
        Task<RedirectRule> GetRedirectAsync(int redirectId);
        Task<RedirectRule> AddRedirectAsync(AddRedirectRuleCommand command);
        Task DeleteRedirectAsync(int redirectId);
    }
}
