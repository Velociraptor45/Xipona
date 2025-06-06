using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Items.Services.Updates;

public class ItemWithTypesUpdate
{
    public ItemWithTypesUpdate(ItemId oldId, ItemName name, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, IEnumerable<ItemTypeUpdate> typeUpdates)
    {
        OldId = oldId;
        Name = name;
        Comment = comment;
        ItemQuantity = itemQuantity;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        TypeUpdates = typeUpdates.ToList();
    }

    public ItemId OldId { get; }
    public ItemName Name { get; }
    public Comment Comment { get; }
    public ItemQuantity ItemQuantity { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public ManufacturerId? ManufacturerId { get; }
    public IReadOnlyCollection<ItemTypeUpdate> TypeUpdates { get; }
}