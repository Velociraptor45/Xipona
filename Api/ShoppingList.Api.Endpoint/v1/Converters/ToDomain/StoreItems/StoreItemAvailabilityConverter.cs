using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class StoreItemAvailabilityConverter : IToDomainConverter<ItemAvailabilityContract, IItemAvailability>
{
    private readonly IItemAvailabilityFactory _storeItemAvailabilityFactory;

    public StoreItemAvailabilityConverter(IItemAvailabilityFactory storeItemAvailabilityFactory)
    {
        _storeItemAvailabilityFactory = storeItemAvailabilityFactory;
    }

    public IItemAvailability ToDomain(ItemAvailabilityContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return _storeItemAvailabilityFactory.Create(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}