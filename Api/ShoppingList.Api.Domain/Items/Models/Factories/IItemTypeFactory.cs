namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

public interface IItemTypeFactory
{
    IItemType Create(ItemTypeId id, ItemTypeName name, IEnumerable<IItemAvailability> availabilities,
        ItemTypeId? predecessorId, bool isDeleted);

    IItemType CreateNew(ItemTypeName name, IEnumerable<IItemAvailability> availabilities, ItemTypeId? predecessorId);

    IItemType CreateNew(ItemTypeName name, IEnumerable<IItemAvailability> availabilities);

    IItemType CloneWithNewId(IItemType itemType);
}