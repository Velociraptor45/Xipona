﻿namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;

public static class ItemTypeEntityMother
{
    public static ItemTypeEntityBuilder Initial()
    {
        return new ItemTypeEntityBuilder()
            .WithoutPredecessorId()
            .WithoutPredecessor()
            .WithoutItem();
    }

    public static ItemTypeEntityBuilder InitialForStore(Guid storeId)
    {
        var availabilities = ItemTypeAvailableAtEntityMother
            .InitialForStore(storeId)
            .CreateMany(1)
            .ToList();

        return new ItemTypeEntityBuilder()
            .WithAvailableAt(availabilities)
            .WithoutPredecessorId()
            .WithoutPredecessor()
            .WithoutItem();
    }
}