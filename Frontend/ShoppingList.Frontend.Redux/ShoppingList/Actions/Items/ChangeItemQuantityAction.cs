using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;

public record ChangeItemQuantityAction(ShoppingListItemId ItemId, Guid? ItemTypeId, float Quantity,
    ChangeItemQuantityAction.ChangeType Type)
{
    public enum ChangeType
    {
        Absolute,
        Diff
    }
}