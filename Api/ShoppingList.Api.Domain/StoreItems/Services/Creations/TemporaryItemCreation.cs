using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;

public class TemporaryItemCreation
{
    public TemporaryItemCreation(Guid clientSideId, ItemName name, IStoreItemAvailability availability)
    {
        ClientSideId = clientSideId;
        Name = name;
        Availability = availability ?? throw new ArgumentNullException(nameof(availability));
    }

    public Guid ClientSideId { get; }
    public ItemName Name { get; }
    public IStoreItemAvailability Availability { get; }
}