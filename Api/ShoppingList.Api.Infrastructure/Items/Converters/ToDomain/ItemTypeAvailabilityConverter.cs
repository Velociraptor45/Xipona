using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Items.Converters.ToDomain;

public class ItemTypeAvailabilityConverter : IToDomainConverter<ItemTypeAvailableAt, IItemAvailability>
{
    private readonly IItemAvailabilityFactory _itemAvailabilityFactory;

    public ItemTypeAvailabilityConverter(IItemAvailabilityFactory itemAvailabilityFactory)
    {
        _itemAvailabilityFactory = itemAvailabilityFactory;
    }

    public IItemAvailability ToDomain(ItemTypeAvailableAt source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return _itemAvailabilityFactory.Create(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}