using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Modifications;

public class ItemWithTypesModification
{
    public ItemWithTypesModification(ItemId id, ItemName name, Comment comment,
        QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        IEnumerable<ItemTypeModification> itemTypes)
    {
        Id = id;
        Name = name;
        Comment = comment;
        QuantityType = quantityType;
        QuantityInPacket = quantityInPacket;
        QuantityTypeInPacket = quantityTypeInPacket;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        ItemTypes = itemTypes?.ToList() ?? throw new ArgumentNullException(nameof(itemTypes));
    }

    public ItemId Id { get; }
    public ItemName Name { get; }
    public Comment Comment { get; }
    public QuantityType QuantityType { get; }
    public float QuantityInPacket { get; }
    public QuantityTypeInPacket QuantityTypeInPacket { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public ManufacturerId? ManufacturerId { get; }
    public IReadOnlyCollection<ItemTypeModification> ItemTypes { get; }
}