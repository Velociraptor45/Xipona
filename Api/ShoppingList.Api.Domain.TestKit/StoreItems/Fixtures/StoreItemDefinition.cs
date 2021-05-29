using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemDefinition
    {
        private ItemCategoryId itemCategoryId;
        private ManufacturerId manufacturerId;

        public ItemId Id { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsTemporary { get; set; }

        public ItemCategoryId ItemCategoryId
        {
            get => itemCategoryId;
            set
            {
                UseItemCategoryId = true;
                itemCategoryId = value;
            }
        }

        public ManufacturerId ManufacturerId
        {
            get => manufacturerId;
            set
            {
                UseManufacturerId = true;
                manufacturerId = value;
            }
        }

        public IEnumerable<IStoreItemAvailability> Availabilities { get; set; }

        public bool UseItemCategoryId { get; private set; } = false;
        public bool UseManufacturerId { get; private set; } = false;

        public static StoreItemDefinition FromTemporary(bool isTemporary)
        {
            return new StoreItemDefinition
            {
                IsTemporary = isTemporary
            };
        }

        public static StoreItemDefinition FromId(ItemId id)
        {
            return new StoreItemDefinition
            {
                Id = id
            };
        }
    }
}