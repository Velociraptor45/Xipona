using ShoppingList.Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingList.Database
{
    interface IShoppingRepository
    {
        public Store AddNewStore(Store store);
        public void RemoveStore(Store store);
        public Item AddNewItem(Item item);
        public Entities.ShoppingList AddNewShoppingList(Entities.ShoppingList shoppingList);
        public Task<List<Store>> GetAllStoresAsync();
        public void CompleteShoppingList(Entities.ShoppingList shoppingList);
        public void AddItemToShoppingList(Item item, Entities.ShoppingList shoppingList);
        public void RemoveItemFromShoppingList(Item item, Entities.ShoppingList shoppingList);
        public List<Item> SearchItems(string search);
    }
}
