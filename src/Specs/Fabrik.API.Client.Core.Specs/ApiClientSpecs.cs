using Fabrik.Common.Logging;
using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Fabrik.API.Client.Core.Specs
{
    [Subject(typeof(ApiClient))]
    public class ApiClientSpecs
    {
        static ApiClient client;
        static ILogger logger;

        public class When_initializing
        {
            Because of = () => client = new ApiClient();

            It Should_have_a_default_http_client_provider = ()
                => client.HttpClientProvider.ShouldNotBeNull();

            It Should_have_a_null_logger = ()
                => client.Logger.ShouldBeOfType<NullLogger>();

            It Should_not_have_a_base_uri = ()
                => client.BaseUri.ShouldBeNull();

            It Should_not_have_any_authentication_headers_set = ()
                => client.AuthenticationHeader.ShouldBeNull();
        }

        public class When_the_response_is_unsuccessful : WithFakes
        {
            static Exception exception;

            Establish ctx = () =>
            {
                logger = An<ILogger>();
                var httpClient = new HttpClient(new AlwaysReturnsErrorHandler());
                client = ApiClient.Create(cfg =>
                {
                    cfg.ConnectTo("http://localhost");
                    cfg.UseBasicAuthentication("username", "password");
                    cfg.SetHttpClientProvider(() => httpClient);
                    cfg.LogUsing(logger);
                });
            };

            Because of = ()
                => exception = Catch.Exception(() => client.GetAsync<string>("values").Wait());

            It Should_log_the_error = ()
                => logger.WasToldTo(x => x.Error(Param.IsAny<string>(), Param.IsAny<object[]>()));

            It Should_contain_the_underlying_api_error = ()
                => ((ApiResponseException)exception.InnerException).Error.ShouldNotBeNull();

            It Should_throw_an_api_response_exception = ()
                => exception.InnerException.ShouldBeOfType<ApiResponseException>();
        }

        public class When_the_response_is_not_successful_and_returns_errors : WithFakes
        {
            static Exception exception;
            
            Establish ctx = () =>
            {
                logger = An<ILogger>();
                var httpClient = new HttpClient(new AlwaysReturnsErrorHandler(true));
                client = ApiClient.Create(cfg =>
                {
                    cfg.ConnectTo("http://localhost");
                    cfg.UseBasicAuthentication("username", "password");
                    cfg.SetHttpClientProvider(() => httpClient);
                    cfg.LogUsing(logger);
                });
            };

            Because of = ()
                => exception = Catch.Exception(() => client.GetAsync<string>("values").Wait());

            It Should_contain_the_underlying_api_error_modelstate = ()
                => ((ApiResponseException)exception.InnerException).Error.ModelState.Count.ShouldEqual(1);
        }
      
        public class AlwaysReturnsErrorHandler : DelegatingHandler
        {
            private bool includeModelState;
            
            public AlwaysReturnsErrorHandler(bool includeModelState = false)
            {
                this.includeModelState = includeModelState;
            }
            
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (!includeModelState)
                {
                    return Task.FromResult(request.CreateResponse(HttpStatusCode.BadRequest));
                }
                
                var modelState = new ModelStateDictionary();
                modelState.AddModelError("Name", "The name is required.");
                var response = request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
                return Task.FromResult(response);
            }
        }
    }
}
