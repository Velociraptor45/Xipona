namespace ShoppingList.Frontend.Models
{
    public class QuantityType
    {
        public QuantityType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}