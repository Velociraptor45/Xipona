namespace ShoppingList.Frontend.Models
{
    public class ItemCategory
    {
        public ItemCategory(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}