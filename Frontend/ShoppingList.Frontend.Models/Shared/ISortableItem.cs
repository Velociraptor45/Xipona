namespace ProjectHermes.ShoppingList.Frontend.Models.Shared;

public interface ISortableItem
{
    public string Name { get; }
    public int SortingIndex { get; }
}