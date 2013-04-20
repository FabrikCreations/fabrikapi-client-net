using Machine.Specifications;
using System;
using System.Net.Http;

namespace Fabrik.API.Client.Core.Specs
{
    [Subject(typeof(ApiClient), "Configuration")]
    public class ApiClientConfigurationSpecs
    {
        [Subject(typeof(ApiClient), "Connection strings")]
        public class Connection_Strings
        {
            static ApiClient client;
            static Exception exception;
            
            public class When_the_connection_string_is_empty
            {
                Because of = ()
                    => exception = Catch.Exception(() => ApiClient.ParseConnectionString(""));

                It Should_throw_an_exception = ()
                    => exception.ShouldBeOfType<ArgumentException>();
            }

            public class When_the_connection_string_does_not_contain_any_valid_connection_parameters
            {
                Because of = ()
                    => client = ApiClient.ParseConnectionString("foo=bar;");

                It Should_return_a_new_client_with_default_values = ()
                    =>
                    {
                        client.ShouldNotBeNull();
                        client.HttpClient.ShouldNotBeNull();
                        client.AuthenticationHeader.ShouldBeNull();
                    };
            }

            public class When_the_connection_string_contains_a_valid_absolute_url
            {
                Because of = ()
                    => client = ApiClient.ParseConnectionString("uri=https://api.onfabrik.com;");

                It Should_set_the_url = ()
                    => client.BaseUri.ShouldEqual(new Uri("https://api.onfabrik.com"));
            }

            public class When_the_connection_string_contains_both_username_and_password
            {
                Because of = ()
                    => client = ApiClient.ParseConnectionString("username=testuser;password=testpassword;");

                It Should_set_the_authentication_header_to_Basic = ()
                    => client.AuthenticationHeader.ShouldBeOfType<BasicAuthenticationHeaderValue>();
            }

            public class When_the_connection_string_contains_username_password_and_apikey
            {
                Because of = ()
                    => client = ApiClient.ParseConnectionString("username=testuser;password=testpassword;apikey=123456;");

                It Should_set_the_authentication_header_to_Basic = ()
                    => client.AuthenticationHeader.ShouldBeOfType<BasicAuthenticationHeaderValue>();
            }

            public class When_the_connection_string_contains_apikey_only
            {
                Because of = ()
                    => client = ApiClient.ParseConnectionString("apikey=123456;");

                It Should_set_the_authentication_header_to_ApiKey = ()
                    => client.AuthenticationHeader.ShouldBeOfType<ApiKeyAuthenticationHeaderValue>();
            }

            public class When_the_connection_string_name_does_not_exist_in_config_file
            {
                Because of = ()
                    => exception = Catch.Exception(() => ApiClient.FromConnectionString("foo"));

                It Should_throw_an_exception = ()
                    => exception.ShouldBeOfType<ArgumentException>();
            }

            public class When_the_connection_string_does_exist_in_config_file
            {
                Because of = ()
                    => client = ApiClient.FromConnectionString("apiconnectionstring");

                It Should_parse_the_connection_string_value = () =>
                {
                    client.ShouldNotBeNull();
                    client.BaseUri.ShouldEqual(new Uri("https://api.onfabrik.com"));

                    var header = new BasicAuthenticationHeaderValue("testuser", "testpassword");
                    client.AuthenticationHeader.Parameter.ShouldEqual(header.Parameter);
                };
            }
        } // connection strings


        [Subject(typeof(ApiClient), "Creating Request URI")]
        public class Creating_Request_Uri
        {
            static ApiClient apiClient;
            static Uri uri;
            static Exception exception;
            
            public class When_the_api_url_has_not_been_set
            {
                Establish ctx = () => apiClient = new ApiClient();

                Because of = () => exception = Catch.Exception(() => apiClient.CreateRequestUri("posts"));

                It Should_throw_an_exception = ()
                    => exception.ShouldBeOfType<InvalidOperationException>();
            }

            public class When_a_relative_path_is_provided
            {
                Establish ctx = () => apiClient = ApiClient.Create(cfg => cfg.ConnectTo("https://api.onfabrik.com"));

                Because of = () => uri = apiClient.CreateRequestUri("posts");

                It Should_create_an_absolute_uri_with_the_path = ()
                    => uri.AbsoluteUri.ShouldEqual("https://api.onfabrik.com/posts");
            }

            public class When_parameters_are_provided
            {
                Establish ctx = () => apiClient = ApiClient.Create(cfg => cfg.ConnectTo("https://api.onfabrik.com"));

                Because of = () => uri = apiClient.CreateRequestUri("posts", new { page = 10, pageSize = 20 });

                It Should_create_an_absolute_uri_with_querystring = ()
                    => uri.AbsoluteUri.ShouldEqual("https://api.onfabrik.com/posts?page=10&pageSize=20");
            }
        }

        [Subject(typeof(ApiClient), "Creating request")]
        public class Creating_request
        {
            static ApiClient apiClient;
            static HttpRequestMessage request;
            
            public class When_authentication_header_has_been_set
            {
                Establish ctx = ()
                    => apiClient = ApiClient.Create(cfg => cfg.UseApiKeyAuthentication("123456"));

                Because of = ()
                    => request = apiClient.CreateRequest(HttpMethod.Post, new Uri("https://api.onfabrik.com"));

                It Should_set_the_authorization_header_of_the_request = ()
                    => request.Headers.Authorization.ShouldBeOfType<ApiKeyAuthenticationHeaderValue>();
            }
        }
    }
}
