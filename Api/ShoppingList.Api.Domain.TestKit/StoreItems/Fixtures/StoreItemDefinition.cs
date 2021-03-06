using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemDefinition
    {
        public StoreItemId Id { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsTemporary { get; set; }
        public IEnumerable<IStoreItemAvailability> Availabilities { get; set; }

        public static StoreItemDefinition FromTemporary(bool isTemporary)
        {
            return new StoreItemDefinition
            {
                IsTemporary = isTemporary
            };
        }
    }
}