namespace ProjectHermes.ShoppingList.Frontend.Models.Shared;

public interface ISortableItem
{
    public string Name { get; set; }
    public int SortingIndex { get; }
}