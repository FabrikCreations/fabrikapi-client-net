using Fabrik.API.Core;
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
    }
}
