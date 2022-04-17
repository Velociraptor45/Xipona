using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class Section
    {
        public Section(SectionId id, string name, int sortingIndex, bool isDefaultSection)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        public SectionId Id { get; set; }
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

        public ItemSection AsStoreItemSection()
        {
            return new ItemSection(Id.BackendId, Name, SortingIndex);
        }
    }
}