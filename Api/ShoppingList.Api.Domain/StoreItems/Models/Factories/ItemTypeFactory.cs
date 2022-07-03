namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

public class ItemTypeFactory : IItemTypeFactory
{
    public IItemType Create(ItemTypeId id, ItemTypeName name, IEnumerable<IItemAvailability> availabilities,
        IItemType? predecessor)
    {
        var type = new ItemType(id, name, availabilities);
        if (predecessor != null)
            type.SetPredecessor(predecessor);

        return type;
    }

    public IItemType CreateNew(ItemTypeName name, IEnumerable<IItemAvailability> availabilities)
    {
        return Create(ItemTypeId.New, name, availabilities, null);
    }

    public IItemType CreateNew(ItemTypeName name, IEnumerable<IItemAvailability> availabilities,
        IItemType? predecessor)
    {
        return Create(ItemTypeId.New, name, availabilities, predecessor);
    }
}