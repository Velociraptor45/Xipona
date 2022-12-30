using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services
{
    public interface IItemsApiService
    {
        Task LoadInitialPageStateAsync(ItemsState state, IAsyncRetryFragmentCreator fragmentCreator);

        Task LoadItemAsync(Guid itemId, IAsyncRetryFragmentCreator fragmentCreator, Action<Item> onSuccessAction);

        Task<IEnumerable<SearchItemByItemCategoryResult>> SearchItemsByItemCategoryAsync(Guid itemCategoryId);
    }
}