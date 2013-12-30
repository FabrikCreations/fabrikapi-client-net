﻿using Fabrik.API.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class SiteClientExtensions
    {
        public static async Task<Site> GetDefaultSiteAsync(this ISiteClient client)
        {
            var sites = await client.ListSitesAsync();
            return sites.FirstOrDefault();
        }
    }
}
