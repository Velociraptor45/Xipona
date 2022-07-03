using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

public class ItemSectionReadModel
{
    public ItemSectionReadModel(SectionId id, SectionName name, int sortingIndex)
    {
        Id = id;
        Name = name;
        SortingIndex = sortingIndex;
    }

    public ItemSectionReadModel(IStoreSection section) :
        this(section.Id, section.Name, section.SortingIndex)
    {
    }

    public SectionId Id { get; }
    public SectionName Name { get; }
    public int SortingIndex { get; }
}