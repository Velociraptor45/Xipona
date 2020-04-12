using Microsoft.EntityFrameworkCore;
using ShoppingList.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Database
{
    public class ShoppingRepository : IShoppingRepository
    {
        private ShoppingContext context;
        private Mapper.Mapper mapper = new Mapper.Mapper();

        public ShoppingRepository(DbContextOptions<ShoppingContext> dbContextOptions)
        {
            context = new ShoppingContext(dbContextOptions);
        }

        public void AddItemToShoppingList(Item item, Entities.ShoppingList shoppingList)
        {
            ItemOnShoppingList itemOnShoppingList = new ItemOnShoppingList
            {
                ItemId = item.ItemId,
                ShoppingListId = shoppingList.ShoppingListId
            };
            context.ItemOnShoppingList.Add(itemOnShoppingList);
            context.SaveChanges();
        }

        public Item AddNewItem(Item item)
        {
            context.Item.Add(item);
            context.SaveChanges();
            return item;
        }

        public Entities.ShoppingList AddNewShoppingList(Entities.ShoppingList shoppingList)
        {
            context.ShoppingList.Add(shoppingList);
            context.SaveChanges();
            return shoppingList;
        }

        public Store AddNewStore(Store store)
        {
            context.Store.Add(store);
            context.SaveChanges();
            return store;
        }

        public async Task<List<Store>> GetAllStoresAsync()
        {
            return await context.Store.ToListAsync();
        }

        public void CompleteShoppingList(Entities.ShoppingList shoppingList)
        {
            shoppingList.CompletionDate = DateTime.Now;
            context.ShoppingList.Update(shoppingList);
            context.SaveChanges();
        }

        public Entities.ShoppingList GetActiveShoppingListByStoreId(uint storeId)
        {
            return context.ShoppingList.AsNoTracking().FirstOrDefault(
                sl => sl.CompletionDate == null
                && sl.StoreId == storeId);
        }

        public Entities.ShoppingList CreateNewShoppingList(uint storeId)
        {
            if(GetActiveShoppingListByStoreId(storeId) != null)
            {
                throw new Exception("Uncompleted shopping list for this store");
            }

            var shoppingList = new Entities.ShoppingList
            {
                StoreId = storeId
            };

            context.ShoppingList.Add(shoppingList);
            context.SaveChanges();
            return shoppingList;
        }

        public List<EntityModels.ItemDto> GetAllItemsOnShoppingList(uint shoppingListId)
        {
            List<EntityModels.ItemDto> itemDtos = new List<EntityModels.ItemDto>();
            List<ItemOnShoppingList> relations = GetAllShoppingListToItemRelations(shoppingListId);
            var itemIdsToLoad = relations.Select(rel => rel.ItemId);
            List<Item> allShoppingListItems = context.Item.AsNoTracking()
                .Where(item => itemIdsToLoad.Contains(item.ItemId))
                .ToList();
            foreach (var relation in relations)
            {
                Entities.Item item = allShoppingListItems.First(it => it.ItemId == relation.ItemId);
                itemDtos.Add(mapper.ToItemDto(item, relation));
            }
            return itemDtos;
            
        }

        private List<Entities.ItemOnShoppingList> GetAllShoppingListToItemRelations(uint shoppingListId)
        {
            return context.ItemOnShoppingList.Where(rel => rel.ShoppingListId == shoppingListId).ToList();
        }

        public void RemoveItemFromShoppingList(Item item, Entities.ShoppingList shoppingList)
        {
            var relationToRemove = context.ItemOnShoppingList.AsNoTracking().First(
                iosl => iosl.ItemId == item.ItemId
                && iosl.ShoppingListId == shoppingList.ShoppingListId);
            if(relationToRemove != null)
                context.ItemOnShoppingList.Remove(relationToRemove);
        }

        public void RemoveStore(Store store)
        {
            context.Remove(store);
            context.SaveChanges();
        }

        public List<EntityModels.ItemDto> SearchItems(string search)
        {
            search = search.ToLower();
            return context.Item.AsNoTracking()
                .Where(item => item.Name.ToLower().Contains(search))
                .Select(item => mapper.ToItemDto(item, null))
                .ToList();
        }
    }
}
