﻿using Microsoft.EntityFrameworkCore;
using ShoppingList.Database.Entities;
using ShoppingList.EntityModels;
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

        private void DetachAllEntries()
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }

        public void UpdateItemRelation(EntityModels.ItemDto itemDto, uint shoppingListId)
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
            relation.IsInShoppingBasket = itemDto.IsInShoppingBasket;

            context.ItemOnShoppingList.Update(relation);
            context.SaveChanges();
            context.Entry(relation).State = EntityState.Detached;
        }

        public Entities.ShoppingList AddNewShoppingList(Entities.ShoppingList shoppingList)
        {
            context.ShoppingList.Add(shoppingList);
            context.SaveChanges();
            context.Entry(shoppingList).State = EntityState.Detached;
            return shoppingList;
        }

        public Store AddNewStore(Store store)
        {
            context.Store.Add(store);
            context.SaveChanges();
            context.Entry(store).State = EntityState.Detached;
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
            context.Entry(shoppingList).State = EntityState.Detached;
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
            context.Entry(shoppingList).State = EntityState.Detached;
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
                context.Entry(relationToRemove).State = EntityState.Detached;
            }
        }

        public void RemoveStore(Store store)
        {
            context.Remove(store);
            context.SaveChanges();
            context.Entry(store).State = EntityState.Detached;
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
                    Quantity = itemDto.Quantity,
                    IsInShoppingBasket = false
                };
                context.ItemOnShoppingList.Add(reference);
                context.SaveChanges();
                context.Entry(reference).State = EntityState.Detached;
            }
            else
            {
                //TODO replace with fitting exception
                throw new Exception("Item already on shopping list");
            }
        }

        public void AddItemsToNewShoppingList(IEnumerable<EntityModels.ItemDto> itemDtos, uint storeId)
        {
            Entities.ShoppingList shoppingList = new Entities.ShoppingList()
            {
                StoreId = storeId
            };
            shoppingList = AddNewShoppingList(shoppingList);

            foreach (var itemDto in itemDtos)
            {
                ItemOnShoppingList reference = new ItemOnShoppingList
                {
                    ShoppingListId = shoppingList.ShoppingListId,
                    ItemId = itemDto.Id,
                    Quantity = itemDto.Quantity,
                    IsInShoppingBasket = false
                };
                context.ItemOnShoppingList.Add(reference);
            }
            context.SaveChanges();
            DetachAllEntries();
        }

        /// <summary>
        /// Removes an item from the shopping list with the given id
        /// </summary>
        public void RemoveItemFromShoppingList(ItemDto itemDto, uint shoppingListId)
        {
            RemoveItemsFromShoppingList(new List<ItemDto> { itemDto }, shoppingListId);
        }

        /// <summary>
        /// Removes given items from the shopping list with the given id
        /// </summary>
        public void RemoveItemsFromShoppingList(IEnumerable<ItemDto> itemDtos, uint shoppingListId)
        {
            var itemIds = itemDtos.Select(item => item.Id);
            var references = context.ItemOnShoppingList.AsNoTracking()
                    .Where(r => r.ShoppingListId == shoppingListId
                        && itemIds.Contains(r.ItemId))
                    .ToList();

            foreach (var itemDto in itemDtos)
            {
                var reference = references.FirstOrDefault(r => r.ItemId == itemDto.Id);

                if (reference != null)
                {
                    context.ItemOnShoppingList.Remove(reference);
                }
            }
            context.SaveChanges();
            DetachAllEntries();
        }

        /// <summary>
        /// Create a new item
        /// </summary>
        public void CreateNewItem(ItemDto itemDto)
        {
            Item item = mapper.ToItem(itemDto);
            item.Active = true;
            context.Item.Add(item);
            context.SaveChanges();
            context.Entry(item).State = EntityState.Detached;
        }

        /// <summary>
        /// Update an existig item's data, e.g. when the store changes the price
        /// -> a new databse entry will be created to be able to calculate
        /// old shopping lists with the old price
        /// </summary>
        public void UpdateItem(ItemDto itemDto)
        {
            Item oldItem = context.Item.AsNoTracking().FirstOrDefault(i => i.ItemId == itemDto.Id);
            if(oldItem != null)
            {
                oldItem.Active = false;
                context.Item.Update(oldItem);
                context.Entry(oldItem).State = EntityState.Detached;
            }
            Item item = mapper.ToItem(itemDto);
            item.ItemId = default;

            context.Item.Add(item);
            context.SaveChanges();
            context.Entry(item).State = EntityState.Detached;
        }

        /// <summary>
        /// Change an existing item' data, e.g. typo in the item name
        /// -> no new database entry will be created
        /// </summary>
        public void ChangeItem(ItemDto itemDto)
        {
            Item item = mapper.ToItem(itemDto);
            context.Item.Update(item);
            context.SaveChanges();
            context.Entry(item).State = EntityState.Detached;
        }
    }
}
