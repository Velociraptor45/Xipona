using ShoppingList.Database.Entities;
using ShoppingList.EntityModels.DataTransfer;
using ShoppingList.EntityModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingList.Database
{
    public interface IShoppingRepository
    {
        public StoreDto AddNewStore(StoreDto storeDto);
        public void RemoveStore(uint storeId);
        public void UpdateItemRelation(ItemDto itemDto, uint shoppingListId);
        public Entities.ShoppingList AddNewShoppingList(Entities.ShoppingList shoppingList);
        public Task<List<StoreDto>> GetAllStoresAsync();
        public List<ItemDto> GetAllItems();
        public void CompleteShoppingList(Entities.ShoppingList shoppingList);
        public Entities.ShoppingList GetActiveShoppingListByStoreId(uint storeId);
        public Entities.ShoppingList CreateNewShoppingList(uint storeId);
        public List<ItemDto> GetAllItemsOnShoppingList(uint shoppingListId);
        public void RemoveItemFromShoppingList(Item item, Entities.ShoppingList shoppingList);
        public List<ItemDto> SearchItems(string search);
        public void AddNewItemToShoppingList(ItemDto itemDto, uint shoppingListId);
        public void RemoveItemFromShoppingList(ItemDto itemDto, uint shoppingListId);
        public void RemoveItemsFromShoppingList(IEnumerable<ItemDto> itemDtos, uint shoppingListId);
        public void CreateNewItem(ItemDto itemDto);
        public void UpdateItem(ItemDto itemDto);
        public void ChangeItem(ItemDto itemDto);
        public void AddItemsToNewShoppingList(IEnumerable<ItemDto> itemDtos, uint storeId);
        public int GetItemCountInStore(uint storeId);
        public void UpdateStore(StoreDto storeDto);
        public void MarkStoreAsDeleted(uint storeId);
    }
}
