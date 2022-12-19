namespace ShoppingList.Frontend.Redux.ShoppingList.States;

public record ShoppingListSection(Guid Id, string Name, int SortingIndex, bool IsExpanded,
    IEnumerable<ShoppingListItem> Items)
{
    public bool AllItemsInBasket => Items.All(i => i.IsInBasket);
    public bool SomeItemsInBasket => Items.Any(i => i.IsInBasket);
}