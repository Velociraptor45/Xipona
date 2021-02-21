namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class StoreItemSectionContract
    {
        public StoreItemSectionContract(int id, string name, int sortingIndex)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
        }

        public int Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
    }
}