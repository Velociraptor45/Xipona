namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

public interface ISortable<in T>
{
    public int MinSortingIndex { get; }
    public int MaxSortingIndex { get; }
}