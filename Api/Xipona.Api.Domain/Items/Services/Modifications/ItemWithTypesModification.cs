using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Items.Services.Modifications;

public class ItemWithTypesModification
{
    public ItemWithTypesModification(ItemId id, ItemName name, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        IEnumerable<ItemTypeModification> itemTypes)
    {
        Id = id;
        Name = name;
        Comment = comment;
        ItemQuantity = itemQuantity;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        ItemTypes = itemTypes.ToList();
    }

    public ItemId Id { get; }
    public ItemName Name { get; }
    public Comment Comment { get; }
    public ItemQuantity ItemQuantity { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public ManufacturerId? ManufacturerId { get; }
    public IReadOnlyCollection<ItemTypeModification> ItemTypes { get; }
}