using ProjectHermes.ShoppingList.Api.Core.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

public class ItemTypeFactory : IItemTypeFactory
{
    private readonly IDateTimeService _dateTimeService;

    public ItemTypeFactory(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public IItemType Create(ItemTypeId id, ItemTypeName name, IEnumerable<ItemAvailability> availabilities,
        ItemTypeId? predecessorId, bool isDeleted, DateTimeOffset createAt)
    {
        var type = new ItemType(id, name, availabilities, predecessorId, isDeleted, createAt);
        return type;
    }

    public IItemType CreateNew(ItemTypeName name, IEnumerable<ItemAvailability> availabilities)
    {
        return Create(ItemTypeId.New, name, availabilities, null, false, _dateTimeService.UtcNow);
    }

    public IItemType CreateNew(ItemTypeName name, IEnumerable<ItemAvailability> availabilities,
        ItemTypeId? predecessorId)
    {
        return Create(ItemTypeId.New, name, availabilities, predecessorId, false, _dateTimeService.UtcNow);
    }
}