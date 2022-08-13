using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Tests.Items.Entities;

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