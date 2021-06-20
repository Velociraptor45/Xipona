using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Stores.Services
{
    public interface IStoresPageCommunicationService
    {
        Task<bool> CreateStoreAsync(CreateStoreRequest request, Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator);
        Task<IEnumerable<Store>> LoadStores(Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator);
        Task<bool> SaveStoreAsync(ModifyStoreRequest request, Func<Task> OnFailure, IAsyncRetryFragmentCreator fragmentCreator);
    }
}