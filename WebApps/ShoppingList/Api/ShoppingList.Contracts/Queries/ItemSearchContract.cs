namespace ShoppingList.Contracts.Queries
{
    public class ItemSearchContract
    {
        public ItemSearchContract(int id, string name, float price, string itemCategoryName, string manufacturerName)
        {
            Id = id;
            Name = name;
            Price = price;
            ItemCategoryName = itemCategoryName;
            ManufacturerName = manufacturerName;
        }

        public int Id { get; }
        public string Name { get; }
        public float Price { get; }
        public string ItemCategoryName { get; }
        public string ManufacturerName { get; }
    }
}