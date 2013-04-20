using Fabrik.API.Client.Core;
using Fabrik.CMS.API.Common;
using Fabrik.Common;
using System;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class AuthClient : IAuthClient
    {
        private readonly ApiClient api;

        public AuthClient(ApiClient apiClient)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            api = apiClient;
        }
        
        public Task<SessionToken> GetSessionTokenAsync()
        {
            if (api.AuthenticationHeader.GetType() != typeof(BasicAuthenticationHeaderValue))
            {
                throw new InvalidOperationException("You must authenticate using the Basic scheme in order to obtain a session token.");
            }

            return api.GetAsync<SessionToken>("token");
        }

        public async Task<Identity> GetIdentityAsync()
        {
            var apiResult = await api.TryGetAsync<Identity>("identity").ConfigureAwait(false);
            return apiResult.Content;
        }
    }
}
