using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemContractConverter :
    IToContractConverter<ItemReadModel, ItemContract>
{
    private readonly IToContractConverter<ItemAvailabilityReadModel, ItemAvailabilityContract> _storeItemAvailabilityContractConverter;
    private readonly IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> _itemCategoryContractConverter;
    private readonly IToContractConverter<ManufacturerReadModel, ManufacturerContract> _manufacturerContractConverter;
    private readonly IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> _quantityTypeContractConverter;
    private readonly IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> _quantityTypeInPacketContractConverter;
    private readonly IToContractConverter<ItemTypeReadModel, ItemTypeContract> _itemTypeContractConverter;

    public ItemContractConverter(
        IToContractConverter<ItemAvailabilityReadModel, ItemAvailabilityContract> storeItemAvailabilityContractConverter,
        IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> itemCategoryContractConverter,
        IToContractConverter<ManufacturerReadModel, ManufacturerContract> manufacturerContractConverter,
        IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> quantityTypeContractConverter,
        IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> quantityTypeInPacketContractConverter,
        IToContractConverter<ItemTypeReadModel, ItemTypeContract> itemTypeContractConverter)
    {
        _storeItemAvailabilityContractConverter = storeItemAvailabilityContractConverter;
        _itemCategoryContractConverter = itemCategoryContractConverter;
        _manufacturerContractConverter = manufacturerContractConverter;
        _quantityTypeContractConverter = quantityTypeContractConverter;
        _quantityTypeInPacketContractConverter = quantityTypeInPacketContractConverter;
        _itemTypeContractConverter = itemTypeContractConverter;
    }

    public ItemContract ToContract(ItemReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        ItemCategoryContract? itemCategoryContract = null;
        if (source.ItemCategory != null)
            itemCategoryContract = _itemCategoryContractConverter.ToContract(source.ItemCategory);

        ManufacturerContract? manufacturerContract = null;
        if (source.Manufacturer != null)
            manufacturerContract = _manufacturerContractConverter.ToContract(source.Manufacturer);

        return new ItemContract(
            source.Id.Value,
            source.Name.Value,
            source.IsDeleted,
            source.Comment.Value,
            source.IsTemporary,
            _quantityTypeContractConverter.ToContract(source.QuantityType),
            source.QuantityInPacket?.Value,
            source.QuantityTypeInPacket is null
                ? null
                : _quantityTypeInPacketContractConverter.ToContract(source.QuantityTypeInPacket),
            itemCategoryContract,
            manufacturerContract,
            _storeItemAvailabilityContractConverter.ToContract(source.Availabilities),
            _itemTypeContractConverter.ToContract(source.ItemTypes));
    }
}