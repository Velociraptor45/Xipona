using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.Index
{
    public class ShoppingListState
    {
        public Action StateChanged { get; set; }
        public Func<Task> ReloadRequestedAsync { get; set; }
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
            StateChanged();
        }

        public void ResetItemEditMode()
        {
            ItemsInEditMode = false;
        }

        public void ToggleItemsInBasketVisible()
        {
            ItemsInBasketVisible = !ItemsInBasketVisible;
            StateChanged();
        }

        public void AddItemToList(ShoppingListItem item, int sectionId)
        {
            ShoppingList.AddItem(item, sectionId);
            StateChanged();
        }

        public void ChangeList(ShoppingListRoot list)
        {
            ShoppingList = list;
            StateChanged();
        }

        public async Task RequestReloadAsync()
        {
            await ReloadRequestedAsync();
            StateChanged();
        }
    }
}