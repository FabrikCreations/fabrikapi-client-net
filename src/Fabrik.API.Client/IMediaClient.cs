using Fabrik.CMS.API.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IMediaClient
    {
        Task<IEnumerable<MediaUploadResult>> UploadMediaAsync(int siteId, string targetPath, params UploadMediaCommand[] commands);
    }
}
