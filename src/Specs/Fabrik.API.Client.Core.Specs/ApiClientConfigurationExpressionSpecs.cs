using Machine.Specifications;
using System;
using System.Net.Http;

namespace Fabrik.API.Client.Core.Specs
{
    [Subject(typeof(ApiClientConfigurationExpression))]
    public class ApiClientConfigurationExpressionSpecs
    {
        static ApiClient client;
        
        public class UseBasicAuthentication
        {
            Because of = ()
                => client = ApiClient.Create(cfg => cfg.UseBasicAuthentication("testuser", "testpassword"));

            It Should_set_the_authentication_header_to_Basic = ()
                => client.AuthenticationHeader.ShouldBeOfType<BasicAuthenticationHeaderValue>();
        }

        public class When_UseApiKeyAuthentication
        {
            Because of = ()
                => client = ApiClient.Create(cfg => cfg.UseApiKeyAuthentication("123456"));

            It Should_set_the_authentication_header_to_ApiKey = ()
                => client.AuthenticationHeader.ShouldBeOfType<ApiKeyAuthenticationHeaderValue>();
        }

        public class When_UseSessionTokenAuthentication
        {
            Because of = ()
                => client = ApiClient.Create(cfg => cfg.UseSessionTokenAuthentication("123456"));

            It Should_set_the_authentication_header_to_Session = ()
                => client.AuthenticationHeader.ShouldBeOfType<SessionTokenAuthenticationHeaderValue>();
        }

        public class When_SetHttpClientProvider
        {
            static HttpClient httpClient = new HttpClient();

            Because of = ()
                => client = ApiClient.Create(cfg => cfg.SetHttpClientProvider(() => httpClient));

            It Should_use_the_custom_provider_to_set_the_http_client_instance = ()
                => client.HttpClient.ShouldBeTheSameAs(httpClient);
        }

        public class When_ConnectTo
        {
            Because of = ()
                => client = ApiClient.Create(cfg => cfg.ConnectTo("https://api.onfabrik.com"));

            It Should_set_the_api_url = ()
                => client.BaseUri.ShouldEqual(new Uri("https://api.onfabrik.com"));
        }
    }
}
