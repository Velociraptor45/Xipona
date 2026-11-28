using Xipona.Api.Contracts.Common.Queries;
using Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.ItemCategories.Services.Shared;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;
using Xipona.Api.Domain.Manufacturers.Services.Shared;
using Xipona.Api.Domain.ShoppingLists.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class ShoppingListItemContractConverter : IToContractConverter<ShoppingListItemReadModel, ShoppingListItemContract>
{
    private readonly IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> _itemCategoryContractConverter;
    private readonly IToContractConverter<ManufacturerReadModel, ManufacturerContract> _manufacturerContractConverter;
    private readonly IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> _quantityTypeContractConverter;
    private readonly IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> _quantityTypeInPacketContractConverter;

    public ShoppingListItemContractConverter(
        IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> itemCategoryContractConverter,
        IToContractConverter<ManufacturerReadModel, ManufacturerContract> manufacturerContractConverter,
        IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> quantityTypeContractConverter,
        IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> quantityTypeInPacketContractConverter)
    {
        _itemCategoryContractConverter = itemCategoryContractConverter;
        _manufacturerContractConverter = manufacturerContractConverter;
        _quantityTypeContractConverter = quantityTypeContractConverter;
        _quantityTypeInPacketContractConverter = quantityTypeInPacketContractConverter;
    }

    public ShoppingListItemContract ToContract(ShoppingListItemReadModel source)
    {
        ItemCategoryContract? itemCategoryContract = null;
        if (source.ItemCategory != null)
            itemCategoryContract = _itemCategoryContractConverter.ToContract(source.ItemCategory);

        ManufacturerContract? manufacturerContract = null;
        if (source.Manufacturer != null)
            manufacturerContract = _manufacturerContractConverter.ToContract(source.Manufacturer);

        QuantityTypeContract quantityTypeContract = _quantityTypeContractConverter.ToContract(source.QuantityType);
        QuantityTypeInPacketContract? quantityTypeInPacketContract = source.QuantityTypeInPacket is null
            ? null
            : _quantityTypeInPacketContractConverter.ToContract(source.QuantityTypeInPacket);

        return new ShoppingListItemContract(
            source.Id,
            source.TypeId,
            source.Name,
            source.IsDeleted,
            source.Comment.Value,
            source.IsTemporary,
            source.PricePerQuantity,
            quantityTypeContract,
            source.QuantityInPacket,
            quantityTypeInPacketContract,
            itemCategoryContract,
            manufacturerContract,
            source.IsInBasket,
            source.Quantity.Value,
            source.IsDiscounted);
    }
}