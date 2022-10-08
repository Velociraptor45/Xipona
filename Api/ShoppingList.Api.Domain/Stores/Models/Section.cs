using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class Section : ISection
{
    public Section(SectionId id, SectionName name, int sortingIndex, bool isDefaultSection)
    {
        Id = id;
        Name = name;
        SortingIndex = sortingIndex;
        IsDefaultSection = isDefaultSection;
    }

    public SectionId Id { get; }
    public SectionName Name { get; }
    public int SortingIndex { get; }
    public bool IsDefaultSection { get; }

    public ISection Update(SectionUpdate update)
    {
        return new Section(
            Id,
            update.Name,
            update.SortingIndex,
            update.IsDefaultSection);
    }
}