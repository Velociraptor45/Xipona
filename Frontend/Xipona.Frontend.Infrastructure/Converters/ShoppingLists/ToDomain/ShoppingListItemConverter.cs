using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ShoppingListItem = ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.ShoppingListItem;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain;

public class ShoppingListItemConverter : IToDomainConverter<ShoppingListItemContract, ShoppingListItem>
{
    private readonly IToDomainConverter<QuantityTypeContract, QuantityType> _quantityTypeConverter;
    private readonly IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket> _quantityTypeInPacketConverter;

    public ShoppingListItemConverter(
        IToDomainConverter<QuantityTypeContract, QuantityType> quantityTypeConverter,
        IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket> quantityTypeInPacketConverter)
    {
        _quantityTypeConverter = quantityTypeConverter;
        _quantityTypeInPacketConverter = quantityTypeInPacketConverter;
    }

    public ShoppingListItem ToDomain(ShoppingListItemContract source)
    {
        return new ShoppingListItem(
            ShoppingListItemId.FromActualId(source.Id),
            source.TypeId,
            source.Name,
            source.IsTemporary,
            source.PricePerQuantity,
            _quantityTypeConverter.ToDomain(source.QuantityType),
            source.QuantityInPacket,
            source.QuantityTypeInPacket is null ?
                null :
                _quantityTypeInPacketConverter.ToDomain(source.QuantityTypeInPacket),
            source.ItemCategory?.Name ?? "",
            source.Manufacturer?.Name ?? "",
            source.IsInBasket,
            source.Quantity,
            Hidden: source.IsInBasket,
            source.IsDiscounted);
    }
}