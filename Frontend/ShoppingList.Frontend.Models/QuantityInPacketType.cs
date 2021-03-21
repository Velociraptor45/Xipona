namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class QuantityTypeInPacket
    {
        public QuantityTypeInPacket(int id, string name, string quantityLabel)
        {
            Id = id;
            Name = name;
            QuantityLabel = quantityLabel;
        }

        public int Id { get; set; }
        public string Name { get; }
        public string QuantityLabel { get; }
    }
}