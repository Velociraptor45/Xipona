namespace ShoppingList.Frontend.Models
{
    public class QuantityInPacketType
    {
        public QuantityInPacketType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}