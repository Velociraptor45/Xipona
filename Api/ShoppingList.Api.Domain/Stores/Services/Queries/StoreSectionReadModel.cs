using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

public class StoreSectionReadModel
{
    public StoreSectionReadModel(SectionId id, SectionName name, int sortingIndex, bool isDefaultSection)
    {
        Id = id;
        Name = name;
        SortingIndex = sortingIndex;
        IsDefaultSection = isDefaultSection;
    }

    public StoreSectionReadModel(IStoreSection section) :
        this(section.Id, section.Name, section.SortingIndex, section.IsDefaultSection)
    {
    }

    public SectionId Id { get; }
    public SectionName Name { get; }
    public int SortingIndex { get; }
    public bool IsDefaultSection { get; }
}