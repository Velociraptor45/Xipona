using ProjectHermes.ShoppingList.Frontend.Infrastructure.Error;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services
{
    public interface IShoppingListApiService
    {
        Task AddItemToShoppingListAsync(Guid shoppingListId, ShoppingListItemId itemId, int quantity, Guid sectionId,
            IAsyncRetryFragmentCreator fragmentCreator, Func<Task> onSuccessAction);

        Task AddItemWithTypeToShoppingListAsync(Guid shoppingListId, Guid itemId, Guid itemTypeId,
            int quantity, Guid sectionId, IAsyncRetryFragmentCreator fragmentCreator, Func<Task> onSuccessAction);

        Task FinishListAsync(Guid shoppingListId, DateTimeOffset? finishedAt,
            IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction);

        void InitializeCommandQueue(ICommandQueueErrorHandler errorHandler);

        Task LoadActiveShoppingListAsync(Guid storeId, IAsyncRetryFragmentCreator fragmentCreator,
            Action<ShoppingListRoot> onSuccessAction);

        Task LoadAllActiveStoresAsync(IAsyncRetryFragmentCreator fragmentCreator,
            Func<IEnumerable<Store>, Task> onSuccessAction);

        Task<IEnumerable<SearchItemForShoppingListResult>> LoadItemSearchResultAsync(string input, Guid storeId,
            IAsyncRetryFragmentCreator fragmentCreator);

        Task CreateTemporaryItemOnShoppingListAsync(ShoppingListItem item, Guid shoppingListId,
            Guid storeId, SectionId sectionId);

        Task RemoveItemFromBasketAsync(Guid shoppingListId, ShoppingListItemId itemId, Guid? itemTypeId);

        Task PutItemInBasketAsync(Guid shoppingListId, ShoppingListItemId itemId, Guid? itemTypeId);

        Task ChangeItemQuantityOnShoppingListAsync(Guid shoppingListId, ShoppingListItemId itemId, Guid? itemTypeId,
            float quantity);

        Task RemoveItemFromShoppingListAsync(Guid shoppingListId, ShoppingListItemId itemId, Guid? itemTypeId);

        Task<IEnumerable<QuantityType>> GetAllQuantityTypesAsync();

        Task UpdateItemPriceAsync(Guid itemId, Guid? itemTypeId, Guid storeId, float updatedPrice,
            Func<Task> onSuccessAction);
    }
}