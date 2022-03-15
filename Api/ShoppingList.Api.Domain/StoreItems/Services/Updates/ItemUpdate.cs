using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

public class ItemUpdate
{
    private readonly IEnumerable<IStoreItemAvailability> _availabilities;

    public ItemUpdate(ItemId oldId, ItemName name, Comment comment,
        QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        IEnumerable<IStoreItemAvailability> availabilities)
    {
        OldId = oldId;
        Name = name;
        Comment = comment;
        QuantityType = quantityType;
        QuantityInPacket = quantityInPacket;
        QuantityTypeInPacket = quantityTypeInPacket;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        _availabilities = availabilities ?? throw new ArgumentNullException(nameof(availabilities));
    }

    public ItemId OldId { get; }
    public ItemName Name { get; }
    public Comment Comment { get; }
    public QuantityType QuantityType { get; }
    public float QuantityInPacket { get; }
    public QuantityTypeInPacket QuantityTypeInPacket { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public ManufacturerId? ManufacturerId { get; }

    public IReadOnlyCollection<IStoreItemAvailability> Availabilities => _availabilities.ToList().AsReadOnly();
}