using Fabrik.CMS.API.Common;
using Fabrik.Common;

namespace Fabrik.API.Client
{
    public static class MediaExtensions
    {
        /// <summary>
        /// Converts a <see cref="MediaUploadResult"/> into a <see cref="AddMediaCommand"/>.
        /// </summary>
        public static AddMediaCommand ToAddMediaCommand(this MediaUploadResult uploadResult, MediaType? mediaType = null, string title = null, string description = null)
        {
            Ensure.Argument.NotNull(uploadResult, "uploadResult");

            return new AddMediaCommand
            {
                Uri = uploadResult.Uri,
                SizeInBytes = uploadResult.SizeInBytes,
                Title = title,
                Description = description,
                MediaType = mediaType
            };
        }
    }
}
