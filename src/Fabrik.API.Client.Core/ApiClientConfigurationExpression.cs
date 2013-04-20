using Fabrik.Common;
using Fabrik.Common.Logging;
using System;
using System.Net.Http;

namespace Fabrik.API.Client.Core
{
    /// <summary>
    /// Used to represent a <see cref="ApiClient"/> configuration.
    /// </summary>
    public class ApiClientConfigurationExpression
    {
        private readonly ApiClient apiClient;

        /// <summary>
        /// Creates a new <see cref="ApiClientConfigurationExpression"/> instance.
        /// </summary>
        /// <param name="apiClient">The <see cref="ApiClient"/> instance to apply the configuration to.</param>
        internal ApiClientConfigurationExpression(ApiClient apiClient)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            this.apiClient = apiClient;
        }

        /// <summary>
        /// Sets the base URI of for all requests.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        public ApiClientConfigurationExpression ConnectTo(string baseUri)
        {
            Ensure.Argument.NotNullOrEmpty(baseUri, "baseUri");
            Ensure.Argument.Is(baseUri.IsValidUrl(), "You must specify a valid absolute url.");

            apiClient.BaseUri = new Uri(baseUri);
            return this;
        }

        /// <summary>
        /// Configures the client to use Basic authentication.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public ApiClientConfigurationExpression UseBasicAuthentication(string username, string password)
        {
            Ensure.Argument.NotNullOrEmpty(username, "username");
            Ensure.Argument.NotNullOrEmpty(password, "password");

            apiClient.AuthenticationHeader = new BasicAuthenticationHeaderValue(username, password);
            return this;
        }

        /// <summary>
        /// Configures the client to use ApiKey (token) based authentication.
        /// </summary>
        /// <param name="apiKey">The apikey to send in the authentication header.</param>
        public ApiClientConfigurationExpression UseApiKeyAuthentication(string apiKey)
        {
            Ensure.Argument.NotNullOrEmpty(apiKey, "apikey");

            apiClient.AuthenticationHeader = new ApiKeyAuthenticationHeaderValue(apiKey);
            return this;
        }

        /// <summary>
        /// Configures the client to use session token authentication.
        /// </summary>
        /// <param name="token">The session token to send in the authentication header.</param>
        public ApiClientConfigurationExpression UseSessionTokenAuthentication(string token)
        {
            Ensure.Argument.NotNullOrEmpty(token, "token");
            apiClient.AuthenticationHeader = new SessionTokenAuthenticationHeaderValue(token);
            return this;
        }

        /// <summary>
        /// Sets the provider used to create the underlying <see cref="System.Net.Http.HttpClient"/> instance
        /// used by the <see cref="ApiClient"/>.
        /// </summary>
        /// <param name="httpClientProvider">A factory function that returns a <see cref="System.Net.Http.HttpClient"/> instance.</param>
        public ApiClientConfigurationExpression SetHttpClientProvider(Func<HttpClient> httpClientProvider)
        {
            Ensure.Argument.NotNull(httpClientProvider, "httpClientProvider");
            apiClient.HttpClientProvider = new Lazy<HttpClient>(httpClientProvider);
            return this;
        }

        /// <summary>
        /// Sets the logger used by the client to log activity and errors.
        /// </summary>
        /// <param name="logger">A logger instance.</param>
        public ApiClientConfigurationExpression LogUsing(ILogger logger)
        {
            Ensure.Argument.NotNull(logger, "logger");
            apiClient.Logger = logger;
            return this;
        }
    }
}
