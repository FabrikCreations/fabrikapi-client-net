using Fabrik.API.Client.Core;
using Fabrik.API.Core;
using Fabrik.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class MediaClient : IMediaClient
    {
        private readonly ApiClient api;
        private readonly int siteId;

        public MediaClient(ApiClient apiClient, int siteId)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            this.api = apiClient;
            this.siteId = siteId;
        }

        public async Task<IEnumerable<MediaUploadResult>> UploadMediaAsync(string targetPath = null, params UploadMediaCommand[] commands)
        {
            Ensure.Argument.NotNull(commands, "commands");
            Ensure.Argument.Is(commands.Length > 0, "You must provide at least one file to upload.");

            var formData = new MultipartFormDataContent();

            foreach (var command in commands)
            {
                Validator.ValidateObject(command, new ValidationContext(command));
                formData.Add(new StreamContent(command.FileStream), command.FileName, command.FileName,
                    new { CorrelationId = command.CorrelationId, PreserveFileName = command.PreserveFileName });
            }
           
            var request = api.CreateRequest(HttpMethod.Post, api.CreateRequestUri(GetMediaPath(targetPath)));
            request.Content = formData;

            var response = await api.HttpClient.SendAsync(request).ConfigureAwait(false);
            return await response.Content.ReadAsAsync<IEnumerable<MediaUploadResult>>().ConfigureAwait(false);
        }

        private string GetMediaPath(string targetPath)
        {
            return PathUtils.CombinePaths("sites/{0}/media".FormatWith(siteId), (targetPath ?? string.Empty).Trim('/'));
        }
    }
}
