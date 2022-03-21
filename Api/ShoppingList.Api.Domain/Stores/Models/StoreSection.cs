using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class StoreSection : IStoreSection
{
    public StoreSection(SectionId id, SectionName name, int sortingIndex, bool isDefaultSection)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        SortingIndex = sortingIndex;
        IsDefaultSection = isDefaultSection;
    }

    public SectionId Id { get; }
    public SectionName Name { get; }
    public int SortingIndex { get; }
    public bool IsDefaultSection { get; }

    public IStoreSection Update(SectionUpdate update)
    {
        return new StoreSection(
            Id,
            update.Name,
            update.SortingIndex,
            update.IsDefaultSection);
    }
}