namespace ShoppingList.Frontend.Redux.Items.States;
public record EditedItemType(Guid id, string name, IEnumerable<EditedItemAvailability> availabilities);