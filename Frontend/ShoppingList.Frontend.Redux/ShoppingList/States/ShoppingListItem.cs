using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

public record ShoppingListItem(
    ShoppingListItemId Id,
    Guid? TypeId,
    string Name,
    bool IsTemporary,
    float PricePerQuantity,
    QuantityType QuantityType,
    float? QuantityInPacket,
    QuantityTypeInPacket? QuantityInPacketType,
    string ItemCategory,
    string Manufacturer,
    bool IsInBasket,
    float Quantity,
    bool Hidden);