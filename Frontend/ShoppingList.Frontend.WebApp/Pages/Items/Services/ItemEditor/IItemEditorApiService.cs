using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Services.ItemEditor
{
    public interface IItemEditorApiService
    {
        Task UpdateItemWithTypesAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task UpdateItemAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task CreateItemWithTypesAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task CreateItemAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task ModifyItemWithTypesAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task ModifyItemAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task MakeTemporaryItemPermanentAsync(Item item, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task DeleteItemAsync(Guid itemId, IAsyncRetryFragmentCreator fragmentCreator,
            Action onSuccessAction);

        Task<ItemCategory> CreateItemCategoryAsync(string name, IAsyncRetryFragmentCreator fragmentCreator);

        Task<Manufacturer> CreateManufacturerAsync(string name, IAsyncRetryFragmentCreator fragmentCreator);
    }
}