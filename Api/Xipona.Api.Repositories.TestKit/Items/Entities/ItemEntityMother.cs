using Xipona.Api.Repositories.Items.Entities;

namespace Xipona.Api.Repositories.TestKit.Items.Entities;

public static class ItemEntityMother
{
    public static ItemEntityBuilder Initial()
    {
        return new ItemEntityBuilder()
            .WithDeleted(false)
            .WithIsTemporary(false)
            .WithoutCreatedFrom()
            .WithoutUpdatedOn()
            .WithoutPredecessorId()
            .WithoutPredecessor()
            .WithEmptyItemTypes()
            .WithIsFavorite(false)
            .WithAvailableAt(new AvailableAtEntityBuilder().CreateMany(3).ToList());
    }

    public static ItemEntityBuilder InitialForStore(Guid storeId)
    {
        return InitialForStore(storeId, Guid.NewGuid());
    }

    public static ItemEntityBuilder InitialForStore(Guid storeId, Guid sectionId)
    {
        var availabilities = AvailableAtEntityMother
            .InitialForStore(storeId)
            .WithDefaultSectionId(sectionId)
            .CreateMany(1)
            .ToList();

        return Initial().WithAvailableAt(availabilities);
    }

    public static ItemEntityBuilder InitialWithTypes()
    {
        var types = Enumerable.Range(0, 3).Select(_ => ItemTypeEntityMother.Initial().Create()).ToList();

        return new ItemEntityBuilder()
            .WithDeleted(false)
            .WithIsTemporary(false)
            .WithoutCreatedFrom()
            .WithoutPredecessorId()
            .WithoutPredecessor()
            .WithEmptyAvailableAt()
            .WithoutUpdatedOn()
            .WithIsFavorite(false)
            .WithItemTypes(types);
    }

    public static ItemEntityBuilder InitialWithTypesForStore(Guid storeId)
    {
        return InitialWithTypesForStore(storeId, Guid.NewGuid());
    }

    public static ItemEntityBuilder InitialWithTypesForStore(Guid storeId, Guid sectionId)
    {
        var types = Enumerable.Range(0, 3)
            .Select(_ => ItemTypeEntityMother.Initial().WithAvailableAt(CreateAvailabilities()).Create())
            .ToList();

        return InitialWithTypes()
            .WithItemTypes(types);

        IList<ItemTypeAvailableAt> CreateAvailabilities()
        {
            return ItemTypeAvailableAtEntityMother
                .InitialForStore(storeId)
                .WithDefaultSectionId(sectionId)
                .CreateMany(1)
                .ToList();
        }
    }

    public static ItemEntityBuilder Temporary(Guid storeId)
    {
        return InitialForStore(storeId)
            .WithCreatedFrom(Guid.NewGuid())
            .WithIsTemporary(true);
    }
}