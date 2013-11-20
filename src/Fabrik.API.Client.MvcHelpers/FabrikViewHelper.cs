using Fabrik.API.Core;
using Fabrik.Common;
using ImageResizer.FluentExtensions;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fabrik.API.Client.MvcHelpers
{
    /// <summary>
    /// A view helper for creating ASP.NET MVC applications built on the fabrik API.
    /// </summary>
    public class FabrikViewHelper
    {
        private readonly UrlHelper url;
        private readonly ViewHelperConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of <see cref="FabrikViewHelper"/>.
        /// </summary>
        public FabrikViewHelper(RequestContext requestContext, RouteCollection routeCollection, ViewHelperConfiguration configuration = null)
        {
            this.url = new UrlHelper(requestContext, routeCollection);
            this.configuration = configuration ?? ViewHelperConfiguration.Configuration;
        }

        /// <summary>
        /// Configures a <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/> that can be reused.
        /// </summary>
        /// <param name="cfg">The configuration to apply to the builder instance.</param>
        /// <returns>A configured ImageUrlBuilder instance.</returns>
        public virtual ImageUrlBuilder ConfigureImageBuilder(Action<ImageUrlBuilder> cfg)
        {
            Ensure.Argument.NotNull(cfg, "cfg");
            var builder = new ImageUrlBuilder();
            cfg(builder);

            // Redirects source image URI to Fabrik Media Server for transformations
            if (configuration.UseMediaServer)
            {
                builder.FromFabrik(configuration);
            }

            return builder;
        }

        /// <summary>
        /// Renders an image from the source media <paramref name="image"/>.
        /// </summary>
        /// <param name="image">The image to render.</param>
        /// <param name="cfg">A configuration expression to apply to a <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/>.</param>
        /// <param name="useFallbackImage">Whether to display a fallback image if the <paramref name="image"/> is invalid.</param>
        /// <param name="fallbackImageUri">The fallback image to display if <paramref name="useFallbackImage"/> is true. Overrides the <see cref="ViewHelperConfiguration.FallbackImageUri"/>.</param>
        /// <param name="htmlAttributes">Optional HTML attributes to apply to the generated img tag.</param>
        /// <returns>A HTML image tag.</returns>
        public virtual IHtmlString DisplayImage(MediaItem image, Action<ImageUrlBuilder> cfg, bool useFallbackImage = false, string fallbackImageUri = null, object htmlAttributes = null)
        {
            Ensure.Argument.NotNull(cfg, "cfg");
            return DisplayImage(image, ConfigureImageBuilder(cfg), useFallbackImage, fallbackImageUri, htmlAttributes);
        }

        /// <summary>
        /// Renders an image from the source media <paramref name="image"/>.
        /// </summary>
        /// <param name="image">The image to render.</param>
        /// <param name="builder">A <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/> instance to use for generating the image URI.</param>
        /// <param name="useFallbackImage">Whether to display a fallback image if the <paramref name="image"/> is invalid.</param>
        /// <param name="fallbackImageUri">The fallback image to display if <paramref name="useFallbackImage"/> is true. Overrides the <see cref="ViewHelperConfiguration.FallbackImageUri"/>.</param>
        /// <param name="htmlAttributes">Optional HTML attributes to apply to the generated img tag.</param>
        /// <returns>A HTML image tag.</returns>
        public virtual IHtmlString DisplayImage(MediaItem image, ImageUrlBuilder builder, bool useFallbackImage = false, string fallbackImageUri = null, object htmlAttributes = null)
        {
            // Get either the image url or the fallbackimage (if enabled)
            var imageUrl = GetImageUrl(image, builder, useFallbackImage, fallbackImageUri);

            if (imageUrl.IsNullOrEmpty())
            {
                return MvcHtmlString.Empty;
            }

            return Image(imageUrl, image != null ? image.Title : "", htmlAttributes);
        }

        /// <summary>
        /// Gets an image URI for the source media <paramref name="image"/> using the <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/> configuration.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <param name="cfg">A configuration expression to apply to a <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/>.</param>
        /// <param name="useFallbackImage">Whether to display a fallback image if the <paramref name="image"/> is invalid.</param>
        /// <param name="fallbackImageUri">The fallback image to display if <paramref name="useFallbackImage"/> is true. Overrides the <see cref="ViewHelperConfiguration.FallbackImageUri"/>.</param>
        /// <returns>The generated image URI or fallback URI if the <paramref name="image"/> is invalid.</returns>
        public virtual string GetImageUrl(MediaItem image, Action<ImageUrlBuilder> cfg, bool useFallbackImage = false, string fallbackImageUri = null)
        {
            Ensure.Argument.NotNull(cfg, "cfg");
            return GetImageUrl(image, ConfigureImageBuilder(cfg), useFallbackImage, fallbackImageUri);
        }

        /// <summary>
        /// Gets an image URI for the source media <paramref name="image"/> using the <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/> instance.
        /// </summary>
        /// <param name="image">The source image.</param>
        /// <param name="builder">A <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/> instance to use for generating the image URI.</param>
        /// <param name="useFallbackImage">Whether to display a fallback image if the <paramref name="image"/> is invalid.</param>
        /// <param name="fallbackImageUri">The fallback image to display if <paramref name="useFallbackImage"/> is true. Overrides the <see cref="ViewHelperConfiguration.FallbackImageUri"/>.</param>
        /// <returns>The generated image URI or fallback URI if the <paramref name="image"/> is invalid.</returns>
        public virtual string GetImageUrl(MediaItem image, ImageUrlBuilder builder, bool useFallbackImage = false, string fallbackImageUri = null)
        {
            if (image == null)
            {
                if (useFallbackImage)
                {
                    var fallbackImage = fallbackImageUri ?? configuration.FallbackImageUri;
                    if (fallbackImage.IsNotNullOrEmpty())
                    {
                        return GetImageUrl(fallbackImage, builder);
                    }
                }

                return null;
            }

            Ensure.Argument.Is(image.MediaType == MediaType.Image, "The media item provided is not a valid image.");
            return GetImageUrl(image.Uri, builder);
        }

        /// <summary>
        /// Generates an image URI using the source <param name="imageUri"/> and <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/> configuration.
        /// </summary>
        /// <param name="imageUri">The source image URI.</param>
        /// <param name="cfg">A configuration expression to apply to a <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/>.</param>
        /// <returns>The generated image URI.</returns>
        public virtual string GetImageUrl(string imageUri, Action<ImageUrlBuilder> cfg)
        {
            Ensure.Argument.NotNull(cfg, "cfg");
            return GetImageUrl(imageUri, ConfigureImageBuilder(cfg));
        }

        /// <summary>
        /// Generates an image URI using the source <param name="imageUri"/> and <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/> instance.
        /// </summary>
        /// <param name="imageUri">The source image URI.</param>
        /// <param name="builder">A <see cref="ImageResizer.FluentExtensions.ImageUrlBuilder"/> instance to use for generating the image URI.</param>
        /// <returns>The generated image URI.</returns>
        public virtual string GetImageUrl(string imageUri, ImageUrlBuilder builder)
        {
            Ensure.Argument.NotNullOrEmpty(imageUri, "imageUri");
            Ensure.Argument.NotNull(builder, "builder");

            return builder.BuildUrl(imageUri);
        }

        /// <summary>
        /// Returns the default fallback image uri.
        /// </summary>
        public virtual string GetFallbackImageUri()
        {
            return configuration.FallbackImageUri;
        }

        /// <summary>
        /// Renders an embed link for the source <paramref name="embed"/>.
        /// </summary>
        /// <param name="embed">The media to embed.</param>
        /// <param name="width">The desired width of the embedded media.</param>
        /// <param name="height">The desired height of the embedded media.</param>
        /// <param name="autoplay">Whether to autoplay the embedded media (if applicable)</param>
        /// <param name="htmlAttributes">Optional HTML attributes to apply to the generated embed tag.</param>
        /// <returns>An embed link that can be convered into embedded media by the Fabrik JavaScript client.</returns>
        public virtual IHtmlString Embed(MediaItem embed, int? width = null, int? height = null, bool autoplay = false, object htmlAttributes = null)
        {
            if (embed == null)
            {
                return MvcHtmlString.Empty;
            }

            return Embed(embed.Uri, embed.Title, width, height, autoplay, htmlAttributes);
        }

        /// <summary>
        /// Renders an embed link for the source <paramref name="embedUri"/>.
        /// </summary>
        /// <param name="embedUri">The URI of the media to embed.</param>
        /// <param name="width">The desired width of the embedded media.</param>
        /// <param name="height">The desired height of the embedded media.</param>
        /// <param name="autoplay">Whether to autoplay the embedded media (if applicable)</param>
        /// <param name="htmlAttributes">Optional HTML attributes to apply to the generated embed tag.</param>
        /// <returns>An embed link that can be convered into embedded media by the Fabrik JavaScript client.</returns>
        public virtual IHtmlString Embed(string embedUri, string title = null, int? width = null, int? height = null, bool autoplay = false, object htmlAttributes = null)
        {
            Ensure.Argument.NotNullOrEmpty(embedUri, "embedUri");

            var builder = new TagBuilder("a");

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            builder.MergeAttribute("href", embedUri);
            builder.MergeAttribute("data-embed", "embed");

            if (width.HasValue)
            {
                builder.MergeAttribute("data-embed-width", width.Value.ToString());
            }

            if (height.HasValue)
            {
                builder.MergeAttribute("data-embed-height", height.Value.ToString());
            }

            if (autoplay)
            {
                builder.MergeAttribute("data-embed-autoplay", true.ToString());
            }

            if (title.IsNotNullOrEmpty())
            {
                builder.SetInnerText(title);
            }

            return MvcHtmlString.Create(builder.ToString());
        }

        protected virtual IHtmlString Image(string src, string alternateText = "", object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(src))
                throw new ArgumentException("src");

            var img = new TagBuilder("img");

            if (src.StartsWith("~/"))
                src = VirtualPathUtility.ToAbsolute(src);

            img.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            img.MergeAttribute("src", src);
            img.MergeAttribute("alt", alternateText);

            return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
        }
    }
}
