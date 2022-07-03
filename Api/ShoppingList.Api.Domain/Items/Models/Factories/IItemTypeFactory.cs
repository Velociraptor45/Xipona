namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

public interface IItemTypeFactory
{
    IItemType Create(ItemTypeId id, ItemTypeName name, IEnumerable<IItemAvailability> availabilities,
        IItemType? predecessor);

    IItemType CreateNew(ItemTypeName name, IEnumerable<IItemAvailability> availabilities, IItemType? predecessor);

    IItemType CreateNew(ItemTypeName name, IEnumerable<IItemAvailability> availabilities);
}