using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain
{
    public class ShoppingListItemConverter : IToDomainConverter<ShoppingListItemContract, ShoppingListItem>
    {
        private readonly IToDomainConverter<QuantityTypeContract, QuantityType> quantityTypeConverter;
        private readonly IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket> quantityTypeInPacketConverter;

        public ShoppingListItemConverter(
            IToDomainConverter<QuantityTypeContract, QuantityType> quantityTypeConverter,
            IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket> quantityTypeInPacketConverter)
        {
            this.quantityTypeConverter = quantityTypeConverter;
            this.quantityTypeInPacketConverter = quantityTypeInPacketConverter;
        }

        public ShoppingListItem ToDomain(ShoppingListItemContract source)
        {
            return new ShoppingListItem(
                    ItemId.FromActualId(source.Id),
                    source.TypeId,
                    source.Name,
                    source.IsTemporary,
                    source.PricePerQuantity,
                    quantityTypeConverter.ToDomain(source.QuantityType),
                    source.QuantityInPacket,
                    source.QuantityTypeInPacket is null ?
                        null :
                        quantityTypeInPacketConverter.ToDomain(source.QuantityTypeInPacket),
                    source.ItemCategory?.Name ?? "",
                    source.Manufacturer?.Name ?? "",
                    source.IsInBasket,
                    source.Quantity);
        }
    }
}