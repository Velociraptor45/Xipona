using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Tests.Items.Entities;

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
            .WithEmptyItemTypes();
    }

    public static ItemEntityBuilder InitialForStore(Guid storeId)
    {
        var availabilities = AvailableAtEntityMother
            .InitialForStore(storeId)
            .CreateMany(1)
            .ToList();

        return Initial().WithAvailabilities(availabilities);
    }

    public static ItemEntityBuilder InitialWithTypes()
    {
        return new ItemEntityBuilder()
            .WithDeleted(false)
            .WithIsTemporary(false)
            .WithoutCreatedFrom()
            .WithoutPredecessorId()
            .WithoutPredecessor()
            .WithEmptyAvailabilities();
    }
}