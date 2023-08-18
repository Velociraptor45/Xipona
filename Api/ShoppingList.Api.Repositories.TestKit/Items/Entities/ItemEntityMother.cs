using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;

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
        var types = ItemTypeEntityMother.Initial().CreateMany(3).ToList();

        return new ItemEntityBuilder()
            .WithDeleted(false)
            .WithIsTemporary(false)
            .WithoutCreatedFrom()
            .WithoutPredecessorId()
            .WithoutPredecessor()
            .WithEmptyAvailableAt()
            .WithItemTypes(types);
    }

    public static ItemEntityBuilder InitialWithTypesForStore(Guid storeId, Guid sectionId)
    {
        var types = Enumerable.Range(0, 3)
            .Select(_ => ItemTypeEntityMother.Initial().WithAvailableAt(CreateAvailabilities()).Create())
            .ToList();

        return new ItemEntityBuilder()
            .WithDeleted(false)
            .WithIsTemporary(false)
            .WithoutCreatedFrom()
            .WithoutPredecessorId()
            .WithoutPredecessor()
            .WithEmptyAvailableAt()
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
}