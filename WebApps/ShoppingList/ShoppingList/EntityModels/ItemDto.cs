using System.ComponentModel.DataAnnotations;

namespace ShoppingList.EntityModels
{
    public class ItemDto
    {
        public uint Id { get; set; }
        public uint? ItemCategoryId { get; set; }
        public uint? ManufacturerId { get; set; }

        [MaxLength(255, ErrorMessage = "The name mustn't be longer than 255 characters")]
        public string Name { get; set; }
        public uint Quantity { get; set; }
        public bool IsInShoppingBasket { get; set; }

        // this variable is of type float in the database. But BlazorStrap has a bug where you can't
        // input a number in a BSInput of type float 
        [RegularExpression(@"\d+", ErrorMessage = "Number must be an integer")]
        public int QuantityInPacket { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ConvertValueInInvariantCulture = true, ErrorMessage = "Price mustn't be negative")]
        public decimal PricePerQuantity { get; set; }
        public bool Active { get; set; }
        public uint StoreId { get; set; }
        public QuantityType QuantityType { get; set; }

        [MaxLength(255, ErrorMessage = "The comment mustn't be longer than 255 characters")]
        public string Comment { get; set; }
        public QuantityType QuantityInPacketType { get; set; }
        public bool IsTemporary { get; set; }
    }
}
