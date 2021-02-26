namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores
{
    public class StoreSectionContract
    {
        public StoreSectionContract(int id, string name, int sortingIndex, bool isDefautlSection)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefautlSection = isDefautlSection;
        }

        public int Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
        public bool IsDefautlSection { get; }
    }
}