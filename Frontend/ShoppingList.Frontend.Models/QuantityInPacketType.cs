namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class QuantityInPacketType
    {
        public QuantityInPacketType(int id, string name, string quantityLabel)
        {
            Id = id;
            Name = name;
            QuantityLabel = quantityLabel;
        }

        public int Id { get; }
        public string Name { get; }
        public string QuantityLabel { get; }
    }
}