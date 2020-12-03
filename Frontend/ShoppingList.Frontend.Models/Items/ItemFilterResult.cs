namespace ShoppingList.Frontend.Models.Items
{
    public class ItemFilterResult
    {
        public ItemFilterResult(int itemId, string name)
        {
            ItemId = itemId;
            Name = name;
        }

        public int ItemId { get; }
        public string Name { get; }
    }
}