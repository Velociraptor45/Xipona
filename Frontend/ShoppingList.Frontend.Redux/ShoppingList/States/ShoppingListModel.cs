namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

public record ShoppingListModel(Guid Id, SortedSet<ShoppingListSection> Sections)
{
    public IEnumerable<ShoppingListItem> Items => Sections.SelectMany(s => s.Items);
    public bool AnyItemInBasket => Sections.Any(s => s.AnyItemsInBasket);
    public int ItemInBasketCount => Items.Count(i => i.IsInBasket);
    public int ItemNotInBasketCount => Items.Count(i => !i.IsInBasket);
}