namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

public interface IItemTypeFactory
{
    IItemType Create(ItemTypeId id, ItemTypeName name, IEnumerable<ItemAvailability> availabilities,
        ItemTypeId? predecessorId, bool isDeleted, DateTimeOffset createAt);

    IItemType CreateNew(ItemTypeName name, IEnumerable<ItemAvailability> availabilities, ItemTypeId? predecessorId);

    IItemType CreateNew(ItemTypeName name, IEnumerable<ItemAvailability> availabilities);
}