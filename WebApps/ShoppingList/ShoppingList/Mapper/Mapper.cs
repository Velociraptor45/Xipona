using ShoppingList.Database.Entities;
using ShoppingList.EntityModels;

namespace ShoppingList.Mapper
{
    public class Mapper
    {
        public ItemDto ToItemDto(Item item, ItemOnShoppingList relation)
        {
            var itemDto = new ItemDto()
            {
                Id = item.ItemId,
                Name = item.Name,
                QuantityType = ToQuantityType(item.QuantityTypeId),
                IsInShoppingBasket = relation?.IsInShoppingBasket ?? false,
                PricePerQuantity = item.PricePerQuantity,
                Quantity = relation?.Quantity ?? 0,
                StoreId = item.StoreId,
                Active = item.Active,
                Comment = item.Comment
            };

            if (relation?.Quantity == null)
            {
                SetDefaultQuantity(itemDto);
            }
            return itemDto;
        }

        public Item ToItem(ItemDto itemDto)
        {
            return new Item
            {
                Name = itemDto.Name,
                QuantityTypeId = (uint)itemDto.QuantityType,
                PricePerQuantity = itemDto.PricePerQuantity,
                StoreId = itemDto.StoreId,
                Active = itemDto.Active,
                Comment = itemDto.Comment
            };
        }

        private void SetDefaultQuantity(EntityModels.ItemDto itemDto)
        {
            switch (itemDto.QuantityType)
            {
                case EntityModels.QuantityType.Unit:
                    itemDto.Quantity = 1;
                    break;
                case EntityModels.QuantityType.Weight:
                    itemDto.Quantity = 100;
                    break;
            }
        }

        public EntityModels.QuantityType ToQuantityType(uint quantityTypeId)
        {
            // there aren't so many quantity types that the conversion
            // uint -> int would be a problem
            return (EntityModels.QuantityType)(int)quantityTypeId;
        }
    }
}
