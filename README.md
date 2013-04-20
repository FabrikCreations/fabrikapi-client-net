# Fabrik API Client for .NET

This client makes it easy to work with the Fabrik API from your .NET applications.

## Quick Start

	Install-Package Fabrik.API.Client -pre

In order to stay as close to the Fabrik API resource heirarchy as possible, the library follows a "client-per-resource" pattern. For example, to work with your Fabrik Blog content you would create an instance of `BlogClient`.

All resource clients require an `ApiClient` instance to communicate with the Fabrik API. The `ApiClient` provides both code-first and file based configuration:

**Code First**

    var api = ApiClient.Create(cfg =>
    {
        cfg.ConnectTo("https://api.onfabrik.com");
        cfg.UseBasicAuthentication("you@domain.com", "YourFabrikPassword");
    });

**Configuration File**

	<connectionStrings>
		<add name="FabrikAPI" 
			 connectionString="Uri=https://api.onfabrik.com;username=you@domain.com;password=YourFabrikPassword"/>
	</connectionStrings>

	var api = ApiClient.FromConnectionString("FabrikAPI");

The `ApiClient` will initialize a `HttpClient` internally but you can pass your own "provider" if you want to override how the `HttpClient` is created. This is useful if you want to use the same `HttpClient` instance throughout your application (best practice) or if you want to set default request headers.

    class Program
    {
        static HttpClient httpClient = new HttpClient();
        
        static void Main(string[] args)
        {
            var api = ApiClient.FromConnectionString("FabrikAPI")
                .Configure(cfg => cfg.SetHttpClientProvider(() => httpClient));
		}
	}

Extension methods are provided for creating resource clients. To create a `BlogClient` simply call `GetBlogClient()` on your `ApiClient` instance.

All API operations are asynchronous, returning `Task` or `Task<T>`.

## Example - Querying all blog posts tagged with "Fabrik"

	using Fabrik.API.Client;
	using Fabrik.API.Client.Core;
	using System;
	using System.Threading.Tasks;
	
	namespace FabrikAPIClientDemo
	{
	    class Program
	    {
	        static void Main(string[] args)
	        {
	            new Program().Run().Wait();
	        }
	
	        async Task Run()
	        {
	            var api = ApiClient.Create(cfg =>
	            {
	                cfg.ConnectTo("https://api.onfabrik.com");
	                cfg.UseBasicAuthentication("you@domain.com", "YourFabrikPassword");
	            });
	
	            // Most API calls require a site identifier so we need to get the site first
	            // If you didn't want to do this every time you could store the siteid in your config file
	            var site = await api.GetSiteClient().GetDefaultSite();
	            
	            // Get an IBlogClient instance so we can work with our Fabrik Blog
	            var blog = api.GetBlogClient();
	
				// all async baby!
	            var result = await blog.GetPostsAsync(
	                siteId: site.Id,
	                tags: new[] { "Fabrik" },
	                pageSize: 20
	            );
	
	            // Most Get operations return a paged result set
	            Console.WriteLine("We found {0} posts that matched your search.", result.TotalCount);
	            
	            // Items returns the underlying items of the paged result set
	            foreach (var post in result.Items)
	            {
	                Console.WriteLine(post.Title);
	            }
	
	            Console.ReadLine();
	        }
	    }
	}


## License

Licensed under the MIT License.