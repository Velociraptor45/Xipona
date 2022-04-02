using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Index.Search;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
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

        Task EnqueueAsync(IApiRequest request);

        Task FinishListAsync(Guid shoppingListId, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction);

        void InitializeCommandQueue(ICommandQueueErrorHandler errorHandler);

        Task<ShoppingListRoot> LoadActiveShoppingListAsync(Guid storeId, IAsyncRetryFragmentCreator fragmentCreator);

        Task<IEnumerable<Store>> LoadAllActiveStoresAsync(IAsyncRetryFragmentCreator fragmentCreator);

        Task<IEnumerable<SearchItemForShoppingListResult>> LoadItemSearchResultAsync(string input, Guid storeId,
            IAsyncRetryFragmentCreator fragmentCreator);
    }
}