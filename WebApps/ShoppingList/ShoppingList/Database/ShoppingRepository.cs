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

        public void UpdateItemQuantity(EntityModels.ItemDto itemDto, uint shoppingListId)
        {
            var relation = context.ItemOnShoppingList.AsNoTracking()
                .FirstOrDefault(item => item.ItemId == itemDto.Id
                && item.ShoppingListId == shoppingListId);

            if(relation == null)
            {
                //TODO replace with fitting exception
                throw new Exception("No relation for this item to the given shoppingListId found");
            }

            relation.Quantity = itemDto.Quantity;

            context.ItemOnShoppingList.Update(relation);
            context.SaveChanges();
            context.Entry(relation).State = EntityState.Detached;
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
                //TODO replace with fitting exception
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
            //TODO refactor
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
            return context.ItemOnShoppingList.AsNoTracking()
                .Where(rel => rel.ShoppingListId == shoppingListId)
                .ToList();
        }

        public void RemoveItemFromShoppingList(Item item, Entities.ShoppingList shoppingList)
        {
            var relationToRemove = context.ItemOnShoppingList.AsNoTracking().First(
                iosl => iosl.ItemId == item.ItemId
                && iosl.ShoppingListId == shoppingList.ShoppingListId);
            if (relationToRemove != null)
            {
                context.ItemOnShoppingList.Remove(relationToRemove);
                context.SaveChanges();
            }
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

        /// <summary>
        /// Adds an item to the shopping list with the given id
        /// </summary>
        /// <exception cref="Exception">Item's already on the given shopping list</exception>
        public void AddNewItemToShoppingList(EntityModels.ItemDto itemDto, uint shoppingListId)
        {
            var existingReference = context.ItemOnShoppingList.AsNoTracking()
                .FirstOrDefault(r => r.ShoppingListId == shoppingListId
                    && r.ItemId == itemDto.Id);
            if (existingReference == null)
            {
                ItemOnShoppingList reference = new ItemOnShoppingList
                {
                    ShoppingListId = shoppingListId,
                    ItemId = itemDto.Id,
                    Quantity = itemDto.Quantity
                };
                context.ItemOnShoppingList.Add(reference);
                context.SaveChanges();
            }
            else
            {
                //TODO replace with fitting exception
                throw new Exception("Item already on shopping list");
            }
        }

        /// <summary>
        /// Removes an item from the shopping list with the given id
        /// </summary>
        /// <exception cref="Exception">Item's not on the given shopping list</exception>
        public void RemoveItemFromShoppingList(EntityModels.ItemDto itemDto, uint shoppingListId)
        {
            var reference = context.ItemOnShoppingList.AsNoTracking()
                .FirstOrDefault(r => r.ShoppingListId == shoppingListId
                    && r.ItemId == itemDto.Id);
            
            if(reference == null)
            {
                //TODO replace with fitting exception
                throw new Exception("Item not on shopping list");
            }
            else
            {
                context.ItemOnShoppingList.Remove(reference);
                context.SaveChanges();
                context.Entry(reference).State = EntityState.Detached;
            }
        }
    }
}
