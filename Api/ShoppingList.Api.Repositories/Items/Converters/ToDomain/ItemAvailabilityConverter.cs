using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToDomain;

public class ItemAvailabilityConverter : IToDomainConverter<AvailableAt, ItemAvailability>
{
    public ItemAvailability ToDomain(AvailableAt source)
    {
        return new ItemAvailability(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}