using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Index.Search;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services
{
    public interface IShoppingListCommunicationService
    {
        Task<bool> AddItemToShoppingListAsync(AddItemToShoppingListRequest request, Func<Task> OnFailure,
            IAsyncRetryFragmentCreator fragmentCreator);

        Task<bool> AddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request,
            Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator);

        Task EnqueueAsync(IApiRequest request);

        Task<bool> FinishListAsync(FinishListRequest request, Func<Task> OnFailure,
            IAsyncRetryFragmentCreator fragmentCreator);

        void InitializeCommandQueue(ICommandQueueErrorHandler errorHandler);

        Task<ShoppingListRoot> LoadActiveShoppingListAsync(Guid storeId, Func<Task> OnFailure,
            IAsyncRetryFragmentCreator fragmentCreator);

        Task<IEnumerable<Store>> LoadAllActiveStoresAsync(Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator);

        Task<IEnumerable<SearchItemForShoppingListResult>> LoadItemSearchResultAsync(string input, Guid storeId,
            Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator);
    }
}