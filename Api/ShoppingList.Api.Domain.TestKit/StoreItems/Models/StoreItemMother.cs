using ProjectHermes.ShoppingList.Api.Core.Extensions;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models;

public static class StoreItemMother
{
    public static StoreItemBuilder Initial()
    {
        return new StoreItemBuilder()
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId()
            .AsItem();
    }

    public static StoreItemBuilder InitialWithTypes()
    {
        return new StoreItemBuilder()
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId();
    }

    public static StoreItemBuilder InitialTemporary()
    {
        return new StoreItemBuilder()
            .WithIsDeleted(false)
            .WithIsTemporary(true)
            .WithoutItemCategoryId()
            .WithoutManufacturerId()
            .WithAvailabilities(StoreItemAvailabilityMother.Initial().Create().ToMonoList())
            .AsItem();
    }

    public static StoreItemBuilder InitialWithoutManufacturer()
    {
        return new StoreItemBuilder()
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId()
            .WithoutManufacturerId()
            .AsItem();
    }

    public static StoreItemBuilder Deleted()
    {
        return new StoreItemBuilder()
            .WithIsDeleted(true)
            .AsItem();
    }
}