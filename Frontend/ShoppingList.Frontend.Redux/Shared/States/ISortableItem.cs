namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

public interface ISortableItem
{
    public string Name { get; }
    public int SortingIndex { get; }
}