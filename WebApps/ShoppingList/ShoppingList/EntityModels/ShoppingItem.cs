namespace ShoppingList.EntityModels
{
    public class ItemDto
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint Quantity { get; set; }
        public decimal? PricePerQuantity { get; set; }
        public QuantityType QuantityType { get; set; }
    }
}
