namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore
{
    public class SectionCreationInfo
    {
        public SectionCreationInfo(string name, int sortingIndex, bool isDefaultSection)
        {
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        public string Name { get; }
        public int SortingIndex { get; }
        public bool IsDefaultSection { get; }
    }
}