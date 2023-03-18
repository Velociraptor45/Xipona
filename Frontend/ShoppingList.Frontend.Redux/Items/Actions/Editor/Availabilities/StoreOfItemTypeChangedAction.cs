using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record StoreOfItemTypeChangedAction(EditedItemType ItemType, EditedItemAvailability Availability, Guid StoreId);