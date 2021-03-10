using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemStoreDefinition
    {
        public StoreItemStoreId Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<IStoreItemSection> Sections { get; set; }

        public static StoreItemStoreDefinition FromId(int id)
        {
            return FromId(new StoreItemStoreId(id));
        }

        public static StoreItemStoreDefinition FromId(StoreItemStoreId id)
        {
            return new StoreItemStoreDefinition
            {
                Id = id
            };
        }
    }
}