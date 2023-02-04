using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Threading.Tasks;
using StoreModels = ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Stores.Services
{
    public interface IStoresApiService
    {
        Task CreateStoreAsync(StoreModels.Store store, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction);

        //Task LoadStores(IAsyncRetryFragmentCreator fragmentCreator, Action<IEnumerable<StoreModels.Store>> onSuccessAction);

        Task SaveStoreAsync(StoreModels.Store store, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction);
    }
}