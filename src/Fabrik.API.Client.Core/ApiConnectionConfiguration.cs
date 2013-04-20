using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fabrik.API.Client.Core
{
    /// <summary>
    /// Represents the connection details for an API endpoint.
    /// </summary>
    internal class ApiConnectionConfiguration
    {
        /// <summary>
        /// The endpoint URI.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// A username for authenticating requests (Basic auth).
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// A password for authenticating requests (Basic auth).
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// An ApiKey for authentication requests (ApiKey auth)
        /// </summary>
        public string ApiKey { get; set; }
    }
}
