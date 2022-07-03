using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class CreateItemTypeConverter : IToDomainConverter<CreateItemTypeContract, IItemType>
{
    private readonly IItemTypeFactory _itemTypeFactory;
    private readonly IToDomainConverter<ItemAvailabilityContract, IItemAvailability> _itemAvailabilityConverter;

    public CreateItemTypeConverter(IItemTypeFactory itemTypeFactory,
        IToDomainConverter<ItemAvailabilityContract, IItemAvailability> itemAvailabilityConverter)
    {
        _itemTypeFactory = itemTypeFactory;
        _itemAvailabilityConverter = itemAvailabilityConverter;
    }

    public IItemType ToDomain(CreateItemTypeContract source)
    {
        return _itemTypeFactory.CreateNew(
            new ItemTypeName(source.Name),
            _itemAvailabilityConverter.ToDomain(source.Availabilities),
            null);
    }
}