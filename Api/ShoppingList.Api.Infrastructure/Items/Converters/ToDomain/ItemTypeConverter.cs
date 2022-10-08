using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Items.Converters.ToDomain;

public class ItemTypeConverter : IToDomainConverter<Entities.ItemType, IItemType>
{
    private readonly IItemTypeFactory _itemTypeFactory;
    private readonly IToDomainConverter<ItemTypeAvailableAt, IItemAvailability> _itemTypeAvailabilityConverter;

    public ItemTypeConverter(IItemTypeFactory itemTypeFactory,
        IToDomainConverter<ItemTypeAvailableAt, IItemAvailability> itemTypeAvailabilityConverter)
    {
        _itemTypeFactory = itemTypeFactory;
        _itemTypeAvailabilityConverter = itemTypeAvailabilityConverter;
    }

    public IItemType ToDomain(Entities.ItemType source)
    {
        IItemType? predecessor = source.PredecessorId is null || source.Predecessor is null
            ? null
            : ToDomain(source.Predecessor!);

        var typeAvailabilities = _itemTypeAvailabilityConverter.ToDomain(source.AvailableAt);
        return _itemTypeFactory.Create(new ItemTypeId(source.Id), new ItemTypeName(source.Name), typeAvailabilities,
            predecessor);
    }
}