using ProjectHermes.ShoppingList.Api.Core.Extensions;

namespace ShoppingList.Api.Domain.TestKit.Items.Models;

public static class ItemMother
{
    public static ItemBuilder Initial()
    {
        return new ItemBuilder()
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId()
            .AsItem();
    }

    public static ItemBuilder InitialWithTypes()
    {
        return new ItemBuilder()
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId();
    }

    public static ItemBuilder InitialTemporary()
    {
        return new ItemBuilder()
            .WithIsDeleted(false)
            .WithIsTemporary(true)
            .WithoutItemCategoryId()
            .WithoutManufacturerId()
            .WithAvailabilities(StoreItemAvailabilityMother.Initial().Create().ToMonoList())
            .AsItem();
    }

    public static ItemBuilder InitialWithoutManufacturer()
    {
        return new ItemBuilder()
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId()
            .WithoutManufacturerId()
            .AsItem();
    }

    public static ItemBuilder Deleted()
    {
        return new ItemBuilder()
            .WithIsDeleted(true)
            .AsItem();
    }
}