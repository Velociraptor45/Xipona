namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model
{
    public class StoreSection : IStoreSection
    {
        public StoreSection(StoreSectionId id, string name, int sortingIndex, bool isDefaultSection)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        public StoreSectionId Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
        public bool IsDefaultSection { get; }
    }
}