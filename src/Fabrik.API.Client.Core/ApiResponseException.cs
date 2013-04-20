using System;

namespace Fabrik.API.Client.Core
{
    [Serializable]
    public class ApiResponseException : Exception
    {
        public ApiError Error { get; private set; }

        public ApiResponseException(ApiError error)
            : base(error.ToString())
        {
            this.Error = error;
        }
    }
}
