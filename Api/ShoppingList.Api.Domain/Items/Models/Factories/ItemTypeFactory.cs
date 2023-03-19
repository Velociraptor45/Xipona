namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

public class ItemTypeFactory : IItemTypeFactory
{
    public IItemType Create(ItemTypeId id, ItemTypeName name, IEnumerable<IItemAvailability> availabilities,
        ItemTypeId? predecessorId, bool isDeleted)
    {
        var type = new ItemType(id, name, availabilities, predecessorId, isDeleted);
        return type;
    }

    public IItemType CreateNew(ItemTypeName name, IEnumerable<IItemAvailability> availabilities)
    {
        return Create(ItemTypeId.New, name, availabilities, null, false);
    }

    public IItemType CreateNew(ItemTypeName name, IEnumerable<IItemAvailability> availabilities,
        ItemTypeId? predecessorId)
    {
        return Create(ItemTypeId.New, name, availabilities, predecessorId, false);
    }

    public IItemType CloneWithNewId(IItemType itemType)
    {
        return new ItemType(
            ItemTypeId.New,
            itemType.Name,
            itemType.Availabilities,
            itemType.PredecessorId,
            itemType.IsDeleted);
    }
}