using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using QuantityType = ShoppingList.Frontend.Redux.Shared.States.QuantityType;
using QuantityTypeInPacket = ShoppingList.Frontend.Redux.Shared.States.QuantityTypeInPacket;

namespace ShoppingList.Frontend.Redux.Items.States;

public record EditedItem(
    Guid Id,
    string Name,
    bool IsDeleted,
    string Comment,
    bool IsTemporary,
    QuantityType QuantityType,
    float? QuantityInPacket,
    QuantityTypeInPacket? QuantityInPacketType,
    Guid? ItemCategoryId,
    Guid? ManufacturerId,
    IReadOnlyCollection<EditedItemAvailability> Availabilities,
    IReadOnlyCollection<EditedItemType> ItemTypes) : IAvailable
{
    public ItemMode ItemMode
    {
        get
        {
            if (Id == Guid.Empty)
            {
                return ItemMode.NotDefined;
            }

            return ItemTypes.Count > 0 ? ItemMode.WithTypes : ItemMode.WithoutTypes;
        }
    }

    public bool IsItemWithTypes => ItemMode == ItemMode.WithTypes;
}