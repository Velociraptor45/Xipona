using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Updates;

public class ItemUpdate
{
    private readonly IEnumerable<ItemAvailability> _availabilities;

    public ItemUpdate(ItemId oldId, ItemName name, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        IEnumerable<ItemAvailability> availabilities)
    {
        OldId = oldId;
        Name = name;
        Comment = comment;
        ItemQuantity = itemQuantity;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        _availabilities = availabilities;
    }

    public ItemId OldId { get; }
    public ItemName Name { get; }
    public Comment Comment { get; }
    public ItemQuantity ItemQuantity { get; }
    public QuantityType QuantityType { get; }
    public float QuantityInPacket { get; }
    public QuantityTypeInPacket QuantityTypeInPacket { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public ManufacturerId? ManufacturerId { get; }

    public IReadOnlyCollection<ItemAvailability> Availabilities => _availabilities.ToList().AsReadOnly();
}