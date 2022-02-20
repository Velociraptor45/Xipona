namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class SearchItemResult
    {
        public SearchItemResult(int itemId, string name)
        {
            ItemId = itemId;
            Name = name;
        }

        public int ItemId { get; }
        public string Name { get; }
    }
}