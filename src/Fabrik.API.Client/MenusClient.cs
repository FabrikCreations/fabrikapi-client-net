using Fabrik.API.Client.Core;
using Fabrik.CMS.API.Common;
using Fabrik.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrik.API.Client
{
    public class MenusClient : IMenusClient
    {
        private readonly ApiClient api;

        public MenusClient(ApiClient apiClient)
        {
            Ensure.Argument.NotNull(apiClient, "apiClient");
            api = apiClient;
        }

        public Task<IEnumerable<Menu>> GetMenusAsync(int siteId, string name = null)
        {
            return api.GetAsync<IEnumerable<Menu>>(GetMenusPath(siteId), new { name = name });
        }

        public Task<Menu> GetMenuAsync(int siteId, int menuId)
        {
            return api.GetAsync<Menu>(GetMenusPath(siteId, menuId));
        }

        public Task<Menu> AddMenuAsync(int siteId, AddMenuCommand command)
        {
            return api.PostAsync<AddMenuCommand, Menu>(GetMenusPath(siteId), command);
        }

        public async Task DeleteMenuAsync(int siteId, int menuId)
        {
            await api.DeleteAsync(GetMenusPath(siteId, menuId));
        }

        public async Task AddMenuItemAsync(int siteId, int menuId, AddMenuItemCommand command)
        {
            await api.PostAsync(GetMenuItemsPath(siteId, menuId), command);
        }

        public async Task UpdateMenuItemAsync(int siteId, int menuId, int menuItemId, UpdateMenuItemCommand command)
        {
            await api.PutAsync(GetMenuItemsPath(siteId, menuId, menuItemId), command);
        }

        public async Task MoveMenuItemAsync(int siteId, int menuId, MoveMenuItemCommand command)
        {
            await api.PatchAsync(GetMenuItemsPath(siteId, menuId), command);
        }

        public async Task DeleteMenuItemAsync(int siteId, int menuId, int id)
        {
            await api.DeleteAsync(GetMenuItemsPath(siteId, menuId, id));
        }

        private string GetMenusPath(int siteId, int? menuId = null)
        {
            var menusPath = "sites/{0}/menus".FormatWith(siteId);

            if (menuId.HasValue)
                menusPath += "/" + menuId;

            return menusPath;
        }

        private string GetMenuItemsPath(int siteId, int menuId, int? id = null)
        {
            var menuItemsPath = GetMenusPath(siteId, menuId) + "/items";

            if (id.HasValue)
                menuItemsPath += "/" + id;

            return menuItemsPath;
        }
    }
}
