using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

public class ItemWithTypesUpdate
{
    public ItemWithTypesUpdate(ItemId oldId, ItemName name, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, IEnumerable<ItemTypeUpdate> typeUpdates)
    {
        OldId = oldId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        ItemQuantity = itemQuantity ?? throw new ArgumentNullException(nameof(itemQuantity));
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        TypeUpdates = typeUpdates?.ToList() ?? throw new ArgumentNullException(nameof(typeUpdates));
    }

    public ItemId OldId { get; }
    public ItemName Name { get; }
    public Comment Comment { get; }
    public ItemQuantity ItemQuantity { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public ManufacturerId? ManufacturerId { get; }
    public IReadOnlyCollection<ItemTypeUpdate> TypeUpdates { get; }
}