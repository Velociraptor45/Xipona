using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class StoreSection
    {
        public StoreSection(StoreSectionId id, string name, int sortingIndex, bool isDefaultSection)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        public StoreSectionId Id { get; }
        public string Name { get; set; }
        public int SortingIndex { get; private set; }
        public bool IsDefaultSection { get; private set; }

        public void SetAsDefaultSection(bool isDefault)
        {
            IsDefaultSection = isDefault;
        }

        public void SetSortingIndex(int index)
        {
            SortingIndex = index;
        }

        public StoreItemSection AsStoreItemSection()
        {
            return new StoreItemSection(Id.BackendId, Name, SortingIndex);
        }
    }
}