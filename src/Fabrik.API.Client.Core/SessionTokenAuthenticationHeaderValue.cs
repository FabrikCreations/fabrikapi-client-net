using System.Net.Http.Headers;

namespace Fabrik.API.Client.Core
{
    public class SessionTokenAuthenticationHeaderValue : AuthenticationHeaderValue
    {
        private const string SessionTokenSchemeName = "Session";

        public SessionTokenAuthenticationHeaderValue(string token)
            : base(SessionTokenSchemeName, token)
        { }
    }
}
