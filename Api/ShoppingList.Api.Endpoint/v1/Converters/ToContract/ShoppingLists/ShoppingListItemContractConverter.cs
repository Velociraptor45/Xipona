using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

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
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        ItemCategoryContract? itemCategoryContract = null;
        if (source.ItemCategory != null)
            itemCategoryContract = _itemCategoryContractConverter.ToContract(source.ItemCategory);

        ManufacturerContract? manufacturerContract = null;
        if (source.Manufacturer != null)
            manufacturerContract = _manufacturerContractConverter.ToContract(source.Manufacturer);

        QuantityTypeContract quantityTypeContract = _quantityTypeContractConverter.ToContract(source.QuantityType);
        QuantityTypeInPacketContract quantityTypeInPacketContract =
            _quantityTypeInPacketContractConverter.ToContract(source.QuantityTypeInPacket);

        return new ShoppingListItemContract(
            source.Id.Value,
            source.TypeId?.Value,
            source.Name,
            source.IsDeleted,
            source.Comment,
            source.IsTemporary,
            source.PricePerQuantity,
            quantityTypeContract,
            source.QuantityInPacket,
            quantityTypeInPacketContract,
            itemCategoryContract,
            manufacturerContract,
            source.IsInBasket,
            source.Quantity);
    }
}