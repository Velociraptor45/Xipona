using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;
using StoreModels = ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Models
{
    public class ShoppingListState
    {
        public CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("de-DE");
        public Action StateChanged { get; set; }
        public Func<Guid, Task> ReloadRequestedAsync { get; set; }
        public ShoppingListRoot ShoppingList { get; private set; }
        public AvailableStores AvailableStores { get; private set; }
        public bool ItemsInBasketVisible { get; private set; }
        public bool ItemsInEditMode { get; private set; }
        public StoreModels.Store SelectedStore => AvailableStores?.SelectedStore;

        public SearchBar SearchBar { get; } = new();

        public ShoppingListState(ShoppingListRoot shoppingList, bool itemsInBasketVisible, bool itemsInEditMode)
        {
            ShoppingList = shoppingList;
            ItemsInBasketVisible = itemsInBasketVisible;
            ItemsInEditMode = itemsInEditMode;
        }

        public void RegisterAvailableStores(AvailableStores stores)
        {
            AvailableStores = stores;
        }

        public void ToggleItemEditMode()
        {
            ItemsInEditMode = !ItemsInEditMode;
            StateChanged?.Invoke();
        }

        public void ResetItemEditMode()
        {
            ItemsInEditMode = false;
        }

        public void ToggleItemsInBasketVisible()
        {
            ItemsInBasketVisible = !ItemsInBasketVisible;
            StateChanged?.Invoke();
        }

        public void AddItem(ShoppingListItem item, Section section)
        {
            ShoppingList.AddItem(item, section);
            StateChanged?.Invoke();
        }

        public void ChangeList(ShoppingListRoot list)
        {
            ShoppingList = list;
            StateChanged?.Invoke();
        }

        public async Task ChangeStoreAsync(Guid storeId)
        {
            ResetItemEditMode();
            await RequestReloadAsync(storeId);
        }

        public async Task RequestReloadAsync()
        {
            await RequestReloadAsync(ShoppingList.Store.Id);
        }

        public async Task RequestReloadAsync(Guid storeId)
        {
            await ReloadRequestedAsync(storeId);
            StateChanged?.Invoke();
        }
    }
}