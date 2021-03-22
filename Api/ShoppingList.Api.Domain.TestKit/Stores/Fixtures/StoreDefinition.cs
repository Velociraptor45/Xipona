using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.Stores.Fixtures
{
    public class StoreDefinition
    {
        public StoreId Id { get; set; }
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }
        public IEnumerable<IStoreSection> Sections { get; set; }

        public StoreDefinition Clone()
        {
            return new StoreDefinition
            {
                Id = Id,
                Name = Name,
                IsDeleted = IsDeleted,
                Sections = Sections
            };
        }

        public static StoreDefinition FromId(StoreId id)
        {
            return new StoreDefinition()
            {
                Id = id
            };
        }
    }
}