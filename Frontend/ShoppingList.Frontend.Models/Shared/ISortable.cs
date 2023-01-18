namespace ProjectHermes.ShoppingList.Frontend.Models.Shared;

public interface ISortable<in T>
{
    public int MinSortingIndex { get; }
    public int MaxSortingIndex { get; }
}