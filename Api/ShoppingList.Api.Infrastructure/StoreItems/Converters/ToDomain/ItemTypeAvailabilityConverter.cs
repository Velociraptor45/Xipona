using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToDomain;

public class ItemTypeAvailabilityConverter : IToDomainConverter<ItemTypeAvailableAt, IItemAvailability>
{
    private readonly IItemAvailabilityFactory _storeItemAvailabilityFactory;

    public ItemTypeAvailabilityConverter(IItemAvailabilityFactory storeItemAvailabilityFactory)
    {
        _storeItemAvailabilityFactory = storeItemAvailabilityFactory;
    }

    public IItemAvailability ToDomain(ItemTypeAvailableAt source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return _storeItemAvailabilityFactory.Create(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}