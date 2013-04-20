using Fabrik.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Fabrik.API.Client.Core
{
    [Serializable]
    public class ApiError : Dictionary<string, object>
    {
        private const string MessageKey = "Message";
        private const string MessageDetailKey = "MessageDetail";
        private const string ModelStateKey = "ModelState";
        private const string ExceptionMessageKey = "ExceptionMessage";
        private const string ExceptionTypeKey = "ExceptionType";
        private const string StackTraceKey = "StackTrace";
        private const string InnerExceptionKey = "InnerException";

        /// <summary>
        /// The HTTP Status Code of the error
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        public ApiError(HttpStatusCode statusCode, IDictionary<string, object> httpError) :
            base(httpError, StringComparer.OrdinalIgnoreCase)
        {
            StatusCode = statusCode;
        }

        public ApiError(HttpStatusCode statusCode)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// The high-level, user-visible message explaining the cause of the error. Information carried in this field 
        /// should be considered public in that it will go over the wire regardless of the <see cref="IncludeErrorDetailPolicy"/>. 
        /// As a result care should be taken not to disclose sensitive information about the server or the application.
        /// </summary>
        public string Message
        {
            get { return GetPropertyValue<String>(MessageKey); }
        }

        /// <summary>
        /// The <see cref="ModelState"/> containing information about the errors that occurred during model binding.
        /// </summary>
        /// <remarks>
        /// The inclusion of <see cref="System.Exception"/> information carried in the <see cref="ModelState"/> is
        /// controlled by the <see cref="IncludeErrorDetailPolicy"/>. All other information in the <see cref="ModelState"/>
        /// should be considered public in that it will go over the wire. As a result care should be taken not to 
        /// disclose sensitive information about the server or the application.
        /// </remarks>
        public ModelStateDictionary ModelState
        {
            get
            {
                object obj;
                if (this.TryGetValue(ModelStateKey, out obj))
                {
                    var jtoken = obj as JToken;

                    if (jtoken != null)
                    {
                        return GetModelStateFromJToken(jtoken);
                    }

                    // obj is HttpError
                    return GetModelStateFromHttpError((HttpError)obj);
                }

                return null;
            }
        }

        /// <summary>
        /// A detailed description of the error intended for the developer to understand exactly what failed.
        /// </summary>
        /// <remarks>
        /// The inclusion of this field is controlled by the <see cref="IncludeErrorDetailPolicy"/>. The 
        /// field is expected to contain information about the server or the application that should not 
        /// be disclosed broadly.
        /// </remarks>
        public string MessageDetail
        {
            get { return GetPropertyValue<String>(MessageDetailKey); }
        }

        /// <summary>
        /// The message of the <see cref="System.Exception"/> if available.
        /// </summary>
        /// <remarks>
        /// The inclusion of this field is controlled by the <see cref="IncludeErrorDetailPolicy"/>. The 
        /// field is expected to contain information about the server or the application that should not 
        /// be disclosed broadly.
        /// </remarks>
        public string ExceptionMessage
        {
            get { return GetPropertyValue<String>(ExceptionMessageKey); }
        }

        /// <summary>
        /// The type of the <see cref="System.Exception"/> if available.
        /// </summary>
        /// <remarks>
        /// The inclusion of this field is controlled by the <see cref="IncludeErrorDetailPolicy"/>. The 
        /// field is expected to contain information about the server or the application that should not 
        /// be disclosed broadly.
        /// </remarks>
        public string ExceptionType
        {
            get { return GetPropertyValue<String>(ExceptionTypeKey); }
        }

        /// <summary>
        /// The stack trace information associated with this instance if available.
        /// </summary>
        /// <remarks>
        /// The inclusion of this field is controlled by the <see cref="IncludeErrorDetailPolicy"/>. The 
        /// field is expected to contain information about the server or the application that should not 
        /// be disclosed broadly.
        /// </remarks>
        public string StackTrace
        {
            get { return GetPropertyValue<String>(StackTraceKey); }
        }

        /// <summary>
        /// The inner <see cref="System.Exception"/> associated with this instance if available.
        /// </summary>
        /// <remarks>
        /// The inclusion of this field is controlled by the <see cref="IncludeErrorDetailPolicy"/>. The 
        /// field is expected to contain information about the server or the application that should not 
        /// be disclosed broadly.
        /// </remarks>
        public HttpError InnerException
        {
            get { return GetPropertyValue<HttpError>(InnerExceptionKey); }
        }

        /// <summary>
        /// Gets a particular property value from this error instance.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="key">The name of the error property.</param>
        /// <returns>The value of the error property.</returns>
        public TValue GetPropertyValue<TValue>(string key)
        {
            object value;
            if (this.TryGetValue(key, out value))
            {
                return (TValue)value;
            }
            return default(TValue);
        }

        private static ModelStateDictionary GetModelStateFromJToken(JToken container)
        {
            if (container == null)
            {
                return null;
            }

            var modelState = container.ToObject<Dictionary<string, string[]>>();
            return GetModelStateFromDictionary(modelState);
        }

        private static ModelStateDictionary GetModelStateFromHttpError(HttpError error)
        {
            if (error == null)
            {
                return null;
            }

            return GetModelStateFromDictionary(error.ToDictionary(x => x.Key, x => x.Value as string[]));
        }

        private static ModelStateDictionary GetModelStateFromDictionary(IDictionary<string, string[]> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            var modelState = new ModelStateDictionary();

            foreach (var error in dictionary)
            {
                foreach (var message in error.Value)
                {
                    modelState.AddModelError(error.Key, message);
                }
            }

            return modelState;
        }

        public override string ToString()
        {
            return "{0} {1}: {2}".FormatWith((int)StatusCode, StatusCode, Message ?? "Api Error");
        }
    }
}
