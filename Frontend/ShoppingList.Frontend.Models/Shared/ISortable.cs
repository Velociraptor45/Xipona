namespace ProjectHermes.ShoppingList.Frontend.Models.Shared;

public interface ISortable<in T>
{
    public int MinSortingIndex { get; }
    public int MaxSortingIndex { get; }

    void Remove(T model);

    void Increment(T model);

    void Decrement(T model);
}