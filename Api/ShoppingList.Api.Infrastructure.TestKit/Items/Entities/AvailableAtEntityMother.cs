namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Items.Entities;

public static class AvailableAtEntityMother
{
    public static AvailableAtEntityBuilder Initial()
    {
        return new AvailableAtEntityBuilder()
            .WithoutItem();
    }

    public static AvailableAtEntityBuilder InitialForStore(Guid storeId)
    {
        return Initial().WithStoreId(storeId);
    }
}