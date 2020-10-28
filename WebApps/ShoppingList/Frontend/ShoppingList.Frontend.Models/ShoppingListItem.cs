namespace ShoppingList.Frontend.Models
{
    public class ShoppingListItem
    {
        public ShoppingListItem(int id, string name, bool isTemporary, float price, float quantityInPacket,
            string itemCategory, string manufacturer, bool isInBasket, float quantity)
        {
            Id = id;
            Name = name;
            IsTemporary = isTemporary;
            Price = price;
            QuantityInPacket = quantityInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            IsInBasket = isInBasket;
            Quantity = quantity;
        }

        public int Id { get; }
        public string Name { get; }
        public bool IsTemporary { get; }
        public float Price { get; }
        public float QuantityInPacket { get; }
        public string ItemCategory { get; }
        public string Manufacturer { get; }
        public bool IsInBasket { get; set; }
        public float Quantity { get; set; }
    }
}