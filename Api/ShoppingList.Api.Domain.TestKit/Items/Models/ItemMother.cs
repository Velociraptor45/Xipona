using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models.Factories;

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
            .WithoutPredecessorId()
            .AsItem();
    }

    public static ItemBuilder InitialWithTypes(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        var types = ItemTypeMother.Initial().WithIsDeleted(false).CreateMany(3);

        return builder
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId()
            .WithoutUpdatedOn()
            .WithoutPredecessorId()
            .WithTypes(new ItemTypes(types, new ItemTypeFactoryMock(MockBehavior.Strict).Object));
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
            .WithoutPredecessorId()
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
            .WithoutPredecessorId()
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