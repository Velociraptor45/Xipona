namespace ShoppingList.Frontend.Redux.ShoppingList.States;

public record ShoppingListSection(Guid Id, string Name, int SortingIndex, bool IsExpanded,
    IEnumerable<ShoppingListItem> Items)
{
    public bool AllItemsHidden => Items.All(i => i.Hidden);
    public bool AllItemsInBasket => Items.All(i => i.IsInBasket);
    public bool AnyItemsInBasket => Items.Any(i => i.IsInBasket);
}