using ShoppingList.Database.Entities;
using System.Runtime.CompilerServices;

namespace ShoppingList.EntityModels
{
    public class ItemDto
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint Quantity { get; set; }
        public bool IsInShoppingBasket { get; set; }
        public decimal PricePerQuantity { get; set; }
        public bool Active { get; set; }
        public uint StoreId { get; set; }
        public QuantityType QuantityType { get; set; }
        public string Comment { get; set; }
    }
}
