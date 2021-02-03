namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemSection : IStoreItemSection
    {
        public StoreItemSection(StoreItemSectionId id, string name, int SortIndex)
        {
            Id = id;
            Name = name;
            this.SortIndex = SortIndex;
        }

        public StoreItemSectionId Id { get; }
        public string Name { get; }
        public int SortIndex { get; }
    }
}