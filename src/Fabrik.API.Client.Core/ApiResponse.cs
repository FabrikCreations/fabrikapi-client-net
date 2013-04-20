using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fabrik.API.Client.Core
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; private set; }
        public ApiError Error { get; set; }

        public ApiResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public bool IsSuccessful
        {
            get
            {
                return Error == null;
            }
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(HttpStatusCode statusCode)
            : base(statusCode) { }

        public T Content { get; set; }
    }
}
