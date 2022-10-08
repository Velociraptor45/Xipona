using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;

public class ItemAvailabilityConverter : IToDomainConverter<ItemAvailabilityContract, IItemAvailability>
{
    private readonly IItemAvailabilityFactory _itemAvailabilityFactory;

    public ItemAvailabilityConverter(IItemAvailabilityFactory itemAvailabilityFactory)
    {
        _itemAvailabilityFactory = itemAvailabilityFactory;
    }

    public IItemAvailability ToDomain(ItemAvailabilityContract source)
    {
        return _itemAvailabilityFactory.Create(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}