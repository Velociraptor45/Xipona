using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.TestKit.Common.Extensions;
using Xipona.Api.Repositories.ItemCategories.Entities;
using Xipona.Api.Repositories.Manufacturers.Entities;
using Xipona.Api.Repositories.Stores.Entities;
using Xipona.Api.Repositories.TestKit.Items.Entities;

namespace Xipona.Api.Endpoint.IntegrationTests.v1.Endpoints;

public class ItemEntityCreationContext
{
    private List<Store> _stores = [];
    private Manufacturer? _manufacturer;
    private ItemCategory? _itemCategory;
    private QuantityType? _quantityType;
    private QuantityTypeInPacket? _quantityTypeInPacket;
    private float? _quantityInPacket;

    public IReadOnlyList<Store> Stores => _stores.AsReadOnly();

    public void AddStores(params Store[] stores)
    {
        _stores = stores.ToList();
    }

    public void AddManufacturer(Manufacturer manufacturer)
    {
        _manufacturer = manufacturer;
    }

    public void AddItemCategory(ItemCategory itemCategory)
    {
        _itemCategory = itemCategory;
    }

    public void AddQuantity(QuantityType quantityType, QuantityTypeInPacket? quantityTypeInPacket,
        float? quantityInPacket)
    {
        _quantityType = quantityType;
        _quantityTypeInPacket = quantityTypeInPacket;
        _quantityInPacket = quantityInPacket;
    }

    public ItemTypeEntityBuilder FillItemType(ItemTypeEntityBuilder builder)
    {
        var avs = _stores
            .Select(s => new ItemTypeAvailableAtEntityBuilder()
                .WithDefaultSectionId(s.Sections.ChooseRandom().Id)
                .WithStoreId(s.Id)
                .Create())
            .ToList();

        return builder
            .WithAvailableAt(avs);
    }

    public ItemEntityBuilder FillItem(ItemEntityBuilder builder)
    {
        var avs = _stores
            .Select(s => new AvailableAtEntityBuilder()
                .WithDefaultSectionId(s.Sections.ChooseRandom().Id)
                .WithStoreId(s.Id)
                .Create())
            .ToList();

        builder = builder
            .WithAvailableAt(avs);

        if (_itemCategory is not null)
            builder = builder.WithItemCategoryId(_itemCategory.Id);

        if (_manufacturer is not null)
            builder = builder.WithManufacturerId(_manufacturer.Id);

        if (_quantityType is not null)
        {
            builder = builder.WithQuantityType(_quantityType.Value.ToInt());
            builder = _quantityTypeInPacket is not null
                ? builder.WithQuantityInPacket(_quantityTypeInPacket.Value.ToInt()).WithQuantityInPacket(_quantityInPacket)
                : builder.WithoutQuantityInPacket().WithoutQuantityInPacket();
        }

        return builder;
    }
}