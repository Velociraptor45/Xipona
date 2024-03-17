namespace ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;

public static class ItemTypeAvailableAtEntityMother
{
    public static ItemTypeAvailableAtEntityBuilder Initial()
    {
        return new ItemTypeAvailableAtEntityBuilder()
            .WithoutItemType();
    }

    public static ItemTypeAvailableAtEntityBuilder InitialForStore(Guid storeId)
    {
        return Initial().WithStoreId(storeId);
    }
}