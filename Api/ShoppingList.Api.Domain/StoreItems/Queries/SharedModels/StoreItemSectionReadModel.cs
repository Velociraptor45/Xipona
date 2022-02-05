using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;

public class StoreItemSectionReadModel
{
    public StoreItemSectionReadModel(SectionId id, string name, int sortingIndex)
    {
        Id = id;
        Name = name;
        SortingIndex = sortingIndex;
    }

    public SectionId Id { get; }
    public string Name { get; }
    public int SortingIndex { get; }
}