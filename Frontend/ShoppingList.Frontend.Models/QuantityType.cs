namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class QuantityType
    {
        public QuantityType(int id, string name, int defaultQuantity, string priceLabel)
        {
            Id = id;
            Name = name;
            DefaultQuantity = defaultQuantity;
            PriceLabel = priceLabel;
        }

        public int Id { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public string PriceLabel { get; }
    }
}