using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ShoppingList.Frontend.Redux.Shared.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.States;

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
    bool Hide);