using ProjectHermes.ShoppingList.Frontend.Infrastructure.Error;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services
{
    public interface IShoppingListApiService
    {
        Task FinishListAsync(Guid shoppingListId, DateTimeOffset? finishedAt,
            IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction);

        void InitializeCommandQueue(ICommandQueueErrorHandler errorHandler);

        Task LoadActiveShoppingListAsync(Guid storeId, IAsyncRetryFragmentCreator fragmentCreator,
            Action<ShoppingListRoot> onSuccessAction);
    }
}