using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Stores.Services
{
    public interface IStoresApiService
    {
        Task CreateStoreAsync(Store store, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction);

        Task LoadStores(IAsyncRetryFragmentCreator fragmentCreator, Action<IEnumerable<Store>> onSuccessAction);

        Task SaveStoreAsync(Store store, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction);
    }
}