using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class StoreItemContractConverter :
    IToContractConverter<StoreItemReadModel, StoreItemContract>
{
    private readonly IToContractConverter<StoreItemAvailabilityReadModel, StoreItemAvailabilityContract> _storeItemAvailabilityContractConverter;
    private readonly IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> _itemCategoryContractConverter;
    private readonly IToContractConverter<ManufacturerReadModel, ManufacturerContract> _manufacturerContractConverter;
    private readonly IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> _quantityTypeContractConverter;
    private readonly IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> _quantityTypeInPacketContractConverter;
    private readonly IToContractConverter<ItemTypeReadModel, ItemTypeContract> _itemTypeContractConverter;

    public StoreItemContractConverter(
        IToContractConverter<StoreItemAvailabilityReadModel, StoreItemAvailabilityContract> storeItemAvailabilityContractConverter,
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

    public StoreItemContract ToContract(StoreItemReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        ItemCategoryContract? itemCategoryContract = null;
        if (source.ItemCategory != null)
            itemCategoryContract = _itemCategoryContractConverter.ToContract(source.ItemCategory);

        ManufacturerContract? manufacturerContract = null;
        if (source.Manufacturer != null)
            manufacturerContract = _manufacturerContractConverter.ToContract(source.Manufacturer);

        return new StoreItemContract
        {
            Id = source.Id.Value,
            Name = source.Name,
            IsDeleted = source.IsDeleted,
            Comment = source.Comment,
            IsTemporary = source.IsTemporary,
            QuantityType = _quantityTypeContractConverter.ToContract(source.QuantityType),
            QuantityInPacket = source.QuantityInPacket,
            QuantityTypeInPacket = _quantityTypeInPacketContractConverter.ToContract(source.QuantityTypeInPacket),
            ItemCategory = itemCategoryContract,
            Manufacturer = manufacturerContract,
            Availabilities = _storeItemAvailabilityContractConverter.ToContract(source.Availabilities),
            ItemTypes = _itemTypeContractConverter.ToContract(source.ItemTypes)
        };
    }
}