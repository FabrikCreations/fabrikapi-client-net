using Fabrik.API.Client.Core;

namespace Fabrik.API.Client
{
    public static class ApiClientExtensions
    {
        public static ISiteClient GetSiteClient(this ApiClient apiClient)
        {
            return new SiteClient(apiClient);
        }

        public static IBlogClient GetBlogClient(this ApiClient apiClient)
        {
            return new BlogClient(apiClient);
        }

        public static IPortfolioClient GetPortfolioClient(this ApiClient apiClient)
        {
            return new PortfolioClient(apiClient);
        }

        public static IPagesClient GetPagesClient(this ApiClient apiClient)
        {
            return new PagesClient(apiClient);
        }

        public static IMediaClient GetMediaClient(this ApiClient apiClient)
        {
            return new MediaClient(apiClient);
        }

        public static IMenusClient GetMenusClient(this ApiClient apiClient)
        {
            return new MenusClient(apiClient);
        }

        public static IAuthClient GetAuthClient(this ApiClient apiClient)
        {
            return new AuthClient(apiClient);
        }

        public static ICustomizationClient GetCustomizationClient(this ApiClient apiClient)
        {
            return new CustomizationClient(apiClient);
        }
    }
}
