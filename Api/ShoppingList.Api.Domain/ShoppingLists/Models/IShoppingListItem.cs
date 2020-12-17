using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public interface IShoppingListItem
    {
        ShoppingListItemId Id { get; }
        string Name { get; }
        bool IsDeleted { get; }
        string Comment { get; }
        bool IsTemporary { get; }
        float PricePerQuantity { get; }
        QuantityType QuantityType { get; }
        float QuantityInPacket { get; }
        QuantityTypeInPacket QuantityTypeInPacket { get; }
        IItemCategory ItemCategory { get; }
        IManufacturer Manufacturer { get; }
        bool IsInBasket { get; }
        float Quantity { get; }

        void PutInBasket();

        void RemoveFromBasket();

        void ChangeQuantity(float quantity);
    }
}