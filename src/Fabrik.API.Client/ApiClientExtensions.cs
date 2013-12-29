using Fabrik.API.Client.Core;

namespace Fabrik.API.Client
{
    public static class ApiClientExtensions
    {
        public static ISiteClient GetSiteClient(this ApiClient apiClient)
        {
            return new SiteClient(apiClient);
        }

        public static IBlogClient GetBlogClient(this ApiClient apiClient, int siteId)
        {
            return new BlogClient(apiClient, siteId);
        }

        public static IPortfolioClient GetPortfolioClient(this ApiClient apiClient, int siteId)
        {
            return new PortfolioClient(apiClient, siteId);
        }

        public static IPagesClient GetPagesClient(this ApiClient apiClient, int siteId)
        {
            return new PagesClient(apiClient, siteId);
        }

        public static IMediaClient GetMediaClient(this ApiClient apiClient, int siteId)
        {
            return new MediaClient(apiClient, siteId);
        }

        public static IMenusClient GetMenusClient(this ApiClient apiClient, int siteId)
        {
            return new MenusClient(apiClient, siteId);
        }

        public static IAuthClient GetAuthClient(this ApiClient apiClient)
        {
            return new AuthClient(apiClient);
        }

        public static ICustomizationClient GetCustomizationClient(this ApiClient apiClient, int siteId)
        {
            return new CustomizationClient(apiClient, siteId);
        }
    }
}
