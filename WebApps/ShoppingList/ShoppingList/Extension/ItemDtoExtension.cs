using ShoppingList.EntityModels;

namespace ShoppingList.Extension
{
    public static class ItemDtoExtension
    {
        public static ItemDto Clone(this ItemDto itemDto)
        {
            return new ItemDto()
            {
                Id = itemDto.Id,
                Name = itemDto.Name,
                Quantity = itemDto.Quantity,
                IsInShoppingBasket = itemDto.IsInShoppingBasket,
                PricePerQuantity = itemDto.PricePerQuantity,
                Active = itemDto.Active,
                StoreId = itemDto.StoreId,
                QuantityType = itemDto.QuantityType,
                Comment = itemDto.Comment
            };
        }
    }
}
