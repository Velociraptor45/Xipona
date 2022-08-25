namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Items.Entities;

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
            .WithAvailabilities(availabilities)
            .WithoutPredecessorId()
            .WithoutPredecessor()
            .WithoutItem();
    }
}