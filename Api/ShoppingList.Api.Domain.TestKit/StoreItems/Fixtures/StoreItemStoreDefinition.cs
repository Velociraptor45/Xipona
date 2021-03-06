using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemStoreDefinition
    {
        public StoreItemStoreId Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<IStoreItemSection> Sections { get; set; }
    }
}