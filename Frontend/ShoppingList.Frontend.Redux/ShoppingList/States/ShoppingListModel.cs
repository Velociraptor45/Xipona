namespace ShoppingList.Frontend.Redux.ShoppingList.States;

public record ShoppingListModel(Guid Id, SortedSet<ShoppingListSection> Sections)
{
    public bool AnyItemInBasket => Sections.Any(s => s.AnyItemsInBasket);
}