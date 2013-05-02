using Fabrik.Common;
using ImageResizer.FluentExtensions;
using System;

namespace Fabrik.API.Client.MvcHelpers
{
    internal static class ImageUrlBuilderExtensions
    {       
        public static void FromFabrik(this ImageUrlBuilder builder, ViewHelperConfiguration configuration)
        {
            builder.AddModifier(src => TransformUri(src, configuration.StorageServerUri, configuration.MediaServerUri));
        }

        private static string TransformUri(string sourceImageUri, string storageServerUri, string mediaServerUri)
        {           
            if (storageServerUri.IsNullOrEmpty() || mediaServerUri.IsNullOrEmpty())
            {
                return sourceImageUri;
            }

            if (sourceImageUri.StartsWith(storageServerUri, StringComparison.OrdinalIgnoreCase))
            {
                return sourceImageUri.Replace(storageServerUri, mediaServerUri);
            }

            return sourceImageUri;
        }
    }
}
