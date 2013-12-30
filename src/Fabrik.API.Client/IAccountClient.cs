using Fabrik.API.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    /// <summary>
    /// Defines a client for managing a Fabrik account
    /// </summary>
    public interface IAccountClient
    {
        /// <summary>
        /// Lists the sites available to the current account.
        /// </summary>
        Task<IEnumerable<Site>> ListSitesAsync();
    }
}
