using ProjectHermes.ShoppingList.Frontend.Models.Items;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services.ItemEditor
{
    public interface IItemEditorApiService
    {
        Task UpdateItemWithTypesAsync(StoreItem item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task UpdateItemAsync(StoreItem item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task CreateItemWithTypesAsync(StoreItem item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task CreateItemAsync(StoreItem item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task ModifyItemWithTypesAsync(StoreItem item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task ModifyItemAsync(StoreItem item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task MakeTemporaryItemPermanentAsync(StoreItem item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task DeleteItemAsync(Guid itemId, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task CreateItemCategoryAsync(string name, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction);

        Task CreateManufacturerAsync(string name, IAsyncRetryFragmentCreator fragmentCreator,
            Func<Task> onSuccessAction);
    }
}