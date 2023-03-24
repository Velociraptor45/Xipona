﻿namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;

public static class ItemEntityMother
{
    public static ItemEntityBuilder Initial()
    {
        return new ItemEntityBuilder()
            .WithDeleted(false)
            .WithIsTemporary(false)
            .WithoutCreatedFrom()
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
}