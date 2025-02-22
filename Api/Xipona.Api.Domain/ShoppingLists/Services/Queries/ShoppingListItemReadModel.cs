using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries.Quantities;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;

public class ShoppingListItemReadModel
{
    public ShoppingListItemReadModel(ItemId id, ItemTypeId? typeId, string name, bool isDeleted, Comment comment,
        bool isTemporary, Price pricePerQuantity, QuantityTypeReadModel quantityType, Quantity? quantityInPacket,
        QuantityTypeInPacketReadModel? quantityTypeInPacket, ItemCategoryReadModel? itemCategory,
        ManufacturerReadModel? manufacturer, bool isInBasket, QuantityInBasket quantity, bool isDiscounted)
    {
        Id = id;
        TypeId = typeId;
        Name = name;
        IsDeleted = isDeleted;
        Comment = comment;
        IsTemporary = isTemporary;
        PricePerQuantity = pricePerQuantity;
        QuantityType = quantityType;
        QuantityInPacket = quantityInPacket;
        QuantityTypeInPacket = quantityTypeInPacket;
        ItemCategory = itemCategory;
        Manufacturer = manufacturer;
        IsInBasket = isInBasket;
        Quantity = quantity;
        IsDiscounted = isDiscounted;
    }

    public ItemId Id { get; }
    public ItemTypeId? TypeId { get; }
    public string Name { get; }
    public bool IsDeleted { get; }
    public Comment Comment { get; }
    public bool IsTemporary { get; }
    public Price PricePerQuantity { get; }
    public QuantityTypeReadModel QuantityType { get; }
    public Quantity? QuantityInPacket { get; }
    public QuantityTypeInPacketReadModel? QuantityTypeInPacket { get; }
    public ItemCategoryReadModel? ItemCategory { get; }
    public ManufacturerReadModel? Manufacturer { get; }
    public bool IsInBasket { get; }
    public QuantityInBasket Quantity { get; }
    public bool IsDiscounted { get; }
}