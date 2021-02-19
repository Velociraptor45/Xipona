namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class StoreSection
    {
        public StoreSection(int id, string name, int sortingIndex, bool isDefaultSection)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        public int Id { get; }
        public string Name { get; set; }
        public int SortingIndex { get; }
        public bool IsDefaultSection { get; private set; }

        public void SetAsDefaultSection(bool isDefault)
        {
            IsDefaultSection = isDefault;
        }
    }
}