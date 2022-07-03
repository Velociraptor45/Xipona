using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;

public class ItemUpdate
{
    private readonly IEnumerable<IItemAvailability> _availabilities;

    public ItemUpdate(ItemId oldId, ItemName name, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        IEnumerable<IItemAvailability> availabilities)
    {
        OldId = oldId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        ItemQuantity = itemQuantity ?? throw new ArgumentNullException(nameof(itemQuantity));
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        _availabilities = availabilities ?? throw new ArgumentNullException(nameof(availabilities));
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

    public IReadOnlyCollection<IItemAvailability> Availabilities => _availabilities.ToList().AsReadOnly();
}