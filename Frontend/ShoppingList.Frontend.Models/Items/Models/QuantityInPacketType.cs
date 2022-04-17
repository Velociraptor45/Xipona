namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models
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