using Fabrik.Common;
using Fabrik.Common.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fabrik.API.Client.Core
{
    /// <summary>
    /// A general purpose HTTP API Client
    /// </summary>
    public class ApiClient
    {
        private readonly JsonMediaTypeFormatter writeMediaTypeFormatter;
        private readonly MediaTypeFormatterCollection formatters;

        /// <summary>
        /// The provider used to initialize the underlying <see cref="System.Net.Http.HttpClient"/> instance.
        /// </summary>
        public virtual Lazy<HttpClient> HttpClientProvider { get; internal set; }

        /// <summary>
        /// The base URI to connect to.
        /// </summary>
        public virtual Uri BaseUri { get; internal set; }

        /// <summary>
        /// The authentication header used to authenticate requests.
        /// </summary>
        public virtual AuthenticationHeaderValue AuthenticationHeader { get; internal set; }

        /// <summary>
        /// Logger instance used to log requests and any errors.
        /// </summary>
        public virtual ILogger Logger { get; internal set; }

        /// <summary>
        /// Gets the <see cref="System.Net.Http.HttpClient"/> instance used by the client.
        /// </summary>
        public virtual HttpClient HttpClient
        {
            get
            {
                return HttpClientProvider.Value;
            }
        }

        /// <summary>
        /// Initializes a new <see cref="ApiClient"/> instance.
        /// </summary>
        public ApiClient()
        {
            writeMediaTypeFormatter = new JsonMediaTypeFormatter();
            formatters = new MediaTypeFormatterCollection(new[] { new JsonMediaTypeFormatter() });

            HttpClientProvider = new Lazy<HttpClient>(() => new HttpClient());
            Logger = new NullLogger();
        }

        /// <summary>
        /// Configures this <see cref="ApiClient"/> instance.
        /// </summary>
        /// <param name="configurationExpression">A configuration expression to apply to this instance.</param>
        public virtual ApiClient Configure(Action<ApiClientConfigurationExpression> configurationExpression)
        {
            Ensure.Argument.NotNull(configurationExpression, "configurationExpression");
            var configuration = new ApiClientConfigurationExpression(this);
            configurationExpression(configuration);

            return this;
        }

        /// <summary>
        /// Makes a HTTP GET request and reads the <typeparamref name="TResult"/> from the response.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="relativePath">The relative path of the resource.</param>
        /// <param name="parameters">An object containing querystring parameters.</param>
        /// <returns>An <see cref="ApiResponse{TResult}"/> containing the deserialized response or any errors that may have occurred.</returns>
        public virtual async Task<ApiResponse<TResult>> TryGetAsync<TResult>(string relativePath, object parameters = null)
        {
            Ensure.Argument.NotNullOrEmpty(relativePath, "relativePath");
            var request = CreateRequest(HttpMethod.Get, CreateRequestUri(relativePath, parameters));
            var response = await TrySendAsync(request).ConfigureAwait(false);

            return await ReadResponseAsync<TResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Makes a HTTP POST request.
        /// </summary>
        /// <typeparam name="TCommand">The type of command to POST.</typeparam>
        /// <param name="relativePath">The relative path of the resource.</param>
        /// <param name="command">The command to serialize and send in the request body.</param>
        /// <returns>An <see cref="ApiResponse"/> containing details of the response and any errors that may have occurred.</returns>
        public virtual async Task<ApiResponse> TryPostAsync<TCommand>(string relativePath, TCommand command)
        {
            Ensure.Argument.NotNullOrEmpty(relativePath, "relativePath");
            Ensure.Argument.NotNull(command, "command");

            var request = CreateRequest<TCommand>(HttpMethod.Post, CreateRequestUri(relativePath), command);
            var response = await TrySendAsync(request).ConfigureAwait(false);

            return await ReadResponseAsync(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Makes a HTTP POST request and reads the <typeparamref name="TResult"/> from the response. 
        /// </summary>
        /// <typeparam name="TCommand">The type of command to POST.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="relativePath">The relative path of the resource.</param>
        /// <param name="command">The command to serialize and send in the request body.</param>
        /// <returns>An <see cref="ApiResponse{TResult}"/> containing the deserialized response or any errors that may have occurred.</returns>
        public virtual async Task<ApiResponse<TResult>> TryPostAsync<TCommand, TResult>(string relativePath, TCommand command)
        {
            Ensure.Argument.NotNullOrEmpty(relativePath, "relativePath");
            Ensure.Argument.NotNull(command, "command");

            var request = CreateRequest<TCommand>(HttpMethod.Post, CreateRequestUri(relativePath), command);
            var response = await TrySendAsync(request).ConfigureAwait(false);

            return await ReadResponseAsync<TResult>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Makes a HTTP PUT request.
        /// </summary>
        /// <typeparam name="TCommand">The type of command to PUT.</typeparam>
        /// <param name="relativePath">The relative path of the resource.</param>
        /// <param name="command">The command to serialize and send in the request body.</param>
        /// <returns>An <see cref="ApiResponse"/> containing details of the response and any errors that may have occurred.</returns>
        public virtual async Task<ApiResponse> TryPutAsync<TCommand>(string relativePath, TCommand command)
        {
            Ensure.Argument.NotNullOrEmpty(relativePath, "relativePath");
            Ensure.Argument.NotNull(command, "command");

            var request = CreateRequest<TCommand>(HttpMethod.Put, CreateRequestUri(relativePath), command);
            var response = await TrySendAsync(request).ConfigureAwait(false);

            return await ReadResponseAsync(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Makes a HTTP DELETE request.
        /// </summary>
        /// <param name="relativePath">The relative path of the resource.</param>
        /// <returns>An <see cref="ApiResponse"/> containing details of the response and any errors that may have occurred.</returns>
        public virtual async Task<ApiResponse> TryDeleteAsync(string relativePath)
        {
            Ensure.Argument.NotNullOrEmpty(relativePath, "relativePath");

            var request = CreateRequest(HttpMethod.Delete, CreateRequestUri(relativePath));
            var response = await TrySendAsync(request).ConfigureAwait(false);

            return await ReadResponseAsync(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Makes a HTTP PATCH request.
        /// </summary>
        /// <typeparam name="TCommand">The type of command to PATCH.</typeparam>
        /// <param name="relativePath">The relative path of the resource.</param>
        /// <param name="command">The command to serialize and send in the request body.</param>
        /// <returns>An <see cref="ApiResponse"/> containing details of the response and any errors that may have occurred.</returns>
        public virtual async Task<ApiResponse> TryPatchAsync<TCommand>(string relativePath, TCommand command)
        {
            Ensure.Argument.NotNullOrEmpty(relativePath, "path");
            Ensure.Argument.NotNull(command, "command");

            var request = CreateRequest(new HttpMethod(Constants.HttpMethodPatch), CreateRequestUri(relativePath), command);
            var response = await TrySendAsync(request).ConfigureAwait(false);

            return await ReadResponseAsync(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads the <paramref name="response"/>.
        /// </summary>
        /// <param name="response">The HTTP response to read.</param>
        /// <returns>A <see cref="ApiResponse"/> containing details of the HTTP response.</returns>
        public virtual async Task<ApiResponse> ReadResponseAsync(HttpResponseMessage response)
        {
            var apiResponse = new ApiResponse(response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsAsync<HttpError>().ConfigureAwait(false);
                Logger.Error("Api Error. {0}: {1}", response.StatusCode, error);
                apiResponse.Error = new ApiError(response.StatusCode, error);
            }

            return apiResponse;
        }

        /// <summary>
        /// Reads the <typeparamref name="TResult"/> from the <paramref name="response"/>.
        /// </summary>
        /// <typeparam name="TResult">The object type to read from the response body.</typeparam>
        /// <param name="response">The HTTP response to read.</param>
        /// <returns>A <see cref="ApiResponse"/> containing details of the HTTP response.</returns>
        public virtual async Task<ApiResponse<TResult>> ReadResponseAsync<TResult>(HttpResponseMessage response)
        {
            Ensure.Argument.NotNull(response, "response");

            var apiResponse = new ApiResponse<TResult>(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                apiResponse.Content = await response.Content.ReadAsAsync<TResult>(formatters).ConfigureAwait(false);
            }
            else
            {
                if (response.Content != null && response.Content.Headers.ContentLength > 0)
                {
                    var error = await response.Content.ReadAsAsync<HttpError>(formatters).ConfigureAwait(false);
                    apiResponse.Error = new ApiError(response.StatusCode, error);
                }
                else
                {
                    apiResponse.Error = new ApiError(response.StatusCode);
                }

                Logger.Error("Api Error. {0} {1}: {2}", (int)response.StatusCode, response.StatusCode,
                    apiResponse.Error != null ? apiResponse.Error.Message : response.ReasonPhrase);
            }

            return apiResponse;
        }

        /// <summary>
        /// Attempts to send the <param name="request"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{HttpResponseMessage}"/> if the request succeeds.
        /// </returns>
        public virtual async Task<HttpResponseMessage> TrySendAsync(HttpRequestMessage request)
        {
            Ensure.Argument.NotNull(request, "request");
            Logger.Debug("Sending request to {0}", request.RequestUri.AbsoluteUri);

            try
            {
                return await HttpClient.SendAsync(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error sending request to {0}.", request.RequestUri.AbsoluteUri);
                throw;
            }
        }

        /// <summary>
        /// Constructs a <see cref="System.Net.Http.HttpRequestMessage"/> containing the specfied <paramref name="content"/>.
        /// </summary>
        /// <typeparam name="TContent">The content type.</typeparam>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="content">The content that should be sent in the request body.</param>
        public virtual HttpRequestMessage CreateRequest<TContent>(HttpMethod httpMethod, Uri requestUri, TContent content)
        {
            Ensure.Argument.NotNull(requestUri, "requestUri");
            Ensure.Argument.NotNull(content, "content");

            ValidateContent(content);

            var request = CreateRequest(httpMethod, requestUri);
            request.Content = new ObjectContent<TContent>(content, writeMediaTypeFormatter);

            return request;
        }

        /// <summary>
        /// Constructs a <see cref="System.Net.Http.HttpRequestMessage"/>.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="requestUri">The request URI.</param>
        public virtual HttpRequestMessage CreateRequest(HttpMethod httpMethod, Uri requestUri)
        {
            Ensure.Argument.NotNull(requestUri, "requestUri");

            var request = new HttpRequestMessage(httpMethod, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaTypeFormatter.DefaultMediaType.MediaType));
            request.Headers.Add("X-UserAgent", "fabrikapi-client-net");

            if (AuthenticationHeader != null)
            {
                request.Headers.Authorization = AuthenticationHeader;
            }

            return request;
        }

        /// <summary>
        /// Creates an absolute request Uri using the <see cref="ApiClient.BaseUri"/> and the 
        /// specified <paramref name="relativePath"/>.
        /// </summary>
        /// <param name="relativePath">The relatve path.</param>
        /// <param name="parameters">An object containing querystring parameters.</param>
        public virtual Uri CreateRequestUri(string relativePath, object parameters = null)
        {
            Ensure.Argument.NotNullOrEmpty(relativePath, "relativePath");

            if (BaseUri == null)
            {
                throw new InvalidOperationException("The ApiUrl has not been set.");
            }

            if (parameters != null)
            {
                var query = new QuerystringCollection(parameters).ToString();
                if (query.IsNotNullOrEmpty())
                    relativePath = "{0}?{1}".FormatWith(relativePath.Trim('/'), query);
            }

            return new Uri(BaseUri, relativePath);
        }

        /// <summary>
        /// Validates the <paramref name="content"/>.
        /// </summary>
        /// <param name="content">The content to validate.</param>
        protected virtual void ValidateContent(object content)
        {
            Ensure.Argument.NotNull(content, "content");
            Validator.ValidateObject(content, new ValidationContext(content));
        }

        /// <summary>
        /// Creates a new <see cref="ApiClient"/> instance using the provided configuration.
        /// </summary>
        /// <param name="configurationExpression">An expression containing the configuration to apply to the constructed instance.</param>
        public static ApiClient Create(Action<ApiClientConfigurationExpression> configurationExpression)
        {
            Ensure.Argument.NotNull(configurationExpression, "configurationExpression");
            var apiClient = new ApiClient();
            return apiClient.Configure(configurationExpression);
        }

        /// <summary>
        /// Creates a new <see cref="ApiClient"/> instance from the connection string with the  <paramref name="connectionStringName"/>.
        /// </summary>
        /// <param name="connectionStringName">The connection string name.</param>
        public static ApiClient FromConnectionString(string connectionStringName)
        {
            Ensure.Argument.NotNullOrEmpty(connectionStringName, "connectionStringName");

            var connectionString = GetConnectionStringFromAppConfig(connectionStringName);
            if (connectionString == null)
            {
                throw new ArgumentException("Connection string '{0}' could not be found.".FormatWith(connectionStringName));
            }

            return ApiClient.ParseConnectionString(connectionString);
        }

        /// <summary>
        /// Creates a new <see cref="ApiClient"/> instance from the <paramref name="connectionString"/>.
        /// </summary>
        /// <param name="connectionString">The connection string that should be parsed.</param>
        public static ApiClient ParseConnectionString(string connectionString)
        {
            Ensure.Argument.NotNullOrEmpty(connectionString, "connectionString");
            var config = ConnectionStringParser.Parse<ApiConnectionConfiguration>(connectionString);

            return CreateFromConnection(config);
        }

        /// <summary>
        /// Creates a new <see cref="ApiClient"/> instance from a connection object.
        /// </summary>
        /// <param name="configuration">An <see cref="ApiConnectionConfiguration"/> containing API connection details.</param>
        private static ApiClient CreateFromConnection(ApiConnectionConfiguration configuration)
        {
            Ensure.Argument.NotNull(configuration, "configuration");

            var client = new ApiClient();

            if (configuration.Uri.IsNotNullOrEmpty() && configuration.Uri.IsValidUrl())
            {
                client.BaseUri = new Uri(configuration.Uri);
            }

            // default to basic auth if it exists
            if (configuration.Username.IsNotNullOrEmpty() && configuration.Password.IsNotNullOrEmpty())
            {
                client.AuthenticationHeader = new BasicAuthenticationHeaderValue(configuration.Username, configuration.Password);
            }
            else if (configuration.ApiKey.IsNotNullOrEmpty())
            {
                client.AuthenticationHeader = new ApiKeyAuthenticationHeaderValue(configuration.ApiKey);
            }

            return client;
        }

        /// <summary>
        /// Reads a connection string from the application configuration file.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string to read.</param>
        private static string GetConnectionStringFromAppConfig(string connectionStringName)
        {
            Ensure.Argument.NotNullOrEmpty(connectionStringName, "connectionStringName");
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
            return connectionString != null ? connectionString.ConnectionString : null;
        }
    }
}
