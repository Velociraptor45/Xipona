using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

public class StoreItemSectionReadModel
{
    public StoreItemSectionReadModel(SectionId id, SectionName name, int sortingIndex)
    {
        Id = id;
        Name = name;
        SortingIndex = sortingIndex;
    }

    public StoreItemSectionReadModel(IStoreSection section) :
        this(section.Id, section.Name, section.SortingIndex)
    {
    }

    public SectionId Id { get; }
    public SectionName Name { get; }
    public int SortingIndex { get; }
}