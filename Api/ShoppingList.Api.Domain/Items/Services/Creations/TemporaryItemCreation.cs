using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;

public class TemporaryItemCreation
{
    public TemporaryItemCreation(Guid clientSideId, ItemName name, IItemAvailability availability)
    {
        ClientSideId = clientSideId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Availability = availability ?? throw new ArgumentNullException(nameof(availability));
    }

    public Guid ClientSideId { get; }
    public ItemName Name { get; }
    public IItemAvailability Availability { get; }
}