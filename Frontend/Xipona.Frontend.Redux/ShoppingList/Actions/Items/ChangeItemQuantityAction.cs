using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Items;

public record ChangeItemQuantityAction(ShoppingListItemId ItemId, Guid? ItemTypeId, float Quantity,
    ChangeItemQuantityAction.ChangeType Type, string ItemName)
{
    public enum ChangeType
    {
        Absolute,
        Diff
    }
}