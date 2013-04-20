﻿using System.Threading.Tasks;

namespace Fabrik.API.Client.Core
{
    /// <summary>
    /// Extensions for <see cref="ApiClient"/>.
    /// </summary>
    public static class ApiClientExtensions
    {
        public static async Task<TResult> GetAsync<TResult>(this ApiClient client, string relativePath, object parameters = null)
        {
            var response = await client.TryGetAsync<TResult>(relativePath, parameters).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new ApiResponseException(response.Error);
            }

            return response.Content;
        }

        public static async Task PostAsync<TCommand>(this ApiClient client, string relativePath, TCommand command)
        {
            var response = await client.TryPostAsync<TCommand>(relativePath, command).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new ApiResponseException(response.Error);
            }
        }

        public static async Task<TResult> PostAsync<TCommand, TResult>(this ApiClient client, string relativePath, TCommand command)
        {
            var response = await client.TryPostAsync<TCommand, TResult>(relativePath, command).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new ApiResponseException(response.Error);
            }

            return response.Content;
        }

        public static async Task PutAsync<TCommand>(this ApiClient client, string relativePath, TCommand command)
        {
            var response = await client.TryPutAsync<TCommand>(relativePath, command).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new ApiResponseException(response.Error);
            }
        }

        public static async Task DeleteAsync(this ApiClient client, string relativePath)
        {
            var response = await client.TryDeleteAsync(relativePath).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new ApiResponseException(response.Error);
            }
        }

        public static async Task PatchAsync<TCommand>(this ApiClient client, string relativePath, TCommand command)
        {
            var response = await client.TryPatchAsync<TCommand>(relativePath, command).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new ApiResponseException(response.Error);
            }
        }
    }
}
