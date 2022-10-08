using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Items.Converters.ToDomain;

public class ItemAvailabilityConverter : IToDomainConverter<AvailableAt, IItemAvailability>
{
    private readonly IItemAvailabilityFactory _itemAvailabilityFactory;

    public ItemAvailabilityConverter(IItemAvailabilityFactory itemAvailabilityFactory)
    {
        _itemAvailabilityFactory = itemAvailabilityFactory;
    }

    public IItemAvailability ToDomain(AvailableAt source)
    {
        return _itemAvailabilityFactory.Create(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}