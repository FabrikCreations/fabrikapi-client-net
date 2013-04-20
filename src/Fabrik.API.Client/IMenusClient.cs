using Fabrik.CMS.API.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IMenusClient
    {
        Task<IEnumerable<Menu>> GetMenusAsync(int siteId, string name = null);
        Task<Menu> GetMenuAsync(int siteId, int menuId);
        Task<Menu> AddMenuAsync(int siteId, AddMenuCommand command);
        Task DeleteMenuAsync(int siteId, int menuId);

        Task AddMenuItemAsync(int siteId, int menuId, AddMenuItemCommand command);
        Task UpdateMenuItemAsync(int siteId, int menuId, int menuItemId, UpdateMenuItemCommand command);
        Task MoveMenuItemAsync(int siteId, int menuId, MoveMenuItemCommand command);
        Task DeleteMenuItemAsync(int siteId, int menuId, int id);
    }
}
