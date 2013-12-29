using Fabrik.API.Client.Core;
using Fabrik.API.Core;
using Fabrik.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class MenusClient : IMenusClient
    {
        private readonly ApiClient api;
        private readonly int siteId;

        public MenusClient(ApiClient apiClient, int siteId)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            this.api = apiClient;
            this.siteId = siteId;
        }

        public Task<IEnumerable<Menu>> GetMenusAsync(string name = null)
        {
            return api.GetAsync<IEnumerable<Menu>>(GetMenusPath(), new { name = name });
        }

        public Task<Menu> GetMenuAsync(int menuId)
        {
            return api.GetAsync<Menu>(GetMenusPath(menuId));
        }

        public Task<Menu> AddMenuAsync(AddMenuCommand command)
        {
            return api.PostAsync<AddMenuCommand, Menu>(GetMenusPath(), command);
        }

        public async Task DeleteMenuAsync(int menuId)
        {
            await api.DeleteAsync(GetMenusPath(menuId));
        }

        public async Task AddMenuItemAsync(int menuId, AddMenuItemCommand command)
        {
            await api.PostAsync(GetMenuItemsPath(menuId), command);
        }

        public async Task UpdateMenuItemAsync(int menuId, int menuItemId, UpdateMenuItemCommand command)
        {
            await api.PutAsync(GetMenuItemsPath(menuId, menuItemId), command);
        }

        public async Task MoveMenuItemAsync(int menuId, MoveMenuItemCommand command)
        {
            await api.PatchAsync(GetMenuItemsPath(menuId), command);
        }

        public async Task DeleteMenuItemAsync(int menuId, int id)
        {
            await api.DeleteAsync(GetMenuItemsPath(menuId, id));
        }

        private string GetMenusPath(int? menuId = null)
        {
            var menusPath = "sites/{0}/menus".FormatWith(siteId);

            if (menuId.HasValue)
                menusPath += "/" + menuId;

            return menusPath;
        }

        private string GetMenuItemsPath(int menuId, int? id = null)
        {
            var menuItemsPath = GetMenusPath(menuId) + "/items";

            if (id.HasValue)
                menuItemsPath += "/" + id;

            return menuItemsPath;
        }
    }
}
