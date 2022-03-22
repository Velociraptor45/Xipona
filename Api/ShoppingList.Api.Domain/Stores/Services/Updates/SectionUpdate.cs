using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

public class SectionUpdate
{
    public SectionUpdate(SectionId? id, SectionName name, int sortingIndex, bool isDefaultSection)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        SortingIndex = sortingIndex;
        IsDefaultSection = isDefaultSection;
    }

    public SectionId? Id { get; }
    public SectionName Name { get; }
    public int SortingIndex { get; }
    public bool IsDefaultSection { get; }
}