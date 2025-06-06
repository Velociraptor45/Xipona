using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Domain.Items.Services.Creations;

public class ItemCreation
{
    private readonly IEnumerable<ItemAvailability> _availabilities;

    public ItemCreation(ItemName name, Comment comment, ItemQuantity itemQuantity, ItemCategoryId itemCategoryId,
        ManufacturerId? manufacturerId, IEnumerable<ItemAvailability> availabilities)
    {
        Name = name;
        Comment = comment;
        ItemQuantity = itemQuantity;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        _availabilities = availabilities;
    }

    public ItemName Name { get; }
    public Comment Comment { get; }
    public ItemQuantity ItemQuantity { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public ManufacturerId? ManufacturerId { get; }

    public IReadOnlyCollection<ItemAvailability> Availabilities => _availabilities.ToList().AsReadOnly();
}