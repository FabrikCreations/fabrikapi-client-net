using Fabrik.API.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IMediaClient
    {
        Task<IEnumerable<MediaUploadResult>> UploadMediaAsync(string targetPath = null, params UploadMediaCommand[] commands);
    }
}
