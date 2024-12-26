using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models.Factories;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;

public static class ItemMother
{
    public static ItemBuilder Initial(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        return EnrichAsActiveWithoutPredecessor(builder)
            .AsItem();
    }

    public static ItemBuilder InitialWithTypes(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        var types = ItemTypeMother.Initial().CreateMany(3);

        return EnrichAsActiveWithoutPredecessor(builder)
            .WithTypes(new ItemTypes(types, new ItemTypeFactoryMock(MockBehavior.Strict).Object));
    }

    public static ItemBuilder InitialWithType(IItemType itemType)
    {
        var types = new ItemTypes(
            [itemType],
            new ItemTypeFactoryMock(MockBehavior.Strict).Object);

        return EnrichAsActiveWithoutPredecessor(new ItemBuilder())
            .WithTypes(types);
    }

    public static ItemBuilder InitialTemporary(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        return builder
            .WithIsDeleted(false)
            .WithIsTemporary(true)
            .WithoutItemCategoryId()
            .WithoutManufacturerId()
            .WithAvailabilities([ItemAvailabilityMother.Initial().Create()])
            .WithoutUpdatedOn()
            .WithoutPredecessorId()
            .AsItem();
    }

    public static ItemBuilder InitialWithoutManufacturer(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        return EnrichAsActiveWithoutPredecessor(builder)
            .WithoutManufacturerId()
            .AsItem();
    }

    public static ItemBuilder WithTypesWithPredecessor()
    {
        ItemBuilder builder = new();
        var types = ItemTypeMother.WithPredecessor().CreateMany(3);

        return EnrichAsActiveWithoutPredecessor(builder)
            .WithTypes(new ItemTypes(types, new ItemTypeFactoryMock(MockBehavior.Strict).Object));
    }

    public static ItemBuilder Deleted(ItemBuilder? builder = null)
    {
        builder ??= new ItemBuilder();

        return builder
            .WithIsDeleted(true)
            .AsItem();
    }

    private static ItemBuilder EnrichAsActiveWithoutPredecessor(ItemBuilder builder)
    {
        return builder
            .WithIsDeleted(false)
            .WithIsTemporary(false)
            .WithoutTemporaryId()
            .WithoutUpdatedOn()
            .WithoutPredecessorId();
    }
}