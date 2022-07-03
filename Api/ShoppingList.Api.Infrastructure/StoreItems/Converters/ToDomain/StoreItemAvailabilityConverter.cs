using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToDomain;

public class StoreItemAvailabilityConverter : IToDomainConverter<AvailableAt, IItemAvailability>
{
    private readonly IItemAvailabilityFactory _storeItemAvailabilityFactory;

    public StoreItemAvailabilityConverter(IItemAvailabilityFactory storeItemAvailabilityFactory)
    {
        _storeItemAvailabilityFactory = storeItemAvailabilityFactory;
    }

    public IItemAvailability ToDomain(AvailableAt source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return _storeItemAvailabilityFactory.Create(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}