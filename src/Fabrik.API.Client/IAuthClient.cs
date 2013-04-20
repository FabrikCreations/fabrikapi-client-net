using Fabrik.CMS.API.Common;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IAuthClient
    {
        Task<Identity> GetIdentityAsync();
        Task<SessionToken> GetSessionTokenAsync();
    }
}
