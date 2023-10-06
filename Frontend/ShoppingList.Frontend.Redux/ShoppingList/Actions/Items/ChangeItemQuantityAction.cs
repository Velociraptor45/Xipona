using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;

public record ChangeItemQuantityAction(ShoppingListItemId ItemId, Guid? ItemTypeId, float Quantity,
    ChangeItemQuantityAction.ChangeType Type, string ItemName)
{
    public enum ChangeType
    {
        Absolute,
        Diff
    }
}