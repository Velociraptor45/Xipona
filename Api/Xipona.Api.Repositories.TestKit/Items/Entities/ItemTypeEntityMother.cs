namespace ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;

public static class ItemTypeEntityMother
{
    public static ItemTypeEntityBuilder Initial()
    {
        return new ItemTypeEntityBuilder()
            .WithoutPredecessorId()
            .WithoutPredecessor()
            .WithoutItem()
            .WithIsDeleted(false)
            .WithAvailableAt(new ItemTypeAvailableAtEntityBuilder().CreateMany(3).ToList());
    }

    public static ItemTypeEntityBuilder InitialForStore(Guid storeId)
    {
        return InitialForStore(storeId, Guid.NewGuid());
    }

    public static ItemTypeEntityBuilder InitialForStore(Guid storeId, Guid sectionId)
    {
        var availabilities = ItemTypeAvailableAtEntityMother
            .InitialForStore(storeId)
            .WithDefaultSectionId(sectionId)
            .CreateMany(1)
            .ToList();

        return new ItemTypeEntityBuilder()
            .WithAvailableAt(availabilities)
            .WithoutPredecessorId()
            .WithoutPredecessor()
            .WithoutItem()
            .WithIsDeleted(false);
    }
}