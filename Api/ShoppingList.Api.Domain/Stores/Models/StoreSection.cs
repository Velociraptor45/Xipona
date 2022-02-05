namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class StoreSection : IStoreSection
{
    public StoreSection(SectionId id, string name, int sortingIndex, bool isDefaultSection)
    {
        Id = id;
        Name = name;
        SortingIndex = sortingIndex;
        IsDefaultSection = isDefaultSection;
    }

    public SectionId Id { get; }
    public string Name { get; }
    public int SortingIndex { get; }
    public bool IsDefaultSection { get; }
}