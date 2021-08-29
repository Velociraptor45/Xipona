using ProjectHermes.ShoppingList.Api.Core.Extensions;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models
{
    public static class StoreItemMother
    {
        public static StoreItemBuilder Initial()
        {
            return new StoreItemBuilder()
                .WithIsDeleted(false)
                .WithIsTemporary(false)
                .WithTemporaryId(null);
        }

        public static StoreItemBuilder InitialTemporary()
        {
            return new StoreItemBuilder()
                .WithIsDeleted(false)
                .WithIsTemporary(true)
                .WithoutItemCategoryId()
                .WithoutManufacturerId()
                .WithAvailabilities(StoreItemAvailabilityMother.Initial().Create().ToMonoList());
        }

        public static StoreItemBuilder InitialWithoutManufacturer()
        {
            return new StoreItemBuilder()
                .WithIsDeleted(false)
                .WithIsTemporary(false)
                .WithTemporaryId(null)
                .WithoutManufacturerId();
        }

        public static StoreItemBuilder Deleted()
        {
            return new StoreItemBuilder()
                .WithIsDeleted(true);
        }
    }
}