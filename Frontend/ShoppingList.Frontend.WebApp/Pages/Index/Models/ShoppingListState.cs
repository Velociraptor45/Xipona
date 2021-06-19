using ProjectHermes.ShoppingList.Frontend.Models;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Models
{
    public class ShoppingListState
    {
        public Action StateChanged { get; set; }
        public Func<int, Task> ReloadRequestedAsync { get; set; }
        public ShoppingListRoot ShoppingList { get; private set; }
        public bool ItemsInBasketVisible { get; private set; }
        public bool ItemsInEditMode { get; private set; }

        public ShoppingListState(ShoppingListRoot shoppingList, bool itemsInBasketVisible, bool itemsInEditMode)
        {
            ShoppingList = shoppingList;
            ItemsInBasketVisible = itemsInBasketVisible;
            ItemsInEditMode = itemsInEditMode;
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

        public void AddItemToList(ShoppingListItem item, int sectionId)
        {
            ShoppingList.AddItem(item, sectionId);
            StateChanged?.Invoke();
        }

        public void ChangeList(ShoppingListRoot list)
        {
            ShoppingList = list;
            StateChanged?.Invoke();
        }

        public async Task ChangeStoreAsync(int storeId)
        {
            ResetItemEditMode();
            await RequestReloadAsync(storeId);
        }

        public async Task RequestReloadAsync()
        {
            await RequestReloadAsync(ShoppingList.Store.Id);
        }

        public async Task RequestReloadAsync(int storeId)
        {
            await ReloadRequestedAsync(storeId);
            StateChanged?.Invoke();
        }
    }
}