using Fabrik.API.Client.Core;
using Fabrik.API.Core;
using Fabrik.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class AccountClient : IAccountClient
    {
        private readonly ApiClient api;

        public AccountClient(ApiClient apiClient)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            this.api = apiClient;
        }
        
        public Task<IEnumerable<Site>> ListSitesAsync()
        {
            return api.GetAsync<IEnumerable<Site>>(GetSitesPath());
        }

        private string GetSitesPath()
        {
            return "sites";
        }
    }
}
