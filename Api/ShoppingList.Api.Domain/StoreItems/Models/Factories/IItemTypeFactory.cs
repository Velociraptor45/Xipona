namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

public interface IItemTypeFactory
{
    IItemType Create(ItemTypeId id, string name, IEnumerable<IStoreItemAvailability> availabilities,
        IItemType? predecessor);

    IItemType CreateNew(string name, IEnumerable<IStoreItemAvailability> availabilities, IItemType? predecessor);
}