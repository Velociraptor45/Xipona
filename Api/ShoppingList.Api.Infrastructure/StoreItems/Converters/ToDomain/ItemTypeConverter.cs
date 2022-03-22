using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToDomain;

public class ItemTypeConverter : IToDomainConverter<Entities.ItemType, IItemType>
{
    private readonly IItemTypeFactory _itemTypeFactory;
    private readonly IToDomainConverter<ItemTypeAvailableAt, IStoreItemAvailability> _itemTypeAvailabilityConverter;

    public ItemTypeConverter(IItemTypeFactory itemTypeFactory,
        IToDomainConverter<ItemTypeAvailableAt, IStoreItemAvailability> itemTypeAvailabilityConverter)
    {
        _itemTypeFactory = itemTypeFactory;
        _itemTypeAvailabilityConverter = itemTypeAvailabilityConverter;
    }

    public IItemType ToDomain(Entities.ItemType source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        IItemType? predecessor = source.PredecessorId is null ? null : ToDomain(source.Predecessor!);

        var typeAvailabilities = _itemTypeAvailabilityConverter.ToDomain(source.AvailableAt);
        return _itemTypeFactory.Create(new ItemTypeId(source.Id), new ItemTypeName(source.Name), typeAvailabilities,
            predecessor);
    }
}