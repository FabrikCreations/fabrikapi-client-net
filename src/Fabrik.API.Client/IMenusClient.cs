using Fabrik.API.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public interface IMenusClient
    {
        Task<IEnumerable<Menu>> ListMenusAsync(string name = null);
        Task<Menu> GetMenuAsync(int menuId);
        Task<Menu> AddMenuAsync(AddMenuCommand command);
        Task DeleteMenuAsync(int menuId);

        Task AddMenuItemAsync(int menuId, AddMenuItemCommand command);
        Task UpdateMenuItemAsync(int menuId, int menuItemId, UpdateMenuItemCommand command);
        Task MoveMenuItemAsync(int menuId, MoveMenuItemCommand command);
        Task DeleteMenuItemAsync(int menuId, int id);
    }
}
