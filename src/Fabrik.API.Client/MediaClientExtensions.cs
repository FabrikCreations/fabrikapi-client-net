using Fabrik.CMS.API.Common;
using Fabrik.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public static class MediaClientExtensions
    {
        public static Task<IEnumerable<MediaUploadResult>> UploadMediaAsync(this IMediaClient mediaClient, int siteId, params UploadMediaCommand[] commands)
        {
            return mediaClient.UploadMediaAsync(siteId, null, commands);
        }
        
        public static Task<IEnumerable<MediaUploadResult>> UploadMediaAsync(this IMediaClient mediaClient, int siteId, params string[] filePaths)
        {
            return mediaClient.UploadMediaAsync(siteId, null, filePaths);
        }
        
        public static Task<IEnumerable<MediaUploadResult>> UploadMediaAsync(this IMediaClient mediaClient, int siteId, string targetPath, params string[] filePaths)
        {
            Ensure.Argument.NotNull(filePaths, "filePaths");
            Ensure.Argument.Is(filePaths.Length > 0, "You must provide at least one file to upload.");

            var uploads = new List<UploadMediaCommand>();

            foreach (var file in filePaths.Select(path => new FileInfo(path)))
            {
                if (!file.Exists)
                {
                    throw new FileNotFoundException("File not found.", file.FullName);
                }

                var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 1024, useAsync: true);
                uploads.Add(new UploadMediaCommand { 
                    FileName = file.Name, 
                    FileStream = fileStream 
                });
            }

            return mediaClient.UploadMediaAsync(siteId, targetPath, uploads.ToArray());
        }
    }
}
