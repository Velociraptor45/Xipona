using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.TemporaryItems;

public class PermanentItem
{
    private readonly IEnumerable<ItemAvailability> _availabilities;

    public PermanentItem(ItemId id, ItemName name, Comment comment, ItemQuantity itemQuantity, ItemCategoryId itemCategoryId,
        ManufacturerId? manufacturerId, IEnumerable<ItemAvailability> availabilities)
    {
        Id = id;
        Name = name;
        Comment = comment;
        ItemQuantity = itemQuantity;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        _availabilities = availabilities;
    }

    public IReadOnlyCollection<ItemAvailability> Availabilities => _availabilities.ToList().AsReadOnly();

    public ItemId Id { get; }
    public ItemName Name { get; }
    public Comment Comment { get; }
    public ItemQuantity ItemQuantity { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public ManufacturerId? ManufacturerId { get; }
}