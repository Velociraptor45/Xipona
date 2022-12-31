namespace ShoppingList.Frontend.Redux.Items.States;
public record EditedItemType(Guid Id, string Name, IReadOnlyCollection<EditedItemAvailability> Availabilities) : IAvailable;