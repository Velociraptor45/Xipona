namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class StoreItemSection
    {
        public StoreItemSection(int id, string name, int sortingIndex)
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