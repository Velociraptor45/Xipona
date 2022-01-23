using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models
{
    public class ItemTypeBuilder : DomainTestBuilderBase<ItemType>
    {
        public ItemTypeBuilder()
        {
        }

        public ItemTypeBuilder(IItemType itemType)
        {
            WithId(itemType.Id);
            WithName(itemType.Name);
            WithAvailabilities(itemType.Availabilities);
        }

        public ItemTypeBuilder WithId(ItemTypeId id)
        {
            FillContructorWith("id", id);
            return this;
        }

        public ItemTypeBuilder WithName(string name)
        {
            FillContructorWith("name", name);
            return this;
        }

        public ItemTypeBuilder WithAvailabilities(IEnumerable<IStoreItemAvailability> availabilities)
        {
            FillContructorWith("availabilities", availabilities);
            return this;
        }
    }
}