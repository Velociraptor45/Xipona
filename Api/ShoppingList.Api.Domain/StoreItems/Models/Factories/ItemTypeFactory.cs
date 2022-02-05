using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

public class ItemTypeFactory : IItemTypeFactory
{
    public IItemType Create(ItemTypeId id, string name, IEnumerable<IStoreItemAvailability> availabilities,
        IItemType? predecessor)
    {
        var type = new ItemType(id, name, availabilities);
        if(predecessor != null)
            type.SetPredecessor(predecessor);

        return type;
    }

    public IItemType CreateNew(string name, IEnumerable<IStoreItemAvailability> availabilities,
        IItemType? predecessor)
    {
        return Create(ItemTypeId.New, name, availabilities, predecessor);
    }
}