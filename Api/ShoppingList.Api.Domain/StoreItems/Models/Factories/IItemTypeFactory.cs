namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

public interface IItemTypeFactory
{
    IItemType Create(ItemTypeId id, ItemTypeName name, IEnumerable<IStoreItemAvailability> availabilities,
        IItemType? predecessor);

    IItemType CreateNew(ItemTypeName name, IEnumerable<IStoreItemAvailability> availabilities, IItemType? predecessor);

    IItemType CreateNew(ItemTypeName name, IEnumerable<IStoreItemAvailability> availabilities);
}