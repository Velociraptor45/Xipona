using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries.Quantities;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

public class ItemReadModel
{
    public ItemReadModel(ItemId id, ItemName name, bool isDeleted, Comment comment, bool isTemporary,
        QuantityTypeReadModel quantityType, Quantity? quantityInPacket, QuantityTypeInPacketReadModel? quantityTypeInPacket,
        ItemCategoryReadModel? itemCategory, ManufacturerReadModel? manufacturer,
        IEnumerable<ItemAvailabilityReadModel> availabilities, IEnumerable<ItemTypeReadModel> itemTypes)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        Comment = comment;
        IsTemporary = isTemporary;
        QuantityType = quantityType;
        QuantityInPacket = quantityInPacket;
        QuantityTypeInPacket = quantityTypeInPacket;
        ItemCategory = itemCategory;
        Manufacturer = manufacturer;
        Availabilities = availabilities.ToList();
        ItemTypes = itemTypes.ToList();
    }

    public ItemId Id { get; }
    public ItemName Name { get; }
    public bool IsDeleted { get; }
    public Comment Comment { get; }
    public bool IsTemporary { get; }
    public QuantityTypeReadModel QuantityType { get; }
    public Quantity? QuantityInPacket { get; }
    public QuantityTypeInPacketReadModel? QuantityTypeInPacket { get; }
    public ItemCategoryReadModel? ItemCategory { get; }
    public ManufacturerReadModel? Manufacturer { get; }
    public IReadOnlyCollection<ItemAvailabilityReadModel> Availabilities { get; }
    public IReadOnlyCollection<ItemTypeReadModel> ItemTypes { get; }
}