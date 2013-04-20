﻿using Fabrik.CMS.API.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface ISiteClient
    {
        Task<IEnumerable<Site>> GetSitesAsync();
        Task<Site> GetSiteAsync(int siteId);
        
        // Url Mappings        
        Task<UrlMapping> MapUrlAsync(int siteId, MapUrlCommand command);
        Task MakeUrlPrimaryAsync(int siteId, MakeUrlPrimaryCommand command);
        Task RemoveUrlAsync(int siteId, int urlMappingId);

        // Settings
        Task<SiteSettings> GetSiteSettingsAsync(int siteId);
        Task UpdateSiteSettingsAsync(int siteId, UpdateSiteSettingsCommand command);
    }
}
