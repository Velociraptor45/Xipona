using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

public record ShoppingListSection(Guid Id, string Name, int SortingIndex, bool IsExpanded,
    IReadOnlyCollection<ShoppingListItem> Items) : ISortableItem
{
    public bool AllItemsHidden => Items.All(i => i.Hidden);
    public bool AllItemsInBasket => Items.All(i => i.IsInBasket);
    public bool AnyItemsInBasket => Items.Any(i => i.IsInBasket);

    public IEnumerable<ShoppingListItem> GetDisplayedItems(bool itemsInBasketVisible)
    {
        IEnumerable<ShoppingListItem> items = itemsInBasketVisible
            ? Items
            : Items.Where(item => !item.Hidden);

        return items.OrderBy(i => i.Name);
    }
}