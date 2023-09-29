namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
public record EditedItemType(Guid Id, Guid Key, string Name, IReadOnlyCollection<EditedItemAvailability> Availabilities) : IAvailable;