namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore
{
    public class SectionCreation
    {
        public SectionCreation(string name, int sortingIndex, bool isDefaultSection)
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