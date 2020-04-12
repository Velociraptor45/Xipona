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
        }

        public Item AddNewItem(Item item)
        {
            return context.Item.Add(item).Entity;
        }

        public Entities.ShoppingList AddNewShoppingList(Entities.ShoppingList shoppingList)
        {
            return context.ShoppingList.Add(shoppingList).Entity;
        }

        public Store AddNewStore(Store store)
        {
            return context.Store.Add(store).Entity;
        }

        public async Task<List<Store>> GetAllStoresAsync()
        {
            return await context.Store.ToListAsync();
        }

        public void CompleteShoppingList(Entities.ShoppingList shoppingList)
        {
            shoppingList.CompletionDate = DateTime.Now;
            context.ShoppingList.Update(shoppingList);
        }

        public void RemoveItemFromShoppingList(Item item, Entities.ShoppingList shoppingList)
        {
            var relationToRemove = context.ItemOnShoppingList.First(
                iosl => iosl.ItemId == item.ItemId
                && iosl.ShoppingListId == shoppingList.ShoppingListId);
            if(relationToRemove != null)
                context.ItemOnShoppingList.Remove(relationToRemove);
        }

        public void RemoveStore(Store store)
        {
            context.Remove(store);
        }
    }
}
