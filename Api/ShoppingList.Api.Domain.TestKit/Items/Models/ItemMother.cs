using ProjectHermes.ShoppingList.Api.Core.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public static class ItemMother
{
    public static ItemBuilder Initial(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        return builder
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId()
            .WithoutUpdatedOn()
            .AsItem();
    }

    public static ItemBuilder InitialWithTypes(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        return builder
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId()
            .WithoutUpdatedOn();
    }

    public static ItemBuilder InitialTemporary(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        return builder
            .WithIsDeleted(false)
            .WithIsTemporary(true)
            .WithoutItemCategoryId()
            .WithoutManufacturerId()
            .WithAvailabilities(ItemAvailabilityMother.Initial().Create().ToMonoList())
            .WithoutUpdatedOn()
            .AsItem();
    }

    public static ItemBuilder InitialWithoutManufacturer(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        return builder
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId()
            .WithoutManufacturerId()
            .WithoutUpdatedOn()
            .AsItem();
    }

    public static ItemBuilder Deleted(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        return builder
            .WithIsDeleted(true)
            .AsItem();
    }
}