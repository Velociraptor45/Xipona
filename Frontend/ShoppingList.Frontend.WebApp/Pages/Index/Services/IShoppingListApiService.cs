using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Index.Search;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services
{
    public interface IShoppingListApiService
    {
        Task AddItemToShoppingListAsync(Guid shoppingListId, ItemId itemId, int quantity, Guid sectionId,
            IAsyncRetryFragmentCreator fragmentCreator, Func<Task> onSuccessAction);

        Task AddItemWithTypeToShoppingListAsync(Guid shoppingListId, Guid itemId, Guid itemTypeId,
            int quantity, Guid sectionId, IAsyncRetryFragmentCreator fragmentCreator, Func<Task> onSuccessAction);

        Task FinishListAsync(Guid shoppingListId, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction);

        void InitializeCommandQueue(ICommandQueueErrorHandler errorHandler);

        Task LoadActiveShoppingListAsync(Guid storeId, IAsyncRetryFragmentCreator fragmentCreator,
            Action<ShoppingListRoot> onSuccessAction);

        Task LoadAllActiveStoresAsync(IAsyncRetryFragmentCreator fragmentCreator,
            Func<IEnumerable<Store>, Task> onSuccessAction);

        Task LoadItemSearchResultAsync(string input, Guid storeId, IAsyncRetryFragmentCreator fragmentCreator,
            Action<IEnumerable<SearchItemForShoppingListResult>> onSuccessAction);

        Task CreateTemporaryItemOnShoppingListAsync(ShoppingListItem item, Guid shoppingListId,
            Guid storeId, SectionId sectionId);

        Task RemoveItemFromBasketAsync(Guid shoppingListId, ItemId itemId, Guid? itemTypeId);

        Task PutItemInBasketAsync(Guid shoppingListId, ItemId itemId, Guid? itemTypeId);

        Task ChangeItemQuantityOnShoppingListAsync(Guid shoppingListId, ItemId itemId, Guid? itemTypeId,
            float quantity);

        Task RemoveItemFromShoppingListAsync(Guid shoppingListId, ItemId itemId, Guid? itemTypeId);
    }
}